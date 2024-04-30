using Azure.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.API.User.Customer;
using TaxiBookingService.Dal.Entities;
using TaxiBookingService.Dal.Repositories;

namespace TaxiBookingService.Dal.Interfaces
{

    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<Customer> GetByToken(string token);
        Task<Customer> GetById(int id);
        Task<Customer> GetByEmail(string email);
    }
}
