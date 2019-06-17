using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Farma.Web.Data.Entities;
using Farma.Web.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Farma.Web.Controllers.API
{
	[Route("api/[Controller]")]
    public class StatesController:Controller
    {
        
        private readonly IStateRepository stateRepository;

        public StatesController(IStateRepository stateRepository)
        	{
            	this.stateRepository = stateRepository;
        	}
        
        	[HttpGet]
        	public IActionResult GetStates()
            {
	
            	return Ok(this.stateRepository.GetStatesWithCities());
        	}
    }
}