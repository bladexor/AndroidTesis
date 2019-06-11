using Farma.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Farma.Web.Data.Repositories
{
    public interface IStateRepository : IGenericRepository<State>
    {
        IEnumerable<State> GetStatesWithCities();
            
        Task<State> GetStateWithCitiesAsync(int id);

        IEnumerable<SelectListItem> GetComboStates();

        IEnumerable<SelectListItem> GetComboCities(int stateId);

        Task<State> GetStateAsync(City city);
          
           Task<City> GetCityAsync(int id);
        /*
           Task AddCityAsync(CityViewModel model);

           Task<int> UpdateCityAsync(City city);

           Task<int> DeleteCityAsync(City city);

           Task<Country> GetCountryAsync(City city);
          */
    }
}
