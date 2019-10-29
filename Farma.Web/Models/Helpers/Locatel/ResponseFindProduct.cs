using System.Collections.Generic;

namespace Farma.Web.Models.Helpers.Locatel
{
    public class ResponseFindProduct
    {
        public bool EndOfSearch { get; set; }
        public int LastIndex { get; set; }
        public string Message { get; set; }
        public List<Product> Products { get; set; }
    }
}