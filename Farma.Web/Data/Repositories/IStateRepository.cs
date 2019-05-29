using Farma.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Farma.Web.Data.Repositories
{
    public interface IStateRepository : IGenericRepository<State>
    {
        IQueryable GetStatesWithCities();
            
        Task<State> GetStateWithCitiesAsync(int id);

        /*  
           Task<City> GetCityAsync(int id);

           Task AddCityAsync(CityViewModel model);

           Task<int> UpdateCityAsync(City city);

           Task<int> DeleteCityAsync(City city);

           IEnumerable<SelectListItem> GetComboCountries();

           IEnumerable<SelectListItem> GetComboCities(int conuntryId);

           Task<Country> GetCountryAsync(City city);
          */
    }
}
