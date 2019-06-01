
namespace Farma.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    public class CityViewModel
    {
        public int StateId { get; set; }

        public int CityId { get; set; }

        [Required]
        [Display(Name = "City")]
        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string Name { get; set; }
    }

}
