using System.Linq;
using System.Threading.Tasks;
using Farma.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Xml;

namespace Farma.Web.Data.Repositories
{
    public class PartnerRepository:GenericRepository<Partner>, IPartnerRepository
    {
        private readonly DataContext context;

        public PartnerRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        /*   public Partner GetPartnerByUserId(string uid)
           {
               var d = this.context.Partners
                              /// .Include(m => m.User)
                               .Where(x => x.UserId == uid);
               
                           return d.First();
           }*/
        public Partner GetPartnerByUserId(string uid)
        {
            throw new System.NotImplementedException();
        }
        
        public async Task<Partner> GetPartnerPharmaciesAsync(int id)
        {
            return await this.context.Partners
                .Include(c => c.Pharmacies)
                    .ThenInclude(b=>b.State)
                .Include(c => c.Pharmacies)
                    .ThenInclude(d=>d.City)
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}