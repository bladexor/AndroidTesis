


using System.Diagnostics;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding.Internal;
using Microsoft.DotNet.PlatformAbstractions;
using Microsoft.Extensions.Hosting;

namespace Farma.Web.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using Farma.Web.Data.Entities;
    using Farma.Web.Helpers;
    using Microsoft.AspNetCore.Identity;
    using System.Net.Http;
    using Newtonsoft.Json;
   

    public class SeedDb
    {
        private readonly DataContext context;
        private readonly IUserHelper userHelper;
        private Random random;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            this.context = context;
            this.userHelper = userHelper;
            this.random = new Random();
        }

        public async Task SeedAsync()
        {
            
            await this.context.Database.EnsureCreatedAsync();

            var separator = Path.DirectorySeparatorChar;
            var dirSeedPath="Data" + separator + "Seed" + separator;
            //Nombres de Medicinas Obtenidas desde PROVITARED
            if (!this.context.Medicines.Any())
            {
                // var filePath = Path.Combine(AppContext.BaseDirectory, "Data/Seed/Medicines.json");
                
                var filePath = dirSeedPath + "Medicines.json";
                var medicines = JsonConvert.DeserializeObject<List<Medicine>>(File.ReadAllText(filePath));
                context.AddRange(medicines);
                context.SaveChanges();
            }
            
            if (!this.context.States.Any())
            {
                //Cargando estados
                var filePath = dirSeedPath + "States.json";
                var states = JsonConvert.DeserializeObject<List<State>>(File.ReadAllText(filePath));
                
                //Ciudades de Amazonas
                filePath = dirSeedPath + "Cities" + separator+"Amazonas.json";
                var citiesAma=JsonConvert.DeserializeObject<List<City>>(File.ReadAllText(filePath));
                
                var state=states.Find(x=>x.Name.Contains("Amazonas"));
                state.Cities = citiesAma;
                
                //Ciudades de Anzoategui
                filePath = dirSeedPath + "Cities" + separator+"Anzoategui.json";
                var citiesAnz=JsonConvert.DeserializeObject<List<City>>(File.ReadAllText(filePath));
                
                state=states.Find(x=>x.Name.Contains("Anzoátegui"));
                state.Cities = citiesAnz;

                //Ciudades de Apure
                filePath = dirSeedPath + "Cities" + separator + "Apure.json";
                var citiesApu = JsonConvert.DeserializeObject<List<City>>(File.ReadAllText(filePath));

                state = states.Find(x => x.Name.Contains("Apure"));
                state.Cities = citiesApu;

                //Ciudades de Aragua
                filePath = dirSeedPath + "Cities" + separator + "Aragua.json";
                var citiesAra = JsonConvert.DeserializeObject<List<City>>(File.ReadAllText(filePath));

                state = states.Find(x => x.Name.Contains("Aragua"));
                state.Cities = citiesAra;

                //Ciudades de Barinas
                filePath = dirSeedPath + "Cities" + separator + "Barinas.json";
                var citiesBar = JsonConvert.DeserializeObject<List<City>>(File.ReadAllText(filePath));

                state = states.Find(x => x.Name.Contains("Barinas"));
                state.Cities = citiesBar;

                //Ciudades de Bolivar
                filePath = dirSeedPath + "Cities" + separator + "Bolivar.json";
                var citiesBol = JsonConvert.DeserializeObject<List<City>>(File.ReadAllText(filePath));

                state = states.Find(x => x.Name.Contains("Bolívar"));
                state.Cities = citiesBol;

                //Ciudades de Carabobo
                filePath = dirSeedPath + "Cities" + separator + "Carabobo.json";
                var citiesCar = JsonConvert.DeserializeObject<List<City>>(File.ReadAllText(filePath));

                state = states.Find(x => x.Name.Contains("Carabobo"));
                state.Cities = citiesCar;

                //Ciudades de Cojedes
                filePath = dirSeedPath + "Cities" + separator + "Cojedes.json";
                var citiesCoj = JsonConvert.DeserializeObject<List<City>>(File.ReadAllText(filePath));

                state = states.Find(x => x.Name.Contains("Cojedes"));
                state.Cities = citiesCoj;

                //Ciudades de Distrito Capital
                filePath = dirSeedPath + "Cities" + separator + "Distrito Capital.json";
                var citiesDis = JsonConvert.DeserializeObject<List<City>>(File.ReadAllText(filePath));

                state = states.Find(x => x.Name.Contains("Distrito Capital"));
                state.Cities = citiesDis;

                //Ciudades de Sucre
                filePath = dirSeedPath + "Cities" + separator + "Sucre.json";
                var citiesSuc = JsonConvert.DeserializeObject<List<City>>(File.ReadAllText(filePath));

                state = states.Find(x => x.Name.Contains("Sucre"));
                state.Cities = citiesSuc;

                //Ciudades de Yaracuy
                filePath = dirSeedPath + "Cities" + separator + "Yaracuy.json";
                var citiesYar = JsonConvert.DeserializeObject<List<City>>(File.ReadAllText(filePath));

                state = states.Find(x => x.Name.Contains("Yaracuy"));
                state.Cities = citiesYar;

                //Ciudades de Zulia
                filePath = dirSeedPath + "Cities" + separator + "Zulia.json";
                var citiesZul = JsonConvert.DeserializeObject<List<City>>(File.ReadAllText(filePath));

                state = states.Find(x => x.Name.Contains("Zulia"));
                state.Cities = citiesZul;

                context.AddRange(states);
                await context.SaveChangesAsync();
             /* 
                context.AddRange(types);
                context.SaveChanges();
                
                //Amazonas State and Cities
                var citiesAma = new List<City>
                {
                    new City { Name = "Maroa" },
                    new City { Name = "Puerto Ayacucho" },
                    new City { Name = "Puerto Paez" },

                    new City { Name = "San Carlos de Rio Negro" },
                    new City { Name = "San Fernando de Atabapo" },
                    new City { Name = "San Fernando de Guainia" },
                    new City { Name = "San Juan de Manapiare" },
                    new City { Name = "San Simon de Cocuy" },
                    new City { Name = "Santa Rosa de Amanadona" }
                };

                this.AddState("Amazonas", citiesAma);
           */
                
            }

            
            await this.userHelper.CheckRoleAsync("Admin");
            await this.userHelper.CheckRoleAsync("Customer");

            var user = await this.userHelper.GetUserByEmailAsync("bladi135@gmail.com");
            if (user == null)
            {
                user = new User
                {
                    FirstName = "Bladimir",
                    LastName = "Velásquez",
                    Email = "bladi135@gmail.com",
                    UserName = "bladi135@gmail.com",
                    PhoneNumber = "04121907221",
                    Address = "Calle 8, Boyaca 3",
                    City = this.context.States.FirstOrDefault().Cities.FirstOrDefault()
                };

                var result = await this.userHelper.AddUserAsync(user, "123456");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }

                await this.userHelper.AddUserToRoleAsync(user, "Admin");
            }

            var isInRole = await this.userHelper.IsUserInRoleAsync(user, "Admin");
            if (!isInRole)
            {
                await this.userHelper.AddUserToRoleAsync(user, "Admin");
            }

            //Generando Token y Confirmando Email de Usuario
            var token = await this.userHelper.GenerateEmailConfirmationTokenAsync(user);
            await this.userHelper.ConfirmEmailAsync(user, token);
                       

        }

        private void AddState(string name)
        {
            this.context.States.Add(new State
            {
                Name = name
            });
        }

        private void AddState(string name, List<City> cities)
        {
            this.context.States.Add(new State
            {
                Cities=cities,
                Name = name
            });
        }


        private void AddMedicine(string name)
        {
            this.context.Medicines.Add(new Medicine
            {
                Name = name
            });
        }
    }

}
