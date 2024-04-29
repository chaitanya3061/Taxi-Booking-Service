using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using TaxiBookingService.Common.Enums;

namespace TaxiBookingService.Dal.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "varchar(30)")]
        public string Name { get; set; }

        [Column(TypeName = "varchar(30)")]

        public string Email { get; set; }

        [Column(TypeName = "varchar(5)")]
        public string CountryCode { get; set; }

        [Column(TypeName = "varchar(10)")]
        public string PhoneNumber { get; set; }

        [ForeignKey("Role")]
        public int RoleId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public string? RefreshToken { get; set; }


        [Column(TypeName = "datetime2")]
        public DateTime TokenCreated { get; set; }


        [Column(TypeName = "datetime2")]
        public DateTime TokenExpires { get; set; }
        public bool IsActive { get; set; }

        public virtual Role Role { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<Driver> Drivers { get; set; }
        public virtual ICollection<Admin>Admins { get; set; }


    }
}
