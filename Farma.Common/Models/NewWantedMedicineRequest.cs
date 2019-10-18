
using System.ComponentModel.DataAnnotations;

namespace Farma.Common.Models
{
    public class NewWantedMedicineRequest
    {
       
        [Required] public int MedicineId { get; set; }

        [Required] public string UserEmail { get; set; }
    }
}