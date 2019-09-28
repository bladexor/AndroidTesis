using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Farma.Common.Models
{
    public class NewDonationRequest
    {
        [Required] public string Details { get; set; }

        [Required] public int MedicineId { get; set; }

        [Required] public string UserEmail { get; set; }

        
    }
}
