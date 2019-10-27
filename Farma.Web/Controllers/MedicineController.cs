using Farma.Web.Data.Entities;
using Farma.Web.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Farma.Web.Controllers
{
    public class MedicineController:Controller
    {
        private readonly IMedicineRepository medicineRepository;

        public MedicineController(IMedicineRepository medicineRepository)
        {
            this.medicineRepository = medicineRepository;
        }

        public async Task<IActionResult> Index()
        {
            var medicines =  this.medicineRepository.GetAll().ToList().OrderBy(c=>c.Name);
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

            return this.View(medicines);
        }
        
        // GET: Medicines/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Medicines/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Medicine medicine)
        {
            if (ModelState.IsValid)
            {
                await medicineRepository.CreateAsync(medicine);
                return RedirectToAction(nameof(Index));
            }
            return View(medicine);
        }

        // GET: Medicines/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicine = await this.medicineRepository.GetByIdAsync(id.Value);
            if (medicine == null)
            {
                return NotFound();
            }
            return View(medicine);
        }

        // POST: Medicines/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Medicine medicine)
        {
            if (id != medicine.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await this.medicineRepository.UpdateAsync(medicine);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await this.medicineRepository.ExistAsync(medicine.Id))
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
            return View(medicine);
        }
        
        // GET: Medicine/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicine = await this.medicineRepository.GetByIdAsync(id.Value);
            if (medicine == null)
            {
                return NotFound();
            }

            // return View(state);
            await this.medicineRepository.DeleteAsync(medicine);
            return RedirectToAction(nameof(Index));
        }
    }
}
