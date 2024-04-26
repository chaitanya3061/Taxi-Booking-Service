using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Security.Authentication;
using System.Text.Json;
using TaxiBookingService.API.Ride;
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
    [Route("api/driver")]
    [ApiController]

    public class DriverController : ControllerBase
    {
        private readonly IDriverLogic<Driver> _DriverLogic;
        private readonly ILoggerAdapter _logger;

        public DriverController(IDriverLogic<Driver> DriverService, ILoggerAdapter logger)
        {
            _DriverLogic = DriverService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(DriverRegisterDto request)
        {
            try
            {
                var userId = await _DriverLogic.Register(request);
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
        public async Task<ActionResult> Login(DriverLoginDto request)
        {
            try
            {
                var result = await _DriverLogic.Login(request);
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

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefeshToken()
        {
            try
            {
                var result = await _DriverLogic.RefreshToken();
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
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _DriverLogic.Logout();
                _logger.LogInformation(AppConstant.LogoutSuccess);
                return Ok(AppConstant.LogoutSuccess);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $"{AppConstant.Error}: {ex.Message}");
            }
        }

        [HttpPost("addtaxi")]
        public async Task<ActionResult<int>> AddTaxi(DriverTaxiDto taxi)
        {
            try
            {
                var addedTaxiId = await _DriverLogic.AddTaxi(taxi);
                _logger.LogInformation($"{AppConstant.TaxiAdded}. taxi ID: {addedTaxiId}");
                return Ok(addedTaxiId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}:{ex.Message}", ex);
                return BadRequest();
            }
        }
      
        [HttpPatch("accept/{rideId}")]
        public async Task<IActionResult> Accept(int rideId)
        {
            try
            {
                var results = await _DriverLogic.Accept(rideId);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $"{AppConstant.Error}: {ex.Message}");
            }
        }

        [HttpPatch("decline/{rideId}")]
        public async Task<IActionResult> Decline(int rideId)
        {
            try
            {
                await _DriverLogic.Decline(rideId);
                return Ok(AppConstant.Declined);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $"{AppConstant.Error}: {ex.Message}");
            }
        }

        [HttpPatch("startride/{rideId}")]
        public async Task<IActionResult> StartRide(int rideId)
        {
            try
            {
                await _DriverLogic.StartRide(rideId);
                return Ok(AppConstant.RideStarted);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $"{AppConstant.Error}: {ex.Message}");
            }
        }

        [HttpPatch("endride/{rideId}")]
        public async Task<IActionResult> EndRide(int rideId)
        {
            try
            {
                var fare=await _DriverLogic.EndRide(rideId);
                return Ok($"{AppConstant.RideEnded} fare: {fare}" );
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $"{AppConstant.Error}: {ex.Message}");
            }
        }
        [HttpPatch("cancelride/{rideId}")]
        public async Task<IActionResult> CancelRide(int rideId,string reason)
        {
            try
            {
                await _DriverLogic.CancelRide(rideId,reason);
                return Ok($"{AppConstant.DriverCancelled}");
            }
            catch(CannotCancel ex)
            {
                _logger.LogError(AppConstant.RideNotFound, ex);
                return Conflict(AppConstant.RideNotFound);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $"{AppConstant.Error}: {ex.Message}");
            }
        }

        [HttpPost("rating/{rideId}")]
        public async Task<IActionResult> FeedBack(DriverRatingDto rating)
        {
            try
            {
                await _DriverLogic.FeedBack(rating);
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
                var result=await _DriverLogic.RideHistory();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $"{AppConstant.Error}: {ex.Message}");
            }
        }


        [HttpGet("GetRide")]
        public async Task<IActionResult> GetRide()
        {
            try
            {
                var result = await _DriverLogic.GetRide();
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
