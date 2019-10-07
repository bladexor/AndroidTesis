

namespace Farma.Web.Controllers.API
{ 
	using Farma.Web.Data.Repositories;
using System.Linq;
using System.Threading.Tasks;
using Farma.Common.Models;
using Data;
using Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.Extensions.Configuration;

    [Route("api/[Controller]")]
public class AccountController : Controller
{
	private readonly IUserHelper userHelper;
	private readonly IStateRepository stateRepository;
	private readonly IMailHelper mailHelper;
        private readonly IConfiguration configuration;
        private bool confirmEmail;

        public AccountController(
    	IUserHelper userHelper,
    	IStateRepository stateRepository,
         IConfiguration configuration,
        IMailHelper mailHelper)
	{
    	this.userHelper = userHelper;
    	this.stateRepository = stateRepository;
    	this.mailHelper = mailHelper;

            confirmEmail = bool.Parse(configuration["SignIn:AutoConfirmEmail"]);
        }

	[HttpPost]
	public async Task<IActionResult> PostUser([FromBody] NewUserRequest request)
	{
    	if (!ModelState.IsValid)
    	{
        	return this.BadRequest(new Response
        	{
            	IsSuccess = false,
            	Message = "Bad request"
        	});
    	}

    	
    	if (await this.userHelper.ExistUserAsync(request.Email))
    	{
        	return this.BadRequest(new Response
        	{
            	IsSuccess = false,
            	Message = "This email is already registered."
        	});
    	}

    	var city = await this.stateRepository.GetCityAsync(request.CityId);
    	if (city == null)
    	{
        	return this.BadRequest(new Response
        	{
            	IsSuccess = false,
            	Message = "City don't exists."
        	});
    	}

    	var user = new Data.Entities.User
    	{
        	FirstName = request.FirstName,
        	LastName = request.LastName,
        	Email = request.Email,
        	UserName = request.Email,
        	Address = request.Address,
        	PhoneNumber = request.Phone,
        	CityId = request.CityId,
        	City = city
    	};

    	var result = await this.userHelper.AddUserAsync(user, request.Password);
    	if (result != IdentityResult.Success)
    	{
        	return this.BadRequest(result.Errors.FirstOrDefault().Description);
    	}

 

                var myToken = await this.userHelper.GenerateEmailConfirmationTokenAsync(user);

            if (this.confirmEmail)
            {
                //CON CONFIRMACION DE EMAIL AUTOMATICO---------------------------------------------------------------
                await this.userHelper.ConfirmEmailAsync(user, myToken);
            }
            else
            {
                var tokenLink = this.Url.Action("ConfirmEmail", "Account", new
                {
                    userid = user.Id,
                    token = myToken
                }, protocol: HttpContext.Request.Scheme);

                this.mailHelper.SendMail(request.Email, "Email confirmation", $"<h1>Email Confirmation</h1>" +
                    $"To allow the user, " +
                    $"plase click in this link:</br></br><a href = \"{tokenLink}\">Confirm Email</a>");
            }
    	return Ok(new Response
    	{
        	IsSuccess = true,
        	Message = "A Confirmation email was sent. Plese confirm your account and log into the App."
    	});
	}

        [HttpPost]
        [Route("GetUserByEmail")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetUserByEmail([FromBody] RecoverPasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Bad request"
                });
            }

            var user = await this.userHelper.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                return this.BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "User don't exists."
                });
            }

            return Ok(user);
        }

        [HttpPost]
        [Route("RecoverPassword")]
        public async Task<IActionResult> RecoverPassword([FromBody] RecoverPasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Bad request"
                });
            }

            var user = await this.userHelper.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                return this.BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "This email is not assigned to any user."
                });
            }

            var myToken = await this.userHelper.GeneratePasswordResetTokenAsync(user);
            var link = this.Url.Action("ResetPassword", "Account", new { token = myToken }, protocol: HttpContext.Request.Scheme);
            this.mailHelper.SendMail(request.Email, "Password Reset", $"<h1>Recover Password</h1>" +
                $"To reset the password click in this link:</br></br>" +
                $"<a href = \"{link}\">Reset Password</a>");

            return Ok(new Response
            {
                IsSuccess = true,
                Message = "An email with instructions to change the password was sent."
            });
        }

        [HttpPut]
        public async Task<IActionResult> PutUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            var userEntity = await this.userHelper.GetUserByEmailAsync(user.Email);
            if (userEntity == null)
            {
                return this.BadRequest("User not found.");
            }

            var city = await this.stateRepository.GetCityAsync(user.CityId);
            if (city != null)
            {
                userEntity.City = city;
            }

            userEntity.FirstName = user.FirstName;
            userEntity.LastName = user.LastName;
            userEntity.CityId = user.CityId;
            userEntity.Address = user.Address;
            userEntity.PhoneNumber = user.PhoneNumber;

            var respose = await this.userHelper.UpdateUserAsync(userEntity);
            if (!respose.Succeeded)
            {
                return this.BadRequest(respose.Errors.FirstOrDefault().Description);
            }

            var updatedUser = await this.userHelper.GetUserByEmailAsync(user.Email);
            return Ok(updatedUser);
        }

        [HttpPost]
        [Route("ChangePassword")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Bad request"
                });
            }

            var user = await this.userHelper.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                return this.BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "This email is not assigned to any user."
                });
            }

            var result = await this.userHelper.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
            if (!result.Succeeded)
            {
                return this.BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = result.Errors.FirstOrDefault().Description
                });
            }

            return this.Ok(new Response
            {
                IsSuccess = true,
                Message = "The password was changed succesfully!"
            });
        }

    }

}