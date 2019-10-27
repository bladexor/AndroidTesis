using Newtonsoft.Json;

namespace Farma.Web.Models.Helpers
{
    public class ProductViewModel
    {
        
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("medicine_name")]
            public string Name { get; set; }
        
    }
}