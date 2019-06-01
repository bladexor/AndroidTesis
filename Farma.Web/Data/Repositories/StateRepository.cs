using Farma.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
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


        public IEnumerable<SelectListItem> GetComboStates()
        {
            var list = this.context.States.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).OrderBy(l => l.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a state...)",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboCities(int stateId)
        {
           // var state = this.GetStateWithCitiesAsync(stateId);

            var state =  this.context.States
                 .Include(c => c.Cities)
                 .Where(c => c.Id == stateId)
                 .FirstOrDefault();

            var list = new List<SelectListItem>();
            if (state != null)
            {
                list = state.Cities.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }).OrderBy(l => l.Text).ToList();
            }

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a city...)",
                Value = "0"
            });

            return list;
        }

        public async Task<State> GetStateAsync(City city)
        {
            return await this.context.States.Where(c => c.Cities.Any(ci => ci.Id == city.Id)).FirstOrDefaultAsync();
        }


        public async Task<City> GetCityAsync(int id)
        {
            return await this.context.Cities.FindAsync(id);
        }

    }
}
