


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

            await this.userHelper.CheckRoleAsync("Admin");
            await this.userHelper.CheckRoleAsync("Customer");


            if (!this.context.States.Any())
            {
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

                //Anzoátegui State and Cities
                var citiesAnz = new List<City>
                {
                    new City { Name = "Anaco" },
                    new City { Name = "Aragua de Barcelona" },
                    new City { Name = "Atapirire" },

                    new City { Name = "Barbacoa" },
                    new City { Name = "Barcelona" },
                    new City { Name = "Bergantin" },
                    new City { Name = "Boca de Uchire" },

                    new City { Name = "Cachipo" },
                    new City { Name = "Caigua" },
                    new City { Name = "Cantaura" },
                    new City { Name = "Clarines" },

                    new City { Name = "El Carito" },
                    new City { Name = "El Hatillo" },
                    new City { Name = "El Morro de Barcelona" },
                    new City { Name = "El Pao" },
                    new City { Name = "El Pilar" },
                    new City { Name = "El Tigre" },

                    new City { Name = "Guanape" },
                    new City { Name = "Guanta" },
                    new City { Name = "Guaribe de Cajigal" },

                    new City { Name = "La Margarita" },
                    new City { Name = "Lecherias" },
                    new City { Name = "Los Altos de Santa Fe" },
                    new City { Name = "Los Pilones" },

                    new City { Name = "Mapire" },
                    new City { Name = "Modulo de Boyacá" },
                    new City { Name = "Modulo de Chuparin" },
                    new City { Name = "Mundo Nuevo" },

                    new City { Name = "Naricual" },
                    new City { Name = "Onoto" },

                    new City { Name = "Pariaguan" },
                    new City { Name = "Pertigalete" },
                    new City { Name = "Piritu" },
                    new City { Name = "Pozuelos" },
                    new City { Name = "Puerto La Cruz" },
                    new City { Name = "Puerto Piritu" },

                    new City { Name = "Sabana de Uchire" },
                    new City { Name = "San Diego de Cabrutica" },
                    new City { Name = "San Joaquin" },
                    new City { Name = "San Jose de Guanipa" },
                    new City { Name = "San Mateo" },
                    new City { Name = "San Miguel" },
                    new City { Name = "San Pablo" },
                    new City { Name = "San Tome" },
                    new City { Name = "Santa Ana" },
                    new City { Name = "Santa Clara" },
                    new City { Name = "Santa Cruz del Orinoco" },
                    new City { Name = "Santa Ines" },
                    new City { Name = "Santa Rosa" },

                    new City { Name = "Urica" },
                    new City { Name = "Uverito" },

                    new City { Name = "Valle Guanape" },
                    new City { Name = "Zuata" }
                };

                this.AddState("Anzoátegui", citiesAnz);

                //Others States and Cities
                this.AddState("Apure");
                this.AddState("Aragua");

                this.AddState("Barinas");
                this.AddState("Bolívar");

                this.AddState("Carabobo");
                this.AddState("Cojedes");

                this.AddState("Delta Amacuro");
                this.AddState("Distrito Capital");

                this.AddState("Falcon");
                this.AddState("Guarico");
                this.AddState("Lara");

                this.AddState("Mérida");
                this.AddState("Miranda");
                this.AddState("Monagas");

                this.AddState("Nueva Esparta");
                this.AddState("Portuguesa");
                this.AddState("Sucre");

                this.AddState("Táchira");
                this.AddState("Trujillo");

                this.AddState("Vargas");
                this.AddState("Yaracuy");
                this.AddState("Zulia");

                await this.context.SaveChangesAsync();
            }

            //Nombres de Medicinas Obtenidas desde PROVITARED
            if (!this.context.Medicines.Any())
            {
                
                //await this.context.SaveChangesAsync();

                this.context.SaveChanges();
            }


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
