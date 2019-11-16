using System.Collections.Generic;
using System.Threading.Tasks;
using Farma.Common.Models;
using Farma.Web.Data.Entities;
using Farma.Web.Data.Repositories;
using Farma.Web.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Farma.Web.Controllers.API
{
[Route("api/[Controller]")]
    public class SearchController:Controller
    {
        private readonly IDonationRepository donationRepository;
        private readonly IExchangeRepository exchangeRepository;
        private readonly IMedicineLocationRepository medicineLocationRepository;
        private readonly IUserHelper userHelper;
        private readonly IStateRepository stateRepository;
        private readonly ICityRepository cityRepository;
        private readonly IMedicineRepository medicineRepository;

        public SearchController(IDonationRepository donationRepository,
            IExchangeRepository exchangeRepository,
            IMedicineLocationRepository medicineLocationRepository,
            IUserHelper userHelper,
            IStateRepository stateRepository,
            ICityRepository cityRepository,
            IMedicineRepository medicineRepository)
        {
            this.donationRepository = donationRepository;
            this.exchangeRepository = exchangeRepository;
            this.medicineLocationRepository = medicineLocationRepository;
            this.userHelper = userHelper;
            this.stateRepository = stateRepository;
            this.cityRepository = cityRepository;
            this.medicineRepository = medicineRepository;
        }
        
        
        [HttpGet("{f}")]
        public async  Task<IActionResult> FindMedicine(string f)
        {

            var medicine=medicineRepository.GetByNameAsync(f);

            List<SearchResult> search_results=new List<SearchResult>();
            
            //Buscando en Donaciones, Intercambios y Ubicaciones
            if (medicine != null)
            {
                var donations = donationRepository.GetDonationsByMedicineId(medicine.Id);
                var exchanges = exchangeRepository.GetOffersByMedicineId(medicine.Id);
                var locations = medicineLocationRepository.GetLocationsByMedicineId(medicine.Id);
               
                foreach (var d in donations)
                {
                    var user=userHelper.GetUserByIdAsync(d.UserId).Result;
                    var userCity = cityRepository.GetByIdAsync(user.CityId).Result;
                    var userState = stateRepository.GetByIdAsync(userCity.StateId).Result;
                    
                    search_results.Add(new SearchResult
                    {
                        type = "D",
                        id=d.Id,
                        medicine_name = d.Medicine.Name,
                        details = d.Details,
                        user_creator = d.UserId,
                        name_surname = user.FullName,
                        city=userCity.Name,
                        state = userState.Name,
                        phone=user.PhoneNumber,
                        urlimage = "/images/medicine_icon.jpg"
                    });
                }
                
                
                foreach (var e in exchanges)
                {
                    var user=userHelper.GetUserByIdAsync(e.UserId).Result;
                    var userCity = cityRepository.GetByIdAsync(user.CityId).Result;
                    var userState = stateRepository.GetByIdAsync(userCity.StateId).Result;
                    
                    search_results.Add(new SearchResult
                    {
                        type = "E",
                        id=e.Id,
                        medicine_name = e.Medicine.Name,
                        details = e.Details,
                        user_creator = e.UserId,
                        name_surname = user.FullName,
                        city=userCity.Name,
                        state = userState.Name,
                        phone=user.PhoneNumber,
                        urlimage = "/images/medicine_icon.jpg"
                    });
                }
                
                foreach (var l in locations)
                {
                    var user=userHelper.GetUserByIdAsync(l.UserId).Result;
                    var userCity = cityRepository.GetByIdAsync(user.CityId).Result;
                    var userState = stateRepository.GetByIdAsync(userCity.StateId).Result;
                    
                    search_results.Add(new SearchResult
                    {
                        type = "L",
                        id=l.Id,
                        medicine_name = l.Medicine.Name,
                        details = l.MedicineDetails,
                        user_creator = l. UserId,
                        name_surname = user.FullName,
                        city=userCity.Name,
                        state = userState.Name,
                        phone=l.PlacePhone,
                        urlimage = "/images/medicine_icon.jpg"
                    });
                }
            }
            
            //if(search_results.Count==0)
            //{
                /*var fHelper=new FarmatodoHelper();

                var resultado = await fHelper.BuscarProducto(f);
                if (resultado!=null)
                {
                    foreach (var hit in resultado.hits)
                    {
                        search_results.Add(new SearchResult
                        {
                            type = "P",
                            id = hit.id,
                            medicine_name = hit.description,
                            details = hit.detailDescription,
                            //user_creator = e.UserId,
                            name_surname = "Farmatodo",
                            urlimage = hit.mediaImageUrl
                            // city=userCity.Name,
                            // state = userState.Name,
                            // phone=user.PhoneNumber,
                        });
                    }
                }*/
                //}
        

            return Ok(search_results);
        }
        
    }
}