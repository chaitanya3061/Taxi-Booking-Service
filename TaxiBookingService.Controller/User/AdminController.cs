//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System.Net;
//using System.Security.Authentication;
//using TaxiBookingService.API.User.Admin;
//using TaxiBookingService.Common.AssetManagement.Common;
////using TaxiBookingService.Common.Enums;
//using TaxiBookingService.Common.Utilities;
//using TaxiBookingService.Dal.Entities;
//using TaxiBookingService.Logic.User.Interfaces;
//using static TaxiBookingService.Common.CustomException;

//namespace TaxiBookingService.Controller.User
//{
//    [Route("api/Admin")]
//    [ApiController]

//    public class AdminController : ControllerBase
//    {
//        private readonly IAdminLogic<Admin> _AdminLogic;
//        private readonly ILoggerAdapter _logger;

//        public AdminController(IAdminLogic<Admin> AdminService, ILoggerAdapter logger)
//        {
//            _AdminLogic = AdminService;
//            _logger = logger;
//        }

//        [HttpPost("login")]
//        public async Task<ActionResult> Login(AdminLoginServiceContracts request)
//        {
//            try
//            {
//                var result = await _AdminLogic.Login(request);
//                _logger.LogInformation($"{AppConstant.LoginSuccess} {request.Email}");
//                return Ok(result);
//            }
//            catch (Common.CustomException.AuthenticationException ex)
//            {
//                _logger.LogError($"{AppConstant.Error}: {ex.Message}", ex);
//                return Unauthorized($"{AppConstant.Error}: {ex.Message}");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"{AppConstant.Error}:{ex.Message}", ex);
//                return StatusCode((int)AppConstant.ServerError, $"{AppConstant.Error}: {ex.Message}");
//            }
//        }

//        [HttpPost("refresh-token")]
//        public async Task<IActionResult> RefeshToken()
//        {
//            try
//            {
//                var result = await _AdminLogic.RefreshToken();
//                _logger.LogInformation(AppConstant.TokenRefreshSuccess);
//                return Ok(result);
//            }
//            catch (TokenExpiredException ex)
//            {
//                _logger.LogError($"{AppConstant.Error}:{ex.Message}", ex);
//                return BadRequest($"{AppConstant.TokenExpired} : {ex.Message}");
//            }
//            catch (InvalidTokenException ex)
//            {
//                _logger.LogError($"{AppConstant.Error} :{ex.Message}", ex);
//                return Unauthorized($"{AppConstant.Error}: {ex.Message}");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
//                return StatusCode((int)AppConstant.ServerError, $"{AppConstant.Error}: {ex.Message}");
//            }

//        }
//        [HttpPost("logout")]
//        public async Task<IActionResult> Logout()
//        {
//            try
//            {
//                await _AdminLogic.Logout();
//                _logger.LogInformation(AppConstant.LogoutSuccess);
//                return Ok(AppConstant.LogoutSuccess);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
//                return StatusCode((int)AppConstant.ServerError, $"{AppConstant.Error}: {ex.Message}");
//            }
//        }
//    }
//}
