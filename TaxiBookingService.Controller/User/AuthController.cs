using Microsoft.AspNetCore.Mvc;
using static TaxiBookingService.Common.CustomException;
using TaxiBookingService.Common.AssetManagement.Common;
using TaxiBookingService.Common.Utilities;
using TaxiBookingService.Logic.User.Interfaces;
using TaxiBookingService.API.User.Driver;
using TaxiBookingService.Common.Enums;

namespace TaxiBookingService.Controller.User
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserLogic _UserLogic;
        private readonly ILoggerAdapter _logger;

        public AuthController(IUserLogic userLogic, ILoggerAdapter logger)
        {
            _UserLogic = userLogic;
            _logger = logger;
        }
    
        [HttpPost("login")]
        public async Task<ActionResult> Login(UserLoginDto request)
        {
            try
            {
                Console.Write(UserRole.Driver);
                var accessToken = await _UserLogic.Login(request); 
               _logger.LogInformation($"{AppConstant.LoginSuccess} {request.Email}");
                return Ok($"{accessToken}");
            }
            catch(NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Common.CustomException.AuthenticationException ex)
            {
                return Unauthorized($"{ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $" {ex.Message}");
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefeshToken()
        {
            try
            {
                var result = await _UserLogic.RefreshToken();
                _logger.LogInformation(AppConstant.TokenRefreshSuccess);
                return Ok(result);
            }
            catch (TokenExpiredException ex)
            {
                return BadRequest($"{AppConstant.TokenExpired} : {ex.Message}");
            }
            catch (InvalidTokenException ex)
            {
                return Unauthorized($"{AppConstant.InvalidToken}: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $" {ex.Message}");
            }

        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _UserLogic.Logout();
                _logger.LogInformation(AppConstant.LogoutSuccess);
                return Ok(AppConstant.LogoutSuccess);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $" {ex.Message}");
            }
        }
    }
}
