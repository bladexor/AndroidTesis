namespace Farma.Web.Data.Entities
{
    public class Product:IEntity
    {
        public int Id { get; set; }

        public string Description { get; set; }
        
        public string ImageUrl { get; set; }
        
        public string PartnerName { get; set; }
    }
}