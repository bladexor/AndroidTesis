using System.ComponentModel.DataAnnotations;

namespace Farma.Web.Data.Entities
{
    public class Pharmacy:IEntity
    {
        public int Id { get; set; }
        
        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }
        
        public string Address { get; set; }
        
        public string Latitude { get; set; }
        
        public string Longitude { get; set; }
        
        public string PhoneNumber { get; set; }
        
        public int CityId { get; set; }
        
        public City City
        {
            get; set;
        }
    }
}