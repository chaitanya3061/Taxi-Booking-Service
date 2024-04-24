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

    public interface ICustomerRepository<T>
    {
        Task UpdateRefreshToken(T user, RefreshToken refreshToken);
        Task<int> Register(User request, byte[] passwordHash, byte[] passwordSalt);
        Task<T> GetByEmail(string email);
        Task<T> GetByToken(string token);
        Task<T> GetById(int id);
        Task Login(T entity);
        Task Logout(T entity);
        Task Update(Customer customer);    
    }
}
