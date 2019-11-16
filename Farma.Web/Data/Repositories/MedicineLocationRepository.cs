using Farma.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
            var d = this.context.MedicineLocations
                .Include(m => m.Medicine)
                .Include(s=>s.City)
                .Where(x => x.UserId == uid);

            return d;
        }
        
        public IEnumerable<MedicineLocation> GetLocationsByMedicineId(int medicineId)
        {
            var o = this.context.MedicineLocations
                .Include(m => m.Medicine)
                .Where(x => x.MedicineId == medicineId);

            return o;
            
        }
    }
}
