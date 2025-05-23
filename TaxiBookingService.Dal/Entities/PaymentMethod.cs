﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiBookingService.Dal.Entities
{
    public class PaymentMethod
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "varchar(30)")]
        public string status { get; set; }
    }
}
