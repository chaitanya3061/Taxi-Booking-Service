//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Configuration;
//using Microsoft.IdentityModel.Tokens;
//using System;
//using System.Collections.Generic;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Security.Cryptography;
//using System.Text;
//using System.Threading.Tasks;
//using TaxiBookingService.API.User.Admin;
//using TaxiBookingService.Common.AssetManagement.Common;
////using TaxiBookingService.Common.Enums;
//using TaxiBookingService.Dal.Entities;
//using TaxiBookingService.Dal.Interfaces;
//using TaxiBookingService.Logic.User.Interfaces;
//using static TaxiBookingService.Common.CustomException;

//namespace TaxiBookingService.Logic.User
//{
//    public class AdminLogic : IAdminLogic<Admin>
//    {
//        private readonly IConfiguration _configuration;
//        private readonly IHttpContextAccessor _httpContextAccessor;
//        private readonly IUnitOfWork _unitOfWork;

//        public AdminLogic(IUnitOfWork unitOfWork, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
//        {
//            _unitOfWork = unitOfWork;
//            _configuration = configuration;
//            _httpContextAccessor = httpContextAccessor;
//        }

//        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
//        {
//            using (var hmac = new HMACSHA512())
//            {
//                passwordSalt = hmac.Key;
//                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
//            }
//        }

//        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
//        {
//            using (var hmac = new HMACSHA512(passwordSalt))
//            {
//                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
//                return computedHash.SequenceEqual(passwordHash);
//            }
//        }

//        private string CreateToken(Admin Admin)
//        {
//            var claims = new List<Claim>
//            {
//                new Claim(ClaimTypes.Email, Admin.User.Email),
//                new Claim(ClaimTypes.Role, UserRole.Admin.ToString())
//            };

//            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
//            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

//            var token = new JwtSecurityToken(
//                claims: claims,
//                expires: DateTime.Now.AddMinutes(15),
//                signingCredentials: creds
//            );

//            return new JwtSecurityTokenHandler().WriteToken(token);
//        }

//        private RefreshToken GenerateRefreshToken()
//        {
//            return new RefreshToken
//            {
//                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
//                Expires = DateTime.UtcNow.AddDays(1)
//            };
//        }

//        private async Task SetRefreshToken(Admin Admin, RefreshToken newRefreshToken)
//        {
//            await _unitOfWork.AdminRepository.UpdateRefreshToken(Admin, newRefreshToken);

//            var cookieOptions = new CookieOptions
//            {
//                HttpOnly = true,
//                Expires = newRefreshToken.Expires,
//            };

//            _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);
//        }


//        public async Task<string> Login(AdminLoginServiceContracts request)
//        {
//            var Admin = await _unitOfWork.AdminRepository.GetByEmail(request.Email);
//            if (Admin == null)
//            {
//                throw new Exception(AppConstant.UserNotFound);
//            }



//            if (!VerifyPasswordHash(request.Password, Admin.User.PasswordHash, Admin.User.PasswordSalt))
//            {
//                throw new AuthenticationException(AppConstant.PasswordNotFound);
//            }

//            await _unitOfWork.AdminRepository.Login(Admin);
//            await _unitOfWork.SaveChangesAsync();

//            string token = CreateToken(Admin);
//            _httpContextAccessor.HttpContext.Response.Cookies.Append("accessToken", token);

//            var refreshToken = GenerateRefreshToken();
//            await SetRefreshToken(Admin, refreshToken);
//            await _unitOfWork.SaveChangesAsync();

//            return token;
//        }

//        public async Task<string> RefreshToken()
//        {
//            var refreshToken = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];
//            var Admin = await _unitOfWork.AdminRepository.GetByToken(refreshToken);

//            if (Admin == null)
//            {
//                throw new InvalidTokenException(AppConstant.InvalidToken);
//            }
//            else if (Admin.User.TokenExpires <= DateTime.Now)
//            {
//                throw new TokenExpiredException(AppConstant.TokenExpired);
//            }

//            string token = CreateToken(Admin);
//            _httpContextAccessor.HttpContext.Response.Cookies.Append("accessToken", token);

//            var newRefreshToken = GenerateRefreshToken();
//            await SetRefreshToken(Admin, newRefreshToken);
//            await _unitOfWork.SaveChangesAsync();

//            return token;
//        }

//        public async Task Logout()
//        {
//            var loggedInUser = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];
//            var Admin = await _unitOfWork.AdminRepository.GetByToken(loggedInUser);
//            if (Admin == null)
//            {
//                throw new Exception(AppConstant.UserNotFound);
//            }

//            await _unitOfWork.AdminRepository.Logout(Admin);
//            await _unitOfWork.SaveChangesAsync();

//            _httpContextAccessor.HttpContext.Response.Cookies.Delete("refreshToken");
//            _httpContextAccessor.HttpContext.Response.Cookies.Delete("accessToken");
//        }
//    }
//}
