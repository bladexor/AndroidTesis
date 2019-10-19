using System;
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
    
    public class ExchangeController:Controller
    {
        private readonly IExchangeRepository exchangeRepository;
        private readonly IMedicineRepository medicineRepository;
        private readonly IUserHelper userHelper;
        private readonly IWantedMedicineRepository wantedMedicineRepository;


        public ExchangeController(IExchangeRepository exchangeRepository,
                                  IWantedMedicineRepository wantedMedicineRepository,
                                  IMedicineRepository medicineRepository,
                                  IUserHelper userHelper)
        {
            this.exchangeRepository = exchangeRepository;
            this.medicineRepository = medicineRepository;
            this.wantedMedicineRepository = wantedMedicineRepository;
            this.userHelper = userHelper;
        }
        
        [HttpGet("Offers/{userEmail}")]
        public  async Task<IActionResult> GetOffers(string userEmail)

        {
            var user = await this.userHelper.GetUserByEmailAsync(userEmail);
            if (user == null)
            {
                return this.BadRequest("Invalid user");
            }

            var exchangeOffers = exchangeRepository.GetOffersByUserId(user.Id);
          
            return Ok(exchangeOffers);
        }
        
        [HttpGet("WantedMedicines/{userEmail}")]
        public  async Task<IActionResult> GetWanted(string userEmail)

        {
            var user = await this.userHelper.GetUserByEmailAsync(userEmail);
            if (user == null)
            {
                return this.BadRequest("Invalid user");
            }

            var exchangeWanted = wantedMedicineRepository.GetWantedByUserId(user.Id);
          
            return Ok(exchangeWanted);
        }
        
        [HttpPost]
        [Route("PostOffer")]
        public async Task<IActionResult> PostExchange([FromBody] Common.Models.NewExchangeRequest exchange)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            var user = await this.userHelper.GetUserByEmailAsync(exchange.UserEmail);
            if (user == null)
            {
                return this.BadRequest("Invalid user");
            }
                        
            var medicine = await this.medicineRepository.GetByIdAsync(exchange.MedicineId);
            if (medicine == null)
            {
                return this.BadRequest("Invalid medicine");
            }

            var entityExchange = new Exchange
            {
                Details = exchange.Details,
                MedicineId=medicine.Id,
                Date = "",
                //Status=0,
                UserId=user.Id
                
            };

           
            var newOffer = await this.exchangeRepository.CreateAsync(entityExchange);
            return Ok(newOffer);
        }
        
        [HttpPost]
        [Route("PostWanted")]
        public async Task<IActionResult> PostWanted([FromBody] Common.Models.NewWantedMedicineRequest wantedMedicine)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            var user = await this.userHelper.GetUserByEmailAsync(wantedMedicine.UserEmail);
            if (user == null)
            {
                return this.BadRequest("Invalid user");
            }
                        
            var medicine = await this.medicineRepository.GetByIdAsync(wantedMedicine.MedicineId);
            if (medicine == null)
            {
                return this.BadRequest("Invalid medicine");
            }

            var entityWantedMedicine = new WantedMedicine
            {
               
                MedicineId=medicine.Id,
                UserId=user.Id
                
            };

            // user.Donations.Add(entityDonation);
            // userHelper.UpdateUserAsync(user);
            // user.Donations.Last();
            var newWantedMedicine = await this.wantedMedicineRepository.CreateAsync(entityWantedMedicine);
            return Ok(newWantedMedicine);
        }

        [HttpDelete("DeleteOffer/{id}")]
        public async Task<IActionResult> DeleteExchange([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            var exchange = await  this.exchangeRepository.GetByIdAsync(id);
            if (exchange == null)
            {
                return this.NotFound();
            }

            await this.exchangeRepository.DeleteAsync(exchange);
            return Ok(exchange);
        }
        
        [HttpDelete("DeleteWanted/{id}")]
        public async Task<IActionResult> DeleteWanted([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            var wanted = await  this.wantedMedicineRepository.GetByIdAsync(id);
            if (wanted == null)
            {
                return this.NotFound();
            }

            await this.wantedMedicineRepository.DeleteAsync(wanted);
            return Ok(wanted);
        }
        
        [HttpPut]
                public async Task<IActionResult> PutExchange([FromBody] Exchange exchange)
                {
                    if (!ModelState.IsValid)
                    {
                        return this.BadRequest(ModelState);
                    }
        
                                     
        
                    var response =  await exchangeRepository.UpdateAsync(exchange);
                    if (response==null)
                    {
                        return this.BadRequest();
                    }
                    return Ok(response);
                }
    }
}
