using Farma.Web.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Farma.Web.Controllers.API
{
    [Route("api/[Controller]")]
    public class MedicineController:Controller
    {
        private readonly IMedicineRepository medicineRepository;

        public MedicineController(IMedicineRepository medicineRepository)
        {
            this.medicineRepository = medicineRepository;

        }

        [HttpGet("{s}")]
        public IActionResult GetSuggest(string s)
        {
            return Ok(this.medicineRepository.GetByNameAsync(s));
        }

    }
}
