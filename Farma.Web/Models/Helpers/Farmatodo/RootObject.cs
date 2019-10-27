using System.Collections.Generic;

namespace Farma.Web.Models.Helpers.Farmatodo
{
    public class RootObject
    {
        public List<Hit> hits { get; set; }
        public int nbHits { get; set; }
        public int page { get; set; }
        public int nbPages { get; set; }
        public int hitsPerPage { get; set; }
        public bool exhaustiveNbHits { get; set; }
        public string query { get; set; }
        public string @params { get; set; }
        public int processingTimeMS { get; set; }
    }
}