using Farma.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Farma.Web.Data.Repositories
{
    public class StateRepository : GenericRepository<State>, IStateRepository
    {
        private readonly DataContext context;

        public StateRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<State> GetByNameAsync(string name)
        {
            return await this.context.States
                            .Include(c => c.Cities)
                            .Where(c => c.Name == name)
                            .FirstOrDefaultAsync();
        }

        public IQueryable GetStatesWithCities()
        {
            return this.context.States
                .Include(c => c.Cities)
                .OrderBy(c => c.Name);
        }

        public async Task<State> GetStateWithCitiesAsync(int id)
        {
            return await this.context.States
                .Include(c => c.Cities)
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();
        }

    }
}
