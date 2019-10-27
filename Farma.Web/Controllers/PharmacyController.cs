using Farma.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Farma.Web.Controllers
{
    using Farma.Web.Data.Entities;
    using Farma.Web.Data.Repositories;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    
        public class PharmacyController:Controller
        {
            private readonly IPartnerRepository partnerRepository;
            private readonly IPharmacyRepository pharmacyRepository;
            private readonly IStateRepository stateRepository;
            private readonly ICityRepository cityRepository;

            public PharmacyController(IPartnerRepository partnerRepository,
                IPharmacyRepository pharmacyRepository,
                IStateRepository stateRepository,
                ICityRepository cityRepository)
            {
                this.partnerRepository = partnerRepository;
                this.pharmacyRepository = pharmacyRepository;
                this.stateRepository = stateRepository;
                this.cityRepository = cityRepository;
            }

            public async Task<IActionResult> Index()
            {
                var pharmacies =  this.pharmacyRepository.GetAll()
                            .Include(c=>c.City).ToList()
                            .OrderBy(c=>c.Description);
                
                /*var users = await this.userHelper.GetAllUsersAsync();
                foreach (var med in medicines)
                {
                    var myUser = await this.userHelper.GetUserByIdAsync(user.Id);
                    if (myUser != null)
                    {
                        user.IsAdmin = await this.userHelper.IsUserInRoleAsync(myUser, "Admin");
                    }
                }
                */

                return this.View(pharmacies);
            }
            
            //DEVUELVE UNH JSON PARA EL COMBOBOX DE CIUDADES DE CIERTO PAIS
            public async Task<JsonResult> GetCitiesAsync(int stateId)
            {
                var state = await this.stateRepository.GetStateWithCitiesAsync(stateId);
                return this.Json(state.Cities.OrderBy(c => c.Name));
            }
            
            // GET: Pharmacy/Create
            [Authorize(Roles = "Admin")]
         /*   public IActionResult Create()
            {
                var model = new PharmacyViewModel
                {
                    States = this.stateRepository.GetComboStates(),
                    Cities = this.stateRepository.GetComboCities(0)
                };

                return this.View(model);
            }
            */
            public async Task<IActionResult> Create(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var partner = await this.partnerRepository.GetByIdAsync(id.Value);
                if (partner == null)
                {
                    return NotFound();
                }

                var model = new PharmacyViewModel
                {
                    States = this.stateRepository.GetComboStates(),
                    Cities = this.stateRepository.GetComboCities(0),
                    PartnerId= partner.Id
                };
                return View(model);
            }

            // POST: Pharmacy/Create
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create(PharmacyViewModel pharmacy)
            {
                if (ModelState.IsValid)
                {
                   var partner=await this.partnerRepository.GetPartnerPharmaciesAsync(pharmacy.PartnerId);
                  
                   var p=new Pharmacy
                    {
                        Description = pharmacy.Description,
                        Address = pharmacy.Address,
                        PhoneNumber = pharmacy.PhoneNumber,
                        Latitude = pharmacy.Latitude,
                        Longitude = pharmacy.Longitude,
                        CityId = pharmacy.CityId,
                        StateId = pharmacy.StateId,
                       
                    };
                   
                   partner.Pharmacies.Add(p);
                   await partnerRepository.UpdateAsync(partner);
                  //  await pharmacyRepository.CreateAsync(p);
                    
                   // return RedirectToAction(nameof(Index));
                    return this.RedirectToAction("Pharmacies","Partner",new { id = pharmacy.PartnerId });
                }
                return View(pharmacy);
            }
            
            
            // GET: Pharmacy/Delete/5
            [Authorize(Roles = "Admin")]
            public async Task<IActionResult> Delete(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var pharmacy = await this.pharmacyRepository.GetByIdAsync(id.Value);
                if (pharmacy == null)
                {
                    return NotFound();
                }

                // return View(state);
                await this.pharmacyRepository.DeleteAsync(pharmacy);
                return this.RedirectToAction("Pharmacies","Partner",new { id = pharmacy.PartnerId });
            }
            
            
            //Pharmacy/Edit/5
            public async Task<IActionResult> Edit(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var pharmacy = await this.pharmacyRepository.GetByIdAsync(id.Value);
                if (pharmacy == null)
                {
                    return NotFound();
                }
                
                return View(new PharmacyViewModel {
                    Id = pharmacy.Id,
                    Description = pharmacy.Description,
                    Address = pharmacy.Address,
                    Latitude = pharmacy.Latitude,
                    Longitude = pharmacy.Longitude,
                    PartnerId = pharmacy.PartnerId,
                   PhoneNumber = pharmacy.PhoneNumber,
                    CityId = pharmacy.CityId,
                    StateId=pharmacy.StateId,
                    
                    States = this.stateRepository.GetComboStates(),
                    Cities = this.stateRepository.GetComboCities(pharmacy.StateId)
                });
            }

            [HttpPost]
            public async Task<IActionResult> Edit(PharmacyViewModel pharmacy)
            {
                if (this.ModelState.IsValid)
                {
                    var p=new Pharmacy
                    {
                        Id = pharmacy.Id,
                        Description = pharmacy.Description,
                        Address = pharmacy.Address,
                        PhoneNumber = pharmacy.PhoneNumber,
                        Latitude = pharmacy.Latitude,
                        Longitude = pharmacy.Longitude,
                        CityId = pharmacy.CityId,
                        StateId = pharmacy.StateId,
                        PartnerId = pharmacy.PartnerId
                       
                    };
                    await this.pharmacyRepository.UpdateAsync(p);
                
                    return this.RedirectToAction("Pharmacies","Partner",new { id = pharmacy.PartnerId });
                }

                return this.View(pharmacy);
            }
        }
    

}