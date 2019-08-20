using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [Display(Name = "Phone Number")]
        public override string PhoneNumber { get => base.PhoneNumber; set => base.PhoneNumber = value; }

        [Display(Name = "Email Confirmed")]
        public override bool EmailConfirmed { get => base.EmailConfirmed; set => base.EmailConfirmed = value; }

        public int CityId { get; set; }
        //[ForeignKey(nameof(CityId))]
        public City City { get; set; }

        [NotMapped]
        [Display(Name = "Is Admin?")]
        public bool IsAdmin { get; set; }

        //TODO: Genero F o M

        public ICollection<Donation> Donations { get; set; }

        public ICollection<Exchange> Exchanges { get; set; }

        public ICollection<WantedMedicine> WantedMedicines { get; set; }
    }
}
