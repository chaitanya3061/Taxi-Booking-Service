//using Microsoft.AspNetCore.Mvc;
//using NuGet.Common;
//using System.Net.Http.Headers;
//using System.Text;
//using System.Text.Json;
//using TaxiBookingService.API.User.Customer;
//using TaxiBookingService.Dal.Entities;
//using TaxiBookingService.Mvc.Models;

//namespace TaxiBookingService.Mvc.Controllers
//{
//    public class CustomerController : Controller
//    {
//        private readonly HttpClient _httpClient;
//        private readonly IHttpContextAccessor _httpContextAccessor;

//        public CustomerController(IHttpContextAccessor httpContextAccessor)
//        {

//            _httpClient = new HttpClient
//            {
//                BaseAddress = new Uri("https://localhost:7182/api/")
//            };
//            _httpContextAccessor = httpContextAccessor;
//        }
//        [HttpGet]
//        public IActionResult Register()
//        {
//            return View();
//        }

//        [HttpPost]
//        public async Task<IActionResult> Register(CustomerRegisterServiceContracts request)
//        {
//            try
//            {
//                var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
//                var response = await _httpClient.PostAsync("customer/register", content);

//                response.EnsureSuccessStatusCode();

//                var responseContent = await response.Content.ReadAsStringAsync();
//                return RedirectToAction("Login");
//            }
//            catch (HttpRequestException ex)
//            {
//                return BadRequest($"Error: {ex.Message}");
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Internal server error: {ex.Message}");
//            }
//        }


//        [HttpGet]
//        public IActionResult Login()
//        {
//            return View();
//        }
//        [HttpPost]
//        public async Task<IActionResult> Login(CustomerLoginServiceContracts request)
//        {
//            try
//            {
//                var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
//                var response = await _httpClient.PostAsync("customer/login", content);


//                var responseContent = await response.Content.ReadAsStringAsync();
//                var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseContent);

//                _httpContextAccessor.HttpContext.Response.Cookies.Append("accessToken", tokenResponse.AccessToken);
//                _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", tokenResponse.RefreshToken);


//                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);

//                return RedirectToAction("BookRide");
//            }
//            catch (HttpRequestException ex)
//            {
//                return BadRequest($"Error: {ex.Message}");
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Internal server error: {ex.Message}");
//            }
//        }

//        [HttpGet]
//        public IActionResult BookRide()
//        {
//            return View();
//        }


//        [HttpPost]
//        public async Task<IActionResult> BookRide(CustomerBookServiceContracts request)
//        {
//            try
//            {
//                var accessToken = HttpContext.Request.Cookies["accessToken"];

//                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

//                var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
//                var response = await _httpClient.PostAsync("customer/bookride", content);

//                response.EnsureSuccessStatusCode();
//                var responseContent = await response.Content.ReadAsStringAsync();

//                var nearbyDriversResponse = await _httpClient.GetAsync($"driver/listofnearbydrivers?location={request.PickupLocation}");

//                nearbyDriversResponse.EnsureSuccessStatusCode();

//                var nearbyDriversJson = await nearbyDriversResponse.Content.ReadAsStringAsync();
//                var nearbyDrivers = JsonSerializer.Deserialize<List<DriverDetail>>(nearbyDriversJson);
//                foreach (var driver in nearbyDrivers)
//                {
//                    var bookRideDetails = new
//                    {
//                        request.PickupLocation,
//                        request.DropoffLocation,
//                        request.TaxiType
//                    };


//                    await Task.Delay(TimeSpan.FromSeconds(10));
//                }

//                    return Ok("searching for drivers");

//            }
//            catch (Exception ex)
//            {
//                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
//            }
//        }


//    }
//}
