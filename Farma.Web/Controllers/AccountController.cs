using System;
using System.Diagnostics;

namespace Farma.Web.Controllers
{
    using Farma.Web.Data.Entities;
    using Farma.Web.Data.Repositories;
    using Helpers;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using Models;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;

    public class AccountController : Controller
    {
        private readonly IUserHelper userHelper;
        private readonly IMailHelper mailHelper;
        private readonly IConfiguration configuration;
        private readonly IStateRepository stateRepository;
        private readonly ICityRepository cityRepository;

        private bool confirmEmail;

        public AccountController(IUserHelper userHelper,
                                 IMailHelper mailHelper,
                                 IConfiguration configuration,
                                 IStateRepository stateRepository,
                                 ICityRepository cityRepository
                                )
        {
            this.userHelper = userHelper;
            this.mailHelper = mailHelper;
            this.configuration = configuration;
            this.stateRepository = stateRepository;
            this.cityRepository = cityRepository;

             confirmEmail= bool.Parse(configuration["SignIn:AutoConfirmEmail"]);

        }

        public IActionResult Login()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction("Index", "Home");
            }

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (this.ModelState.IsValid)
            {
               
                var result = await this.userHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    if (this.Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return this.Redirect(this.Request.Query["ReturnUrl"].First());
                    }

                    return this.RedirectToAction("Index", "Home");
                }
            }

            this.ModelState.AddModelError(string.Empty, "Failed to login.");
            return this.View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await this.userHelper.LogoutAsync();
            return this.RedirectToAction("Index", "Home");
        }


        public IActionResult Register()
        {
            var model = new RegisterNewUserViewModel
            {
                States = this.stateRepository.GetComboStates(),
                Cities = this.stateRepository.GetComboCities(0)
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterNewUserViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await this.userHelper.GetUserByEmailAsync(model.Username);
                if (user == null)
                {
                    //var city = await this.cityRepository.GetByIdAsync(model.CityId);
                    var city= await this.stateRepository.GetCityAsync(model.CityId);
                    //  city.Id = 0;

                    user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Username,
                        UserName = model.Username,
                        Address = model.Address,
                        PhoneNumber = model.PhoneNumber,
                        //CityId = model.CityId,
                        City = city

                    };

                    var result = await this.userHelper.AddUserAsync(user, model.Password);
                    if (result != IdentityResult.Success)
                    {
                        this.ModelState.AddModelError(string.Empty, "The user couldn't be created.");
                        return this.View(model);
                    }

                    var myToken = await this.userHelper.GenerateEmailConfirmationTokenAsync(user);

                    if (this.confirmEmail) {
                        //CON CONFIRMACION DE EMAIL AUTOMATICO---------------------------------------------------------------
                        await this.userHelper.ConfirmEmailAsync(user, myToken);

                        var loginViewModel = new LoginViewModel
                        {
                            Password = model.Password,
                            RememberMe = false,
                            Username = model.Username
                        };

                        var result2 = await this.userHelper.LoginAsync(loginViewModel);

                        if (result2.Succeeded)
                        {
                            return this.RedirectToAction("Index", "Home");
                        }

                        this.ModelState.AddModelError(string.Empty, "The user couldn't be login.");
                    }
                    else
                    {
                        //CONFIRMACION DE EMAIL POR PARTE DEL USUARIO REQUERIDO--------------------------------------------------------
                        var tokenLink = this.Url.Action("ConfirmEmail", "Account", new
                        {
                            userid = user.Id,
                            token = myToken
                        }, protocol: HttpContext.Request.Scheme);

                        this.mailHelper.SendMail(model.Username, "Farma.Web Email confirmation", $"<h1>Email Confirmation</h1>" +
                            $"To allow the user, " +
                            $"plase click in this link:</br></br><a href = \"{tokenLink}\">Confirm Email</a>");
                        this.ViewBag.Message = "The instructions to allow your user has been sent to email.";

                        return this.RedirectToAction("RegisterSuccess", "Account");

                    }

                    return this.View(model);
                }

                this.ModelState.AddModelError(string.Empty, "The username is already registered.");
            }

            return this.View(model);
        }

        public async Task<IActionResult> ChangeUser()
        {
            var user = await this.userHelper.GetUserByEmailAsync(this.User.Identity.Name);
            var model = new ChangeUserViewModel();

            if (user != null)
            {
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.Address = user.Address;
                model.PhoneNumber = user.PhoneNumber;

                var city = await this.stateRepository.GetCityAsync(user.CityId);
                if (city != null)
                {
                    var state = await this.stateRepository.GetStateAsync(city);
                    if (state != null)
                    {
                        model.StateId = state.Id;
                        model.Cities = this.stateRepository.GetComboCities(state.Id);
                        model.States = this.stateRepository.GetComboStates();
                        model.CityId = user.CityId;
                    }
                }
            }

            model.Cities = this.stateRepository.GetComboCities(model.StateId);
            model.States = this.stateRepository.GetComboStates();
            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUser(ChangeUserViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await this.userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                if (user != null)
                {
                    var city = await this.stateRepository.GetCityAsync(model.CityId);

                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Address = model.Address;
                    user.PhoneNumber = model.PhoneNumber;
                   // user.CityId = model.CityId;
                    user.City = city;
                                       
                    var respose = await this.userHelper.UpdateUserAsync(user);
                    if (respose.Succeeded)
                    {
                        this.ViewBag.UserMessage = "User updated!";
                    }
                    else
                    {
                        this.ModelState.AddModelError(string.Empty, respose.Errors.FirstOrDefault().Description);
                    }

                    var state = await this.stateRepository.GetStateAsync(city);
                    if (state != null)
                    {
                        model.States = this.stateRepository.GetComboStates();
                        model.Cities = this.stateRepository.GetComboCities(state.Id);
                    }
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "User no found.");
                }
            }

            return this.View(model);
        }


        public IActionResult ChangePassword()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await this.userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                if (user != null)
                {
                    var result = await this.userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return this.RedirectToAction("ChangeUser");
                    }
                    else
                    {
                        this.ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "User no found.");
                }
            }

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
           // Debug.Write(model.ToString());
            if (this.ModelState.IsValid)
            {
                var user = await this.userHelper.GetUserByEmailAsync(model.Username);
                if (user != null)
                {
                    var result = await this.userHelper.ValidatePasswordAsync(
                        user,
                        model.Password);

                    if (result.Succeeded)
                    {
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                         };

                            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["Tokens:Key"]));
                            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                            var token = new JwtSecurityToken(
                                this.configuration["Tokens:Issuer"],
                                this.configuration["Tokens:Audience"],
                                claims,
                                expires: DateTime.UtcNow.AddDays(15),
                                signingCredentials: credentials);
                            var results = new
                            {
                                token = new JwtSecurityTokenHandler().WriteToken(token),
                                expiration = token.ValidTo
                            };

                        return this.Created(string.Empty, results);
                    }
                }
            }

            return this.BadRequest();
        }

        
        public IActionResult NotAuthorized()
        {
            return this.View();
        }

        //DEVUELVE UNH JSON PARA EL COMBOBOX DE CIUDADES DE CIERTO PAIS
        public async Task<JsonResult> GetCitiesAsync(int stateId)
        {
            var state = await this.stateRepository.GetStateWithCitiesAsync(stateId);
            return this.Json(state.Cities.OrderBy(c => c.Name));
        }


        //Para Confirmar Email de Usuarios Nuevos
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return this.NotFound();
            }

            var user = await this.userHelper.GetUserByIdAsync(userId);
            if (user == null)
            {
                return this.NotFound();
            }

            var result = await this.userHelper.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return this.NotFound();
            }

            return View();
        }


        public IActionResult RegisterSuccess()
        {
           return View();
        }


        public IActionResult RecoverPassword()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await this.userHelper.GetUserByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "The email doesn't correspont to a registered user.");
                    return this.View(model);
                }

                var myToken = await this.userHelper.GeneratePasswordResetTokenAsync(user);
                var link = this.Url.Action(
                    "ResetPassword",
                    "Account",
                    new { token = myToken }, protocol: HttpContext.Request.Scheme);
                this.mailHelper.SendMail(model.Email, "Farma.Web Password Reset", $"<h1>Shop Password Reset</h1>" +
                    $"To reset the password click in this link:</br></br>" +
                    $"<a href = \"{link}\">Reset Password</a>");
                this.ViewBag.Message = "The instructions to recover your password has been sent to email.";
                return this.View();

            }

            return this.View(model);
        }

        public IActionResult ResetPassword(string token)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await this.userHelper.GetUserByEmailAsync(model.UserName);
            if (user != null)
            {
                var result = await this.userHelper.ResetPasswordAsync(user, model.Token, model.Password);
                if (result.Succeeded)
                {
                    this.ViewBag.Message = "Password reset successful.";
                    return this.View();
                }

                this.ViewBag.Message = "Error while resetting the password.";
                return View(model);
            }

            this.ViewBag.Message = "User not found.";
            return View(model);
        }

        public async Task<IActionResult> Index()
        {
            var users = await this.userHelper.GetAllUsersAsync();
            foreach (var user in users)
            {
                var myUser = await this.userHelper.GetUserByIdAsync(user.Id);
                if (myUser != null)
                {
                    user.IsAdmin = await this.userHelper.IsUserInRoleAsync(myUser, "Admin");
                }
            }

            return this.View(users);
        }

        public async Task<IActionResult> AdminOff(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await this.userHelper.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await this.userHelper.RemoveUserFromRoleAsync(user, "Admin");
            return this.RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AdminOn(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await this.userHelper.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await this.userHelper.AddUserToRoleAsync(user, "Admin");
            return this.RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await this.userHelper.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await this.userHelper.DeleteUserAsync(user);
            return this.RedirectToAction(nameof(Index));
        }


    }

}
