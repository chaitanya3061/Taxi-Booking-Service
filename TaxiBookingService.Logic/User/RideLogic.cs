//using GoogleMaps.LocationServices;
//using TaxiBookingService.Dal.Entities;
//using TaxiBookingService.Dal.Interfaces;
//using TaxiBookingService.Logic.User.Interfaces;

//namespace TaxiBookingService.Logic.User
//{
//    public class RideLogic : IRideLogic
//    {
//        private readonly IDriverRepository<Driver> _driverRepository;

//        public RideLogic(IDriverRepository<Driver> driverRepository)
//        {
//            _driverRepository = driverRepository;
//        }

//        public async Task<List<Driver>> FindNearbyDrivers(string customerloc, double maxDistance)
//        {
//            var locationService = new GoogleLocationService();
//            var point = locationService.GetLatLongFromAddress(customerloc);
//            var Customerlatitude = point.Latitude;
//            var Customerlongitude = point.Longitude;

//            var allDrivers = await _driverRepository.GetAllDrivers();
//            var nearbyDrivers = new List<Driver>();
//            foreach (var driver in allDrivers)
//            {
//                var point2 = locationService.GetLatLongFromAddress(driver.DriverLocation);
//                var Driverlatitude = point2.Latitude;
//                var Driverlongitude = point2.Longitude;
//                double distance = CalculateDistance(Customerlatitude, Customerlongitude, Driverlatitude, Driverlongitude);
//                if (distance <= maxDistance)
//                {
//                    nearbyDrivers.Add(driver);
//                }
//            }
//            return nearbyDrivers;
//        }



//        private double CalculateDistance(double customerLat, double customerLong, double driverLat, double driverLong)
//        {
//            const double earthRadiusKm = 6371;

//            var lat1 = customerLat;
//            var lon1 = customerLong;
//            var lat2 = driverLat;
//            var lon2 = driverLong;

//            var dLat = ToRadians(lat2 - lat1);
//            var dLon = ToRadians(lon2 - lon1);

//            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
//                    Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
//                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
//            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
//            return earthRadiusKm * c;
//        }
//        private double ToRadians(double angle)
//        {
//            return Math.PI * angle / 180.0;
//        }
//    }
//}
