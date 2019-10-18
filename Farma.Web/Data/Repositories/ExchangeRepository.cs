using Farma.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Farma.Web.Data.Repositories
{
    public class ExchangeRepository:GenericRepository<Exchange>,IExchangeRepository
    {
        private readonly DataContext context;
        
        public ExchangeRepository(DataContext context) : base(context)
        {
            this.context = context;
        }
        
        public IEnumerable<Exchange> GetOffersByUserId(string uid)
        {
            var d = this.context.Exchanges
                .Include(m => m.Medicine)
                .Where(x => x.UserId == uid);

            return d;
            
        }
    }
}
