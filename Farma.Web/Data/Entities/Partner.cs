using System;
using System.Collections.Generic;

namespace Farma.Web.Data.Entities
{
    public class Partner:IEntity
    {
        public int Id { get; set; }
        
        public String Name { get; set; }
        
        public String Website { get; set; }
        
        public String Logo { get; set; }
        
        public String UserId { get; set; }

        public User User { get; set;  }
        
        public ICollection<Pharmacy> Pharmacies { get; set; }
    }
}