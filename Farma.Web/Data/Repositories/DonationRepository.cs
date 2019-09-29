using Farma.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Farma.Web.Data.Repositories
{
    public class DonationRepository : GenericRepository<Donation>, IDonationRepository
    {
        private readonly DataContext context;

        public DonationRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        public IEnumerable<Donation> GetDonationsByUserId(string uid)
        {
            var d = this.context.Donations
                .Include(m => m.Medicine)
                .Where(x => x.UserId == uid);

            return d;
            
        }
    }
}
