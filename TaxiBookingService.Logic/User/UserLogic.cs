using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TaxiBookingService.API.User.Driver;
using TaxiBookingService.Common.AssetManagement.Common;
using TaxiBookingService.Common.Utilities;
using TaxiBookingService.Dal.Entities;
using TaxiBookingService.Dal.Interfaces;
using TaxiBookingService.Logic.User.Interfaces;
using static TaxiBookingService.Common.CustomException;


namespace TaxiBookingService.Logic.User
{
    public class UserLogic :IUserLogic
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly ILoggerAdapter _loggerAdapter;
        public UserLogic(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, ILoggerAdapter loggerAdapter)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _loggerAdapter = loggerAdapter;
        }


        private async Task<Dal.Entities.User> GetUserFromToken()
        {
            var loggedInUser = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];
            var user = await _unitOfWork.UserRepository.GetByToken(loggedInUser);

            if (user == null)
            {
                throw new NotFoundException(AppConstant.UserNotFound, _loggerAdapter);
            }

            return user;
        }

        private async Task SetRefreshToken(Dal.Entities.User user, RefreshToken newRefreshToken)
        {
            await _unitOfWork.UserRepository.UpdateRefreshToken(user, newRefreshToken);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires,
            };

            _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);
        }
        private RefreshToken GenerateRefreshToken()
        {
            return new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.UtcNow.AddDays(1)
            };
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateToken(Dal.Entities.User user)
        {
            var role = _unitOfWork.UserRepository.GetRoleById(user.Id).Result;
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role,role.Name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task Logout()
        {
            var user = await GetUserFromToken();
            await _unitOfWork.UserRepository.Logout(user);
            await _unitOfWork.SaveChangesAsync();
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("refreshToken");
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("accessToken");
        }

        public async Task<string> RefreshToken()
        {
            var refreshToken = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];
            var user = await GetUserFromToken();

            if (user.TokenExpires <= DateTime.Now)
            {
                throw new TokenExpiredException(AppConstant.TokenExpired, _loggerAdapter);
            }

            string token = CreateToken(user);
            _httpContextAccessor.HttpContext.Response.Cookies.Append("accessToken", token);
            var newRefreshToken = GenerateRefreshToken();
            await SetRefreshToken(user, newRefreshToken);
            await _unitOfWork.SaveChangesAsync();
            return token;
        }

        public async Task<string> Login(UserLoginDto request)
        {
            var user = await _unitOfWork.UserRepository.GetByEmail(request.Email);

            if (user == null)
            {
                throw new NotFoundException(AppConstant.UserNotFound,_loggerAdapter);
            }
            else if (!VerifyPasswordHash(request.Password, user.PasswordHash,user.PasswordSalt))
            {
                throw new AuthenticationException(AppConstant.WrongPassword, _loggerAdapter);
            }

            await _unitOfWork.UserRepository.Login(user);
            string token = CreateToken(user);
            _httpContextAccessor.HttpContext.Response.Cookies.Append("accessToken", token);
            var refreshToken = GenerateRefreshToken();
            await SetRefreshToken(user, refreshToken);
            await _unitOfWork.SaveChangesAsync();
            return token;
        }
    }
}
