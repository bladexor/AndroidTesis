using Farma.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Farma.Web.Data.Repositories
{
    public class MedicineLocationRepository : GenericRepository<MedicineLocation>, IMedicineLocationRepository
    {

        private readonly DataContext context;

        public MedicineLocationRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        public IEnumerable<MedicineLocation> GetLocationsByUserId(string uid)
        {
            throw new NotImplementedException();
        }
    }
}
