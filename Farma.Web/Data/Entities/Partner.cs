using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Farma.Web.Data.Entities
{
    public class Partner:IEntity
    {
        public int Id { get; set; }
        
        public String Name { get; set; }
        
        public String Website { get; set; }
        
        public String Logo { get; set; }
        
    //    public String UserId { get; set; }

    //    public User User { get; set;  }
        
    [Display(Name = "# Pharmacies")]
    public int NumberPharmacies { get { return this.Pharmacies == null ? 0 : this.Pharmacies.Count; } }
        public ICollection<Pharmacy> Pharmacies { get; set; }
        
       // public ICollection<Product> Products { get; set; }
    }
}