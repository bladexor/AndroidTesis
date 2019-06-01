using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Farma.Web.Data;
using Farma.Web.Data.Entities;
using Farma.Web.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Farma.Web.Models;

namespace Farma.Web.Controllers
{
 
    public class StatesController : Controller
    {
        private readonly IStateRepository stateRepository;
        private readonly ICityRepository cityRepository;

        public StatesController(IStateRepository stateRepository,
                                ICityRepository cityRepository)
        {
            this.stateRepository = stateRepository;
            this.cityRepository = cityRepository;
        }

        // GET: States
        public IActionResult Index()
        {
            return View(this.stateRepository.GetStatesWithCities());
        }

        // GET: States/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var state = await stateRepository.GetStateWithCitiesAsync(id.Value);
            if (state == null)
            {
                return NotFound();
            }

            return View(state);
        }

        // GET: States/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: States/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(State state)
        {
            if (ModelState.IsValid)
            {
                await stateRepository.CreateAsync(state);
                return RedirectToAction(nameof(Index));
            }
            return View(state);
        }

        // GET: States/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var state = await this.stateRepository.GetByIdAsync(id.Value);
            if (state == null)
            {
                return NotFound();
            }
            return View(state);
        }

        // POST: States/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, State state)
        {
            if (id != state.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await this.stateRepository.UpdateAsync(state);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await this.stateRepository.ExistAsync(state.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(state);
        }

        // GET: States/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var state = await this.stateRepository.GetByIdAsync(id.Value);
            if (state == null)
            {
                return NotFound();
            }

            // return View(state);
            await this.stateRepository.DeleteAsync(state);
            return RedirectToAction(nameof(Index));
        }


        //-------------------------------------------------------------
        // For City
        //-------------------------------------------------------------
        public async Task<IActionResult> AddCity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var state = await this.stateRepository.GetByIdAsync(id.Value);
            if (state == null)
            {
                return NotFound();
            }

            var model = new CityViewModel { StateId = state.Id };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddCity(CityViewModel model)
        {
            //model.Id = 0; //Necesario para evitar excepcion

            if (this.ModelState.IsValid)
            {
                var state=await this.stateRepository.GetStateWithCitiesAsync(model.StateId);

              //  model.StateId = model.Id; //Por Alguna Razon se mapea el StateId en City.Id
              //  model.Id = 0;             //Y City.StateId viene null

                state.Cities.Add(new City { Name = model.Name });
               await stateRepository.UpdateAsync(state);
               // await this.cityRepository.CreateAsync(model);

                //return this.RedirectToAction($"Details/{model.CountryId}");
                return this.RedirectToAction("Details", new { id = model.StateId });
            }

            return this.View(model);
        }


        //States/EditCity/5
        public async Task<IActionResult> EditCity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await this.cityRepository.GetByIdAsync(id.Value);
            if (city == null)
            {
                return NotFound();
            }

            var state = await this.stateRepository.GetStateAsync(city);
            if (state == null)
            {
                return NotFound();
            }

            return View(new CityViewModel {
                CityId = city.Id,
                Name=city.Name,
                StateId=state.Id
                                });
        }

        [HttpPost]
        public async Task<IActionResult> EditCity(CityViewModel city)
        {
            if (this.ModelState.IsValid)
            {
                await this.cityRepository.UpdateAsync(new City { Id=city.CityId, Name=city.Name});
                
                return this.RedirectToAction("Details", new { id = city.StateId });
            }

            return this.View(city);
        }

        //States/DeleteCity/5
        public async Task<IActionResult> DeleteCity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await this.cityRepository.GetByIdAsync(id.Value);

            if (city == null)
            {
                return NotFound();
            }

            var state = await stateRepository.GetStateAsync(city);
            await this.cityRepository.DeleteAsync(city);

            // return this.RedirectToAction($"Details/{countryId}");
            return this.RedirectToAction("Details", new { id = state.Id });
        }
    }
}
