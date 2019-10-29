using System.Collections.Generic;

namespace Farma.Web.Models.Helpers.Locatel
{
    public class ResponseAvalavility
    {
       
            public List<availability> Availabilities { get; set; }
            public string Message { get; set; }
            public Product Product { get; set; }
        
    }
}