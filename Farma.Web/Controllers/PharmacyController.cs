using Farma.Web.Models;
using Microsoft.AspNetCore.Authorization;

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
            private readonly IPharmacyRepository pharmacyRepository;
            private readonly IStateRepository stateRepository;
            private readonly ICityRepository cityRepository;

            public PharmacyController(IPharmacyRepository pharmacyRepository,
                IStateRepository stateRepository,
                ICityRepository cityRepository)
            {
                this.pharmacyRepository = pharmacyRepository;
                this.stateRepository = stateRepository;
                this.cityRepository = cityRepository;
            }

            public async Task<IActionResult> Index()
            {
                var pharmacies =  this.pharmacyRepository.GetAll().ToList().OrderBy(c=>c.Description);
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
            public IActionResult Create()
            {
                var model = new PharmacyViewModel
                {
                    States = this.stateRepository.GetComboStates(),
                    Cities = this.stateRepository.GetComboCities(0)
                };

                return this.View(model);
            }

            // POST: Pharmacy/Create
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create(Pharmacy pharmacy)
            {
                if (ModelState.IsValid)
                {
                    await pharmacyRepository.CreateAsync(pharmacy);
                    return RedirectToAction(nameof(Index));
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
                return RedirectToAction(nameof(Index));
            }
        }
    

}