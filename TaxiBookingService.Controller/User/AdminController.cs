using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Authentication;
using TaxiBookingService.API.User.Admin;
using TaxiBookingService.Common.AssetManagement.Common;
//using TaxiBookingService.Common.Enums;
using TaxiBookingService.Common.Utilities;
using TaxiBookingService.Dal.Entities;
using TaxiBookingService.Logic.User.Interfaces;
using static TaxiBookingService.Common.CustomException;

namespace TaxiBookingService.Controller.User
{
    [Route("api/Admin")]
    [ApiController]

    public class AdminController : ControllerBase
    {
        private readonly IAdminLogic<Admin> _AdminLogic;
        private readonly ILoggerAdapter _logger;

        public AdminController(IAdminLogic<Admin> AdminService, ILoggerAdapter logger)
        {
            _AdminLogic = AdminService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(AdminLoginDto request)
        {
            try
            {
                var result = await _AdminLogic.Login(request);
                _logger.LogInformation($"{AppConstant.LoginSuccess} {request.Email}");
                return Ok(result);
            }
            catch (Common.CustomException.AuthenticationException ex)
            {
                _logger.LogError($"{AppConstant.Error}: {ex.Message}", ex);
                return Unauthorized($"{AppConstant.Error}: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}:{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $"{AppConstant.Error}: {ex.Message}");
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _AdminLogic.Logout();
                _logger.LogInformation(AppConstant.LogoutSuccess);
                return Ok(AppConstant.LogoutSuccess);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $"{AppConstant.Error}: {ex.Message}");
            }
        }


        [HttpPost("addcancellationreason")]
        public async Task<IActionResult> AddCancellationReason(AdminManageReasonDto request)
        {
            try
            {
                await _AdminLogic.AddCancellationReason(request);
                _logger.LogInformation(AppConstant.ReasonAdded);
                return Ok(AppConstant.ReasonAdded);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $"{AppConstant.Error}: {ex.Message}");
            }
        }
        [HttpDelete("deletecancellationreason/{id}")]
        public async Task<IActionResult> DeleteCancellationReason(int id)
        {
            try
            {
                await _AdminLogic.DeleteCancellationReason(id);
                _logger.LogInformation(AppConstant.Delete);
                return Ok(AppConstant.Delete);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $"{AppConstant.Error}: {ex.Message}");
            }
        }

        [HttpPut("Updatecancellationreason/{id}")]
        public async Task<IActionResult> UpdateCancellationReason(AdminManageReasonDto request,int id)
        {
            try
            {
                await _AdminLogic.UpdateCancellationReason(request,id);
                _logger.LogInformation(AppConstant.Update);
                return Ok(AppConstant.Update);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $"{AppConstant.Error}: {ex.Message}");
            }
        }


        [HttpDelete("deleteuser/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _AdminLogic.DeleteUser(id);
                _logger.LogInformation(AppConstant.Delete);
                return Ok(AppConstant.Delete);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $"{AppConstant.Error}: {ex.Message}");
            }
        }

        [HttpPut("UpdateUser/{id}")]
        public async Task<IActionResult> UpdateUser(AdminManageUserDto request, int id)
        {
            try
            {
                await _AdminLogic.UpdateUser(request, id);
                _logger.LogInformation(AppConstant.Update);
                return Ok(AppConstant.Update);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $"{AppConstant.Error}: {ex.Message}");
            }
        }

        //[HttpPut("ViewRideHistory")]
        //public async Task<IActionResult> ViewRideHistory()
        //{
        //    try
        //    {
        //        await _AdminLogic.ViewRideHistory();
        //        _logger.LogInformation(AppConstant.RetrievedRideSuccess);
        //        return Ok(AppConstant.UpdateRetrievedRideSuccess);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
        //        return StatusCode((int)AppConstant.ServerError, $"{AppConstant.Error}: {ex.Message}");
        //    }
        //}
    }
}
