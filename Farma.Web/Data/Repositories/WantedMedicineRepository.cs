using Farma.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Farma.Web.Data.Repositories
{
    public class WantedMedicineRepository:GenericRepository<WantedMedicine>,IWantedMedicineRepository
    {
        public WantedMedicineRepository(DataContext context) : base(context)
        {

        }
    }
}
