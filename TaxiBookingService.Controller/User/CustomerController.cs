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
            catch (EmailAlreadyExistsExecption ex)
            {
                return Conflict($"{ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", ex);
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
                _logger.LogInformation($"{AppConstant.RequestSent}:-{result}");
                return Ok($"{AppConstant.RequestSent}:-{result}");
            }
            catch(CustomerAlreadyInSearchRideException ex)
            {
                return Conflict($"{ex.Message} ");
            }
            catch(InsufficientFundsException ex)
            {
                return BadRequest($"{ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $" {ex.Message}");
            }
        }

        [Authorize(Roles = "Customer")]
        [HttpPost("rides/schedule-ride")]
        public async Task<IActionResult> ScheduleRide(CustomerBookRideDto request)
        {
            try
            {
                var result = await _CustomerLogic.ScheduleRide(request);
                _logger.LogInformation($"{AppConstant.RequestSent}:-{result}");
                return Ok($"{AppConstant.RequestSent}:-{result}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $" {ex.Message}");
            }
        }

        [Authorize(Roles = "Customer")]
        [HttpGet("rides/GetAllActiveSchedulerides")]
        public async Task<IActionResult> GetAllActiveSchedulerides()
        {
            try
            {
                var result = await _CustomerLogic.GetAllActiveSchedulerides();
                _logger.LogInformation($"{result}");
                return Ok($"{result}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $" {ex.Message}");
            }
        }

        [Authorize(Roles = "Customer")]
        [HttpGet("rides/{rideId}/driver-allocated")]
        public async Task<IActionResult> GetAllocatedDriverDetails(int rideId)
        {
            try
            {
                var result = await _CustomerLogic.GetAllocatedDriver(rideId);
                _logger.LogInformation(AppConstant.RetrievedDriverSuccess);
                return Ok(result);
            }
            catch(NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $" {ex.Message}");
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
            catch (CannotCancelException ex)
            {
                return Conflict($"{ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $" {ex.Message}");
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
            catch(RideNotCompletedException ex)
            {
                return Conflict($"{ex.Message} ");
            }
            catch (NotFoundException ex)
            {
                return NotFound($"{ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $" {ex.Message}");
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
                return NotFound($"{ex.Message} ");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $" {ex.Message}");
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
            catch(NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (CannotUpdateDropOffExecption ex)
            {
                return Conflict($"{ex.Message} ");
            }
            catch (SameLocationException ex)
            {
                return Conflict($"{ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $" {ex.Message}");
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
            catch(InvalidTopUpAmountException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $" {ex.Message}");
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
                return StatusCode((int)AppConstant.ServerError, $" {ex.Message}");
            }
        }

        [Authorize(Roles = "Customer")]
        [HttpPost("add-stop/add/{rideId}")]
        public async Task<IActionResult> AddStop(CustomerAddStopDto request)
        {
            try
            {
                var result = await _CustomerLogic.AddStop(request);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict($"{ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $" {ex.Message}");
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
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict($"{ex.Message} ");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $" {ex.Message}");
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
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (NotStartedException ex)
            {
                return Conflict($"{ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.Error}{ex.Message}", ex);
                return StatusCode((int)AppConstant.ServerError, $" {ex.Message}");
            }
        }


    }
}
