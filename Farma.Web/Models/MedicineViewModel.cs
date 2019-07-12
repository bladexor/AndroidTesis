using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Farma.Web.Models
{
    public class MedicineViewModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("medicine_name")]
        public string Name { get; set; }
    }
}
