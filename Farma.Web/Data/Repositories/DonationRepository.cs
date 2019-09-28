using Farma.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Farma.Web.Data.Repositories
{
    public class DonationRepository:GenericRepository<Donation>,IDonationRepository
    {
        private readonly DataContext context;

        public DonationRepository(DataContext context) : base(context)
        {
            this.context = context;
        }
    }
}
