using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using TaxiBookingService.Client.DistanceMatrix;
using TaxiBookingService.Client.DistanceMatrix.Interfaces;
using TaxiBookingService.Client.Geocoding;
using TaxiBookingService.Client.Geocoding.Interfaces;
using TaxiBookingService.Common.Utilities;
using TaxiBookingService.Dal;
using TaxiBookingService.Dal.Entities;
using TaxiBookingService.Dal.Interfaces;
using TaxiBookingService.Dal.Profiles.CustomerMapping;
using TaxiBookingService.Dal.Profiles.DriverMapping;
using TaxiBookingService.Dal.Repositories;
using TaxiBookingService.Logic.User;
using TaxiBookingService.Logic.User.Interfaces;

namespace TaxiBookingService.Host
{
    public static class ServiceCollecction
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(CustomerConfig).Assembly);
            services.AddAutoMapper(typeof(DriverConfig).Assembly);
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddAutoMapper(typeof(Program));
            services.AddHttpClient("ExternalApi", client =>
            {
                client.BaseAddress = new Uri("https://api.geoapify.com/v1/geocode");
            });
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Standard Authorization header using bearer scheme(\"bearer{token}\")",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
            });

            services.AddHttpContextAccessor();
            services.AddSingleton<ILoggerAdapter, SerilogAdapter>();
            services.AddTransient<IDriverLogic, DriverLogic>();
            services.AddTransient<IUserLogic,UserLogic>();
            services.AddTransient<IDriverRepository, DriverRepository>();
            services.AddTransient<IGeoCodingHttpClient, GeoCodingHttpClient>();
            services.AddTransient<IDistanceMatrixHttpClient, DistanceMatrixHttpClient>();
            services.AddTransient<ICustomerLogic, CustomerLogic>();
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddTransient<IAdminLogic, AdminLogic>();
            services.AddTransient<IAdminRepository, AdminRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IRideLogic, RideLogic>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddDbContext<TaxiBookingServiceDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("TaxiBookingServiceDbContext")));
        }
    }
}
