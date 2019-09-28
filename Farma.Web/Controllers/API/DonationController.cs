﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Farma.Web.Data.Entities;
using Farma.Web.Data.Repositories;
using Farma.Web.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Farma.Web.Controllers.API
{
    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DonationController : Controller
    {
        private readonly IDonationRepository donationRepository;
        private readonly IUserHelper userHelper;
        private readonly IMedicineRepository medicineRepository;

        public DonationController(IDonationRepository donationRepository, IMedicineRepository medicineRepository,
            IUserHelper userHelper)
        {
            this.donationRepository = donationRepository;
            this.userHelper = userHelper;
            this.medicineRepository = medicineRepository;
        }

        [HttpGet("{userEmail}")]
        public  async Task<IActionResult> GetDonations(string userEmail)

        {
            var user = await this.userHelper.GetUserByEmailAsync(userEmail);
            if (user == null)
            {
                return this.BadRequest("Invalid user");
            }

            var donations=donationRepository.GetAll()
                .Where(u => u.UserId == user.Id);
           // var userd = await this.userHelper.GetUserwithDonationsAsync(user);
           // var donations = userd.Donations;
            return Ok(donations);
        }

        [HttpPost]
        public async Task<IActionResult> PostDonation([FromBody] Common.Models.NewDonationRequest donation)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            var user = await this.userHelper.GetUserByEmailAsync(donation.UserEmail);
            if (user == null)
            {
                return this.BadRequest("Invalid user");
            }
                        
            var medicine = await this.medicineRepository.GetByIdAsync(donation.MedicineId);
            if (medicine == null)
            {
                return this.BadRequest("Invalid medicine");
            }

            var entityDonation = new Donation
            {
                Details = donation.Details,
                //Medicine = (Medicine)medicine,
                MedicineId=medicine.Id,
                Date = "",
                Status=0,
                UserId=user.Id
                
            };

           // user.Donations.Add(entityDonation);
           // userHelper.UpdateUserAsync(user);
           // user.Donations.Last();
            var newDonation = await this.donationRepository.CreateAsync(entityDonation);
            return Ok(newDonation);
        }
    }
}
