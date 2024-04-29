using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaxiBookingService.API.User.Admin;
using TaxiBookingService.Common.AssetManagement.Common;
using TaxiBookingService.Common.Utilities;
using TaxiBookingService.Dal.Entities;
using TaxiBookingService.Logic.User.Interfaces;
using static TaxiBookingService.Common.CustomException;

namespace TaxiBookingService.Controller.User
{
    [Route("api/admin")]
    [ApiController]

    public class AdminController : ControllerBase
    {
        private readonly IAdminLogic _AdminLogic;
        private readonly ILoggerAdapter _logger;

        public AdminController(IAdminLogic AdminService, ILoggerAdapter logger)
        {
            _AdminLogic = AdminService;
            _logger = logger;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("reasons/cancellation/add")]
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

        [Authorize(Roles = "Admin")]
        [HttpDelete("reasons/cancellation/{id}")]
        public async Task<IActionResult> DeleteCancellationReason(int id)
        {
            try
            {
                await _AdminLogic.DeleteCancellationReason(id);
                _logger.LogInformation(AppConstant.Delete);
                return Ok(AppConstant.Delete);
            }
            catch (NotFoundException ex)
            {
                return NotFound($"{AppConstant.ReasonNotFound}{ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $"{AppConstant.Error}: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("reasons/cancellation/{id}")]
        public async Task<IActionResult> UpdateCancellationReason(AdminManageReasonDto request,int id)
        {
            try
            {
                await _AdminLogic.UpdateCancellationReason(request,id);
                _logger.LogInformation(AppConstant.Update);
                return Ok(AppConstant.Update);
            }
            catch (NotFoundException ex)
            {
                return NotFound($"{AppConstant.ReasonNotFound}{ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $"{AppConstant.Error}: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _AdminLogic.DeleteUser(id);
                _logger.LogInformation(AppConstant.Delete);
                return Ok(AppConstant.Delete);
            }
            catch (NotFoundException ex)
            {
                return NotFound($"{AppConstant.UserNotFound}{ex}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $"{AppConstant.Error}: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("users/{id}")]
        public async Task<IActionResult> UpdateUser(AdminManageUserDto request, int id)
        {
            try
            {
                await _AdminLogic.UpdateUser(request, id);
                _logger.LogInformation(AppConstant.Update);
                return Ok(AppConstant.Update);
            }
            catch (NotFoundException ex)
            {
                return NotFound($"{AppConstant.UserNotFound}{ex}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $"{AppConstant.Error}: {ex.Message}");
            }
        }
    }
}
