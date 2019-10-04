using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Farma.Web.Data.Repositories;
using Farma.Web.Helpers;
using Farma.Web.Data.Entities;

namespace Farma.Web.Controllers.API
{ 
    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    
    public class MedicineLocationController:Controller
    {
        private readonly IMedicineRepository medicineRepository;
        private readonly IUserHelper userHelper;
        private readonly IMedicineLocationRepository medicineLocationRepository;

        public MedicineLocationController(IMedicineLocationRepository medicineLocationRepository,
            IMedicineRepository medicineRepository,
            IUserHelper userHelper)
        {
            this.medicineLocationRepository = medicineLocationRepository;
            this.userHelper = userHelper;
            this.medicineRepository = medicineRepository;
        }



        [HttpGet("{userEmail}")]
        public async Task<IActionResult> GetLocations(string userEmail)

        {
            var user = await this.userHelper.GetUserByEmailAsync(userEmail);
            if (user == null)
            {
                return this.BadRequest("Invalid user");
            }

            var locations = medicineLocationRepository.GetLocationsByUserId(user.Id);

            return Ok(locations);
        }

        [HttpPost]
        public async Task<IActionResult> PostLocation([FromBody] Common.Models.NewLocationRequest location)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            var user = await this.userHelper.GetUserByEmailAsync(location.UserEmail);
            if (user == null)
            {
                return this.BadRequest("Invalid user");
            }

            var medicine = await this.medicineRepository.GetByIdAsync(location.MedicineId);
            if (medicine == null)
            {
                return this.BadRequest("Invalid medicine");
            }

            var entityLocation = new MedicineLocation
            {
                MedicineId = location.MedicineId,
                MedicineDetails = location.Details,
                //Medicine = (Medicine)medicine,
                CityId = location.CityId,
                Date = "",
                PlaceAddress=location.PlaceAddress,
                PlaceName=location.PlaceName,
                PlacePhone=location.PlacePhone,
                UserId = user.Id

            };

            // user.Donations.Add(entityDonation);
            // userHelper.UpdateUserAsync(user);
            // user.Donations.Last();
            var newLocation = await this.medicineLocationRepository.CreateAsync(entityLocation);
            return Ok(newLocation);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            var location = await this.medicineLocationRepository.GetByIdAsync(id);
            if (location == null)
            {
                return this.NotFound();
            }

            await this.medicineLocationRepository.DeleteAsync(location);
            return Ok(location);
        }
    }
}
