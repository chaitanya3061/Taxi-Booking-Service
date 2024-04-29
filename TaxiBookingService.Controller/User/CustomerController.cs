using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Authentication;
using TaxiBookingService.API.User.Customer;
using TaxiBookingService.API.User.Driver;
using TaxiBookingService.Common.AssetManagement.Common;
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
        private readonly ICustomerLogic _CustomerLogic;
        private readonly ILoggerAdapter _logger;

        public CustomerController(ICustomerLogic CustomerService, ILoggerAdapter logger)
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

        [Authorize(Roles = "Customer")]
        [HttpPost("rides/book")]
        public async Task<IActionResult> BookRide(CustomerBookRideDto request)
        {
            try
            {
                var result = await _CustomerLogic.BookRide(request);
                _logger.LogInformation($"{AppConstant.RequestSended}:-{result}");
                return Ok($"{AppConstant.RequestSended}:-{result}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $"{AppConstant.Error}: {ex.Message}");
            }

        }

        [Authorize(Roles = "Customer")]
        [HttpGet("rides/{rideId}/matched-driver")]
        public async Task<IActionResult> GetMatchedDriverDetails(int rideId)
        {
            try
            {
                var result = await _CustomerLogic.GetMatchedDriver(rideId);
                _logger.LogInformation(AppConstant.RequestSended);
                return Ok(result);
            }
            catch (NomatchesFound ex)
            {
                return NotFound($"{ex.Message} {AppConstant.Nomatching}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $"{AppConstant.Error}: {ex.Message}");
            }

        }

        [Authorize(Roles = "Customer")]
        [HttpPatch("rides/{rideId}/cancel")]
        public async Task<IActionResult> CancelRide(int rideId, string reason)
        {
            try
            {
                await _CustomerLogic.CancelRide(rideId, reason);
                return Ok($"{AppConstant.CustomerCancelled}");
            }
            catch (CannotCancel ex)
            {
                return Conflict($"{ex.Message} {AppConstant.CannotCancel}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $"{AppConstant.Error}: {ex.Message}");
            }
        }

        [Authorize(Roles = "Customer")]
        [HttpPost("rides/{rideId}/feedback")]
        public async Task<IActionResult> FeedBack(CustomerRatingDto rating)
        {
            try
            {
                await _CustomerLogic.FeedBack(rating);
                return Ok($"{AppConstant.Feedback}");
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

        [Authorize(Roles = "Customer")]
        [HttpGet("rides/history")]
        public async Task<IActionResult> RideHistory()
        {
            try
            {
                var result = await _CustomerLogic.RideHistory();
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

        [Authorize(Roles = "Customer")]
        [HttpPatch("rides/{rideId}/dropoff-location")]
        public async Task<IActionResult> UpdateDropOffLocation(CustomerUpdateDropOffDto request)
        {
            try
            {
                var result = await _CustomerLogic.UpdateDropOffLocation(request);
                return Ok(result);
            }
            catch (CannotUpdateDropOff ex)
            {
                return Conflict($"{ex.Message} {AppConstant.CannotUpdateDropOff}");
            }
            catch (SameLocationException ex)
            {
                return Conflict($"{ex.Message} {AppConstant.MismatchLocation}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $"{AppConstant.Error}: {ex.Message}");
            }
        }

        [Authorize(Roles = "Customer")]
        [HttpPatch("wallet/top-up")]
        public async Task<IActionResult> TopUpWallet(int amount)
        {
            try
            {
                var result = await _CustomerLogic.TopUpWallet(amount);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $"{AppConstant.Error}: {ex.Message}");
            }
        }

        [Authorize(Roles = "Customer")]
        [HttpPost("trusted-contacts/add")]
        public async Task<IActionResult> AddTrustedContact(CustomerTrustedContactDto request)
        {
            try
            {
                var result = await _CustomerLogic.AddTrustedContact(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $"{AppConstant.Error}: {ex.Message}");
            }
        }

        [Authorize(Roles = "Customer")]
        [HttpPost("add-stop/add")]
        public async Task<IActionResult> AddStop(CustomerAddStopDto request)
        {
            try
            {
                var result = await _CustomerLogic.AddStop(request);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict($"{ex.Message} {AppConstant.CannotAddStopLocation}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $"{AppConstant.Error}: {ex.Message}");
            }
        }

        [Authorize(Roles = "Customer")]
        [HttpDelete("delete-stop/delete")]
        public async Task<IActionResult> DeleteStop(int rideId)
        {
            try
            {
                var result = await _CustomerLogic.DeleteStop(rideId);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict($"{ex.Message} {AppConstant.CannotDeleteStopLocation}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $"{AppConstant.Error}: {ex.Message}");
            }
        }

        [Authorize(Roles = "Customer")]
        [HttpPatch("rides/{rideId}/make-payment")]
        public async Task<IActionResult> MakePayment(int rideId)
        {
            try
            {
                var result =await _CustomerLogic.MakePayment(rideId);
                return Ok(result);
            }
            catch (NotStarted ex)
            {
                return Conflict($"{ex.Message} {AppConstant.RideNotStarted}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $"{AppConstant.Error}: {ex.Message}");
            }
        }


    }
}
