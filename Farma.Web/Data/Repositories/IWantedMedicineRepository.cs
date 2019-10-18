using Farma.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Farma.Web.Data.Repositories
{
    public interface IWantedMedicineRepository:IGenericRepository<WantedMedicine>
    {
        IEnumerable<WantedMedicine> GetWantedByUserId(string uid);
    }
}
