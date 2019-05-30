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

namespace Farma.Web.Controllers
{
    [Authorize]
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

            var model = new City { StateId = state.Id };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddCity(City model)
        {
            //model.Id = 0; //Necesario para evitar excepcion

            if (this.ModelState.IsValid)
            {
                var state=await this.stateRepository.GetStateWithCitiesAsync(model.Id);

                model.StateId = model.Id; //Por Alguna Razon se mapea el StateId en City.Id
                model.Id = 0;             //Y City.StateId viene null

                state.Cities.Add(model);
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

            return View(city);
        }

        [HttpPost]
        public async Task<IActionResult> EditCity(City city)
        {
            if (this.ModelState.IsValid)
            {
                await this.cityRepository.UpdateAsync(city);
                if (city.StateId != 0)
                {
                 
                    return this.RedirectToAction("Details", new { id = city.StateId });
                }
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

            var stateId = city.StateId;
            await this.cityRepository.DeleteAsync(city);

            // return this.RedirectToAction($"Details/{countryId}");
            return this.RedirectToAction("Details", new { id = stateId });
        }
    }
}
