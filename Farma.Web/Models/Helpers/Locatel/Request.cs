namespace Farma.Web.Models.Helpers.Locatel
{
    public class Request
    {
        public string ApiKey { get; set; }
        public int Count { get; set; }
        public string Search { get; set; }
        public int StartIndex { get; set; }
        public object StoreId { get; set; }
        
        public string ProductId { get; set; }
        
    }
}