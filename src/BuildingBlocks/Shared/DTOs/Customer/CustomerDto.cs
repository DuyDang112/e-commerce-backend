﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Customer
{
    public class CustomerDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [Column(TypeName = "varchar(150)")]
        public string LastName { get; set; }
        [Required]
        [Column(TypeName = "varchar(150)")]
        public string FirrtName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
