//using Microsoft.AspNetCore.Mvc;
//using System.Net.Http.Headers;
//using System.Text.Json;
//using System.Text;
//using TaxiBookingService.API.User.Driver;
//using NuGet.Common;

//namespace TaxiBookingService.Mvc.Controllers
//{
//    public class DriverController : Controller
//    {
//        private readonly HttpClient _httpClient;
//        private readonly IHttpContextAccessor _httpContextAccessor;

//        public DriverController(IHttpContextAccessor httpContextAccessor)
//        {

//            _httpClient = new HttpClient
//            {
//                BaseAddress = new Uri("https://localhost:7182/api/driver/")
//            };
//            _httpContextAccessor = httpContextAccessor;
//        }
//        [HttpGet]
//        public IActionResult Register()
//        {
//            return View();
//        }

//        [HttpPost]
//        public async Task<IActionResult> Register(DriverRegisterServiceContracts request)
//        {
//            try
//            {
//                var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
//                var response = await _httpClient.PostAsync("register", content);

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
//        public async Task<IActionResult> Login(DriverLoginServiceContracts request)
//        {
//            try
//            {

//                var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
//                var response = await _httpClient.PostAsync("login", content);

//                response.EnsureSuccessStatusCode();

//                var responseContent = await response.Content.ReadAsStringAsync();
//                _httpContextAccessor.HttpContext.Response.Cookies.Append("accessToken", responseContent);

//                return Ok(responseContent);
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
//        public IActionResult GetRide()
//        {
//            return View();
//        }


      
//    }
//}
