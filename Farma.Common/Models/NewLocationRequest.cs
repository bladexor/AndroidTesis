using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Farma.Common.Models
{
    public class NewLocationRequest
    {
        [Required] public int MedicineId { get; set; }

        [Required] public string Details { get; set; }

        [Required] public string PlaceName { get; set; }

        [Required] public int CityId { get; set; }

        [Required] public string PlaceAddress { get; set; }

        [Required] public string PlacePhone { get; set; }

        [Required] public string UserEmail { get; set; }
    }
}
