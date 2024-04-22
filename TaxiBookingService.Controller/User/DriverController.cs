using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Security.Authentication;
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
        private readonly IDriverLogic<Driver> _AccountLogic;
        private readonly ILoggerAdapter _logger;

        public DriverController(IDriverLogic<Driver> AccountService, ILoggerAdapter logger)
        {
            _AccountLogic = AccountService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(DriverRegisterServiceContracts request)
        {
            try
            {
                var userId = await _AccountLogic.Register(request);
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
        public async Task<ActionResult> Login(DriverLoginServiceContracts request)
        {
            try
            {
                var result = await _AccountLogic.Login(request);
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
                var result = await _AccountLogic.RefreshToken();
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
                await _AccountLogic.Logout();
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
        public async Task<ActionResult<int>> AddTaxi(DriverTaxiServiceContracts taxi)
        {
            try
            {
                var addedTaxiId = await _AccountLogic.AddTaxi(taxi);
                _logger.LogInformation($"{AppConstant.AssetAddedSuccess}. taxi ID: {addedTaxiId}");
                return Ok(addedTaxiId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}:{ex.Message}", ex);
                return BadRequest();
            }
        }

        [HttpPut("updatetaxi/{taxiId}")]
        public async Task<IActionResult> UpdateTaxi(int taxiId, DriverTaxiServiceContracts taxi)
        {
            try
            {
                await _AccountLogic.UpdateTaxi(taxiId, taxi);
                _logger.LogInformation($"{AppConstant.Update}. taxi ID: {taxiId}");
                return Ok($"{AppConstant.Update}. taxi ID: {taxiId}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}:{ex.Message}", ex);
                return BadRequest();
            }
        }

    

      
        [HttpPatch("accept/{rideId}")]
        public async Task<IActionResult> Accept(int driverId,int rideId)
        {
            try
            {
                var results = await _AccountLogic.Accept(driverId,rideId);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $"{AppConstant.Error}: {ex.Message}");
            }
        }

        [HttpPatch("decline/{rideId}")]
        public async Task<IActionResult> Decline(int driverId, int rideId)
        {
            try
            {
                await _AccountLogic.Decline(driverId, rideId);
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
                await _AccountLogic.StartRide(rideId);
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
                var fare=await _AccountLogic.EndRide(rideId);
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
                await _AccountLogic.CancelRide(rideId,reason);
                return Ok($"{AppConstant.DriverCancelled}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $"{AppConstant.Error}: {ex.Message}");
            }
        }

    }
}
