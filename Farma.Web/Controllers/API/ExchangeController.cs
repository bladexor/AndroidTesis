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


        public ExchangeController(IExchangeRepository exchangeRepository,
                                  IMedicineRepository medicineRepository,
                                  IUserHelper userHelper)
        {
            this.exchangeRepository = exchangeRepository;
            this.medicineRepository = medicineRepository;
            this.userHelper = userHelper;
        }
        
        [HttpPost]
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
                //Medicine = (Medicine)medicine,
                MedicineId=medicine.Id,
                Date = "",
                //Status=0,
                UserId=user.Id
                
            };

            // user.Donations.Add(entityDonation);
            // userHelper.UpdateUserAsync(user);
            // user.Donations.Last();
            var newDonation = await this.exchangeRepository.CreateAsync(entityExchange);
            return Ok(newDonation);
        }

        [HttpDelete("{id}")]
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
    }
}
