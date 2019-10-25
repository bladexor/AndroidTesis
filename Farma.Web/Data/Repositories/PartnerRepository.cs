using System.Linq;
using Farma.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;

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
    }
}