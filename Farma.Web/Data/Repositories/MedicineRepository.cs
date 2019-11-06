using Farma.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Farma.Web.Data.Repositories
{
    public class MedicineRepository : GenericRepository<Medicine>, IMedicineRepository
    {
        private readonly DataContext context;

        public MedicineRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        public IEnumerable<Medicine> FindRangeStartsWithAsync(string name)
        {
            return this.context.Medicines
                               .Where(c => c.Name.StartsWith(name,StringComparison.CurrentCultureIgnoreCase))
                               .OrderBy(c => c.Name)
                               .Take(10).ToList();

        }

        public  Medicine GetByNameAsync(string name)
        {
            return context.Medicines
                .Where(c => c.Name.Equals(name,StringComparison.CurrentCultureIgnoreCase))
                .FirstOrDefault();
           
        }
    }
}
