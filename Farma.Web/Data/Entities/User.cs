﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Farma.Web.Data.Entities
{
    public class User:IdentityUser
    {
        [Display(Name = "First Name")]
        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string LastName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get { return $"{this.FirstName} {this.LastName}"; } }

        [MaxLength(100, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string Address { get; set; }
    }
}
