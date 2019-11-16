using Farma.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Farma.Web.Data.Repositories
{
    public interface IMedicineLocationRepository:IGenericRepository<MedicineLocation>
    {
        IEnumerable<MedicineLocation> GetLocationsByUserId(string uid);
        
        IEnumerable<MedicineLocation> GetLocationsByMedicineId(int medicineId);
    }
}
