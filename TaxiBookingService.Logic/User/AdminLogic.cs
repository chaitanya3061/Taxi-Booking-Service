using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.API.User.Admin;
using TaxiBookingService.Common;
using TaxiBookingService.Common.AssetManagement.Common;
//using TaxiBookingService.Common.Enums;
using TaxiBookingService.Dal.Entities;
using TaxiBookingService.Dal.Interfaces;
using TaxiBookingService.Logic.User.Interfaces;
using static TaxiBookingService.Common.CustomException;

namespace TaxiBookingService.Logic.User
{
    public class AdminLogic : IAdminLogic<Admin>
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AdminLogic(IUnitOfWork unitOfWork, IConfiguration configuration, IHttpContextAccessor httpContextAccessor,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _mapper=mapper;
        }

        private string CreateToken(Admin admin)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, admin.User.Email),
                new Claim(ClaimTypes.Role, "Admin"), 

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

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        public async Task<string> Login(AdminLoginDto request)
        {
            var admin = await _unitOfWork.AdminRepository.GetByEmail(request.Email);
            if (admin == null)
            {
                throw new Exception("Invalid email or password.");
            }
            if (!VerifyPasswordHash(request.Password, admin.User.PasswordHash, admin.User.PasswordSalt))
            {
                throw new AuthenticationException(AppConstant.PasswordNotFound);
            }
            string token = CreateToken(admin);
            _httpContextAccessor.HttpContext.Response.Cookies.Append("accessToken", token);
            await _unitOfWork.SaveChangesAsync();
            return token;
        }

        public async Task Logout()
        {
          _httpContextAccessor.HttpContext.Response.Cookies.Delete("accessToken");
        }

        public async Task<int> AddCancellationReason(AdminManageReasonDto request)
        {
            var reasonentity=_mapper.Map<RideCancellationReason>(request);
            await _unitOfWork.RideCancellationReasonRepository.Add(reasonentity);
            await _unitOfWork.SaveChangesAsync();
            return reasonentity.Id;
        }

        public async Task<bool> DeleteCancellationReason(int Id)
        {
            var exisitingreason=await _unitOfWork.RideCancellationReasonRepository.GetById(Id);
            await _unitOfWork.RideCancellationReasonRepository.Delete(exisitingreason);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<int> UpdateCancellationReason(AdminManageReasonDto request,int Id)
        {
            var exisitingreason = await _unitOfWork.RideCancellationReasonRepository.GetById(Id);
            _mapper.Map(request, exisitingreason);
            await _unitOfWork.RideCancellationReasonRepository.Update(exisitingreason);
            await _unitOfWork.SaveChangesAsync();
            return exisitingreason.Id;
        }

        public async Task<bool> DeleteUser(int Id)
        {
            var exisitingUser = await _unitOfWork.UserRepository.GetById(Id);
            await _unitOfWork.UserRepository.Delete(exisitingUser);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<int> UpdateUser(AdminManageUserDto request, int Id)
        {
            var exisitingUser = await _unitOfWork.UserRepository.GetById(Id);
            _mapper.Map(request, exisitingUser);
            await _unitOfWork.UserRepository.Update(exisitingUser);
            await _unitOfWork.SaveChangesAsync();
            return exisitingUser.Id;
        }
    }
}
