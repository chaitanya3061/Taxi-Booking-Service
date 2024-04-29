using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaxiBookingService.API.User.Driver;
using TaxiBookingService.Common.AssetManagement.Common;
using TaxiBookingService.Common.Utilities;
using TaxiBookingService.Dal.Entities;
using TaxiBookingService.Logic.User.Interfaces;
using static TaxiBookingService.Common.CustomException;

namespace TaxiBookingService.Controller.User
{
    [Route("api/driver")]
    [ApiController]

    public class DriverController : ControllerBase
    {
        private readonly IDriverLogic _DriverLogic;
        private readonly ILoggerAdapter _logger;

        public DriverController(IDriverLogic DriverService, ILoggerAdapter logger)
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
                _logger.LogInformation($"{AppConstant.RegistrationSuccess} id: {userId}") ;
                return Ok($"{AppConstant.RegistrationSuccess} id: {userId}");
            }
            catch (EmailAlreadyExists ex)
            {
                return Conflict($"{ex.Message} {AppConstant.EmailAlreadyExists}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}:{ex.Message}", ex);
                return BadRequest();
            }
        }

        [Authorize(Roles = "Driver")]
        [HttpPost("addtaxi")]
        public async Task<ActionResult<int>> AddTaxi(DriverTaxiDto taxi)
        {
            try
            {
                var addedTaxiId = await _DriverLogic.AddTaxi(taxi);
                _logger.LogInformation($"{AppConstant.TaxiAdded}. taxi ID: {addedTaxiId}");
                return Ok($"{AppConstant.TaxiAdded}. taxi ID: {addedTaxiId}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}:{ex.Message}", ex);
                return BadRequest();
            }
        }

        [Authorize(Roles = "Driver")]
        [HttpPatch("rides/accept/{rideId}")]
        public async Task<IActionResult> Accept(int rideId)
        {
            try
            {
                var result = await _DriverLogic.Accept(rideId);
                _logger.LogInformation($"{result} rideid: {rideId}");
                return Ok($"{result} rideid: {rideId}");
            }
            catch(InvalidOperationException ex)
            {
                return Conflict($"{ex.Message} {AppConstant.DriverNotAssignedToRide}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $"{AppConstant.Error}: {ex.Message}");
            }
        }

        [Authorize(Roles = "Driver")]
        [HttpPatch("rides/decline/{rideId}")]
        public async Task<IActionResult> Decline(int rideId)
        {
            try
            {
                var result=await _DriverLogic.Decline(rideId);
                _logger.LogInformation($"{result} rideid: {rideId}");
                return Ok($"{result} rideid: {rideId}");
            }
            catch (RideAlreadyAccepted ex)
            {
                return Conflict($"{ex.Message} {AppConstant.RideAlreadyAccepted}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $"{AppConstant.Error}: {ex.Message}");
            }
        }

        [Authorize(Roles = "Driver")]
        [HttpPatch("rides/start/{rideId}")]
        public async Task<IActionResult> StartRide(int rideId,int verificationPin)
        {
            try
            {
                var result=await _DriverLogic.StartRide(rideId, verificationPin);
                _logger.LogInformation($"{result} rideid: {rideId}");
                return Ok($"{result} rideid: {rideId}");
            }
            catch (InvalidOperationException ex)
            {
                return Conflict($"{ex.Message} {AppConstant.DriverNotAssignedToRide}");
            }
            catch (InvalidverificationPin ex)
            {
                return BadRequest($"{ex.Message} {AppConstant.InvalidverificationPin}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $"{AppConstant.Error}: {ex.Message}");
            }
        }

        [Authorize(Roles = "Driver")]
        [HttpPatch("rides/end/{rideId}")]
        public async Task<IActionResult> EndRide(int rideId)
        {
            try
            {
                var result=await _DriverLogic.EndRide(rideId);
                _logger.LogInformation(result);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(AppConstant.DriverNotAssignedToRide);
            }
            catch(PaymentNotCompleted ex)
            {
                return StatusCode(AppConstant.PaymentRequired, AppConstant.PaymentNotCompleted);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $"{AppConstant.Error}: {ex.Message}");
            }
        }

        [Authorize(Roles = "Driver")]
        [HttpPatch("rides/{rideId}/cancel")]
        public async Task<IActionResult> CancelRide(int rideId,string reason)
        {
            try
            {
                var result=await _DriverLogic.CancelRide(rideId,reason);
                _logger.LogInformation($"{result} rideid: {rideId}");
                return Ok($"{result} rideid: {rideId}");
            }
            catch(CannotCancel ex)
            {
                return NotFound($"{ex.Message} {AppConstant.CannotCancel}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $"{AppConstant.Error}: {ex.Message}");
            }
        }


        [Authorize(Roles = "Driver")]
        [HttpPost("rides/{rideId}/rating")]
        public async Task<IActionResult> FeedBack(DriverRatingDto request)
        {
            try
            {
                var result=await _DriverLogic.FeedBack(request);
                _logger.LogInformation($"{result} rideid: {request.RideId}");
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound($"{ex.Message} {AppConstant.RideNotFound}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $"{AppConstant.Error}: {ex.Message}");
            }
        }

        [Authorize(Roles = "Driver")]
        [HttpGet("rides/history")]
        public async Task<IActionResult> RideHistory()
        {
            try
            {
                var result=await _DriverLogic.RideHistory();
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound($"{ex.Message} {AppConstant.NoridesFound}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $"{AppConstant.Error}: {ex.Message}");
            }
        }

        [Authorize(Roles = "Driver")]
        [HttpGet("rides/active")]
        public async Task<IActionResult> GetActiveRide()
        {
            try
            {
                var result = await _DriverLogic.GetActiveRide();
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound($"{ex.Message} {AppConstant.NoridesFound}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $"{AppConstant.Error}: {ex.Message}");
            }
        }

        [Authorize(Roles = "Driver")]
        [HttpPatch("confirmpayment/{rideId}")]
        public async Task<IActionResult> ConfirmRidePayment(int rideId)
        {
            try
            {
                var result =await _DriverLogic.ConfirmRidePayment(rideId);
                _logger.LogInformation($"{result} rideid: {rideId}");
                return Ok($"{result} rideid: {rideId}");
            }
            catch (NotStarted ex)
            {
                return Conflict($"{ex.Message} {AppConstant.DriverNotYetStarted}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $"{AppConstant.Error}: {ex.Message}");
            }
        }
    }
}
