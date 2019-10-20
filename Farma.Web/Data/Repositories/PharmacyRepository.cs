using Farma.Web.Data.Entities;

namespace Farma.Web.Data.Repositories
{
    public class PharmacyRepository:GenericRepository<Pharmacy>,IPharmacyRepository
    {
        private readonly DataContext context;
        
        public PharmacyRepository(DataContext context) : base(context)
        {
            this.context = context;
        }
    }
}