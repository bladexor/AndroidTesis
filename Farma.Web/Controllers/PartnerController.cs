using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Farma.Web.Data.Entities;
using Farma.Web.Data.Repositories;
using Farma.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Farma.Web.Controllers
{
    public class PartnerController:Controller
    {
        private readonly IPartnerRepository partnerRepository;

        public PartnerController(IPartnerRepository partnerRepository)
        {
            this.partnerRepository = partnerRepository;
        }
        
        public async Task<IActionResult> Index()
        {
            var partners =  this.partnerRepository.GetAll().ToList()
               // .Include(c=>c.City).ToList()
                .OrderBy(c=>c.Name);

            return this.View(partners);
        }
        
        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Partners/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PartnerViewModel partnervm)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if(partnervm.ImageFile!=null && partnervm.ImageFile.Length > 0)
                {
                    var guid = Guid.NewGuid().ToString();
                    var file = $"{guid}.jpg";

                    path = Path.Combine(
                        Directory.GetCurrentDirectory(), 
                        "wwwroot//images//partners",
                        file);

                    using(var stream=new FileStream(path, FileMode.Create))
                    {
                        await partnervm.ImageFile.CopyToAsync(stream);
                    }

                    path = $"~/images/partners/{file}";
                }

                var partner = new Partner
                {
                        Name = partnervm.Name,
                        Website = partnervm.Website,
                        Logo = path
                };
               
               
               // product.User = await this.userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                await this.partnerRepository.CreateAsync(partner);
                           
                return RedirectToAction(nameof(Index));
            }
            return View(partnervm);
        }
        
        [Authorize(Roles = "Admin")]
        // GET: Partner/Edit/5
        public async Task<IActionResult> Edit(int? id)
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

            var view = new PartnerViewModel
            {
                Id = partner.Id,
                Name=partner.Name,
                Website = partner.Website,
                Logo = partner.Logo
            };

            return View(view);
        }
        
        // POST: Partner/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, PartnerViewModel pvm)
        {
           
            if (ModelState.IsValid)
            {
                try
                {
                    var path = pvm.Logo;

                    if (pvm.ImageFile != null && pvm.ImageFile.Length > 0)
                    {
                        var guid = Guid.NewGuid().ToString();
                        var file = $"{guid}.jpg";

                        path = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "wwwroot//images//partners",
                            file);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await pvm.ImageFile.CopyToAsync(stream);
                        }

                        path = $"~/images/partners/{file}";
                    }

                    var partner = new Partner
                    {
                        Id = pvm.Id,
                        Name=pvm.Name,
                        Website = pvm.Website,
                        Logo = path
                    };

                 
                   // partner.User = await this.userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                    await this.partnerRepository.UpdateAsync(partner);
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await this.partnerRepository.ExistAsync(pvm.Id))
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
            return View(pvm);
        }
        
        // GET: Partner/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pharmacy = await this.partnerRepository.GetByIdAsync(id.Value);
            if (pharmacy == null)
            {
                return NotFound();
            }

            // return View(state);
            await this.partnerRepository.DeleteAsync(pharmacy);
            return RedirectToAction(nameof(Index));
        }
        
        
    }
}