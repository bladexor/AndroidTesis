using Farma.Web.Data.Entities;
using Farma.Web.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
