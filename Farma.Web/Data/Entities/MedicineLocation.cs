using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Farma.Web.Data.Entities
{
    public class MedicineLocation : IEntity
    {
        public int Id { get; set; }

        public string MedicineDetails { get; set; }

        public string PlaceName { get; set; }

        public string PlacePhone { get; set; }

        public string PlaceAddress { get; set; }

        public string Date { get; set; }

        public int CityId { get; set; }
        public City City
        {
            get; set;
        }

        public int MedicineId { get; set; }
        public Medicine Medicine { get; set; }

        public string UserId { get; set; }



    }
}
