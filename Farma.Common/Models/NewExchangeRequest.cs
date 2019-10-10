using System.ComponentModel.DataAnnotations;

namespace Farma.Common.Models
{
    public class NewExchangeRequest
    {
        [Required] public string Details { get; set; }

        [Required] public int MedicineId { get; set; }

        [Required] public string UserEmail { get; set; }
    }
}