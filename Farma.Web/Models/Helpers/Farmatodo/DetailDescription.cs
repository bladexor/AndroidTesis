using System.Collections.Generic;

namespace Farma.Web.Models.Helpers.Farmatodo
{
    public class DetailDescription
    {
        public string value { get; set; }
        public string matchLevel { get; set; }
        public bool fullyHighlighted { get; set; }
        public List<string> matchedWords { get; set; }
    }
}