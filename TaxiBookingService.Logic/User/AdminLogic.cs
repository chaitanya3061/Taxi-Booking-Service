using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using TaxiBookingService.API.Ride;
using TaxiBookingService.API.User.Admin;
using TaxiBookingService.Common.AssetManagement.Common;
using TaxiBookingService.Common.Utilities;
using TaxiBookingService.Dal.Entities;
using TaxiBookingService.Dal.Interfaces;
using TaxiBookingService.Logic.User.Interfaces;
using static TaxiBookingService.Common.CustomException;

namespace TaxiBookingService.Logic.User
{
    public class AdminLogic : IAdminLogic
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILoggerAdapter _loggerAdapter;

        public AdminLogic(IUnitOfWork unitOfWork, IConfiguration configuration, IHttpContextAccessor httpContextAccessor,IMapper mapper, ILoggerAdapter loggerAdapter)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _mapper=mapper;
            _loggerAdapter = loggerAdapter;
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
            var existingReason = await _unitOfWork.RideCancellationReasonRepository.GetById(Id);

            if (existingReason == null)
            {
                throw new NotFoundException(AppConstant.ReasonNotFound, _loggerAdapter);
            }

            await _unitOfWork.RideCancellationReasonRepository.Delete(existingReason);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<int> UpdateCancellationReason(AdminManageReasonDto request,int Id)
        {
            var existingReason = await _unitOfWork.RideCancellationReasonRepository.GetById(Id);

            if (existingReason == null)
            {
                throw new NotFoundException(AppConstant.ReasonNotFound, _loggerAdapter);
            }

            _mapper.Map(request, existingReason);
            await _unitOfWork.RideCancellationReasonRepository.Update(existingReason);
            await _unitOfWork.SaveChangesAsync();
            return existingReason.Id;
        }

        public async Task<bool> DeleteUser(int Id)
        {
            var exisitingUser = await _unitOfWork.UserRepository.GetById(Id);

            if (exisitingUser == null) { 
                throw new NotFoundException(AppConstant.UserNotFound,_loggerAdapter); 
            }

            await _unitOfWork.UserRepository.Delete(exisitingUser);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<int> UpdateUser(AdminManageUserDto request, int Id)
        {
            var exisitingUser = await _unitOfWork.UserRepository.GetById(Id);

            if (exisitingUser == null) { 
                throw new NotFoundException(AppConstant.UserNotFound,_loggerAdapter); 
            }

            _mapper.Map(request, exisitingUser);
            await _unitOfWork.UserRepository.Update(exisitingUser);
            await _unitOfWork.SaveChangesAsync();
            return exisitingUser.Id;
        }

        public async Task<List<Ride>> GetCustomerRides(int CustomerId, RideQueryParametersDto request)
        {
            var result = await _unitOfWork.RideRepository.GetCustomerRides(CustomerId, request);
            return result;
        }
    }
}
