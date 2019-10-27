using System.Collections.Generic;

namespace Farma.Web.Models.Helpers.Farmatodo
{
    public class Brand
    {
        public string value { get; set; }
        public string matchLevel { get; set; }
        public List<object> matchedWords { get; set; }
    }
}