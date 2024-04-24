using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Authentication;
using TaxiBookingService.API.User.Customer;
using TaxiBookingService.API.User.Driver;
using TaxiBookingService.Common.AssetManagement.Common;
//using TaxiBookingService.Common.Enums;
using TaxiBookingService.Common.Utilities;
using TaxiBookingService.Dal.Entities;
using TaxiBookingService.Logic.User;
using TaxiBookingService.Logic.User.Interfaces;
using static TaxiBookingService.Common.CustomException;

namespace TaxiBookingService.Controller.User
{
    [Route("api/customer")]
    [ApiController]

    public class CustomerController : ControllerBase
    {
        private readonly ICustomerLogic<Customer> _CustomerLogic;
        private readonly ILoggerAdapter _logger;

        public CustomerController(ICustomerLogic<Customer> CustomerService, ILoggerAdapter logger)
        {
            _CustomerLogic = CustomerService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(CustomerRegisterDto request)
        {
            try
            {
                var userId = await _CustomerLogic.Register(request);
                _logger.LogInformation(AppConstant.RegistrationSuccess);
                return Ok($"{AppConstant.RegistrationSuccess} id: {userId}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}:{ex.Message}", ex);
                return BadRequest();
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(CustomerLoginDto request)
        {
            try
            {
                var (accessToken, refreshToken) = await _CustomerLogic.Login(request);

                var tokenResponse = new
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };
                _logger.LogInformation($"{AppConstant.LoginSuccess} {request.Email}");
                return Ok(tokenResponse);

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

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefeshToken()
        {
            try
            {
                var result = await _CustomerLogic.RefreshToken();
                _logger.LogInformation(AppConstant.TokenRefreshSuccess);
                return Ok(result);
            }
            catch (TokenExpiredException ex)
            {
                _logger.LogError($"{AppConstant.Error}:{ex.Message}", ex);
                return BadRequest($"{AppConstant.TokenExpired} : {ex.Message}");
            }
            catch (InvalidTokenException ex)
            {
                _logger.LogError($"{AppConstant.Error} :{ex.Message}", ex);
                return Unauthorized($"{AppConstant.Error}: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $"{AppConstant.Error}: {ex.Message}");
            }

        }


        //[Authorize(Roles = "Customer")]
        [HttpPost("bookride")]
        public async Task<IActionResult> BookRide(CustomerBookRideDto request)
        {
            try
            {
                var result = await _CustomerLogic.BookRide(request);
                _logger.LogInformation(AppConstant.RequestSended);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $"{AppConstant.Error}: {ex.Message}");
            }

        }


        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _CustomerLogic.Logout();
                _logger.LogInformation(AppConstant.LogoutSuccess);
                return Ok(AppConstant.LogoutSuccess);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $"{AppConstant.Error}: {ex.Message}");
            }
        }

        [HttpPatch("cancelride/{rideId}")]
        public async Task<IActionResult> CancelRide(int rideId, string reason)
        {
            try
            {
                await _CustomerLogic.CancelRide(rideId, reason);
                return Ok($"{AppConstant.DriverCancelled}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $"{AppConstant.Error}: {ex.Message}");
            }
        }

        [HttpGet("getdriver/{rideId}")]
        public async Task<IActionResult> GetDriver(int rideId)
        {
            try
            {
                var results = await _CustomerLogic.GetDriverAsync(rideId);
                return Ok(results);
            }
            catch (NotFoundException ex)
            {
                _logger.LogError(AppConstant.NodriversFound, ex);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $"{AppConstant.Error}: {ex.Message}");
            }
        }

        [HttpPost("rating/{rideId}")]
        public async Task<IActionResult> FeedBack(CustomerRatingDto rating)
        {
            try
            {
                await _CustomerLogic.FeedBack(rating);
                return Ok($"{AppConstant.Feedback}");
            }
            catch (NotFoundException ex)
            {
                _logger.LogError(AppConstant.RideNotFound, ex);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $"{AppConstant.Error}: {ex.Message}");
            }
        }

        [HttpGet("ridehistory")]
        public async Task<IActionResult> RideHistory()
        {
            try
            {
                var result = await _CustomerLogic.RideHistory();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $"{AppConstant.Error}: {ex.Message}");
            }
        }

        [HttpGet("updatedropOffLocation")]
        public async Task<IActionResult> UpdateDropOffLocation()
        {
            try
            {
                var result = await _CustomerLogic.UpdateDropOffLocation();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $"{AppConstant.Error}: {ex.Message}");
            }
        }
    }
}
