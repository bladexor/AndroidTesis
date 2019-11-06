


using System.Diagnostics;
using System.IO;
using System.Text;
using Farma.Web.Models.Helpers.Farmatodo;
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

            var productos = new List<string>{
                "atamel","allegra",
                "captopril","calcibon", "ciprofloxacina","cefadroxilo",
                "diclofenac",
                "ketoprofeno",
                "omeprazol",
                "prospan"
            };

           // var helperF=new FarmatodoHelper();

           // foreach (var item in productos)
           // {
           //     var resultado = await helperF.BuscarProducto(item);

           //     foreach (var hit in resultado.hits)
           //     {
           //         this.context.Products.Add(new Product
           //         {
           //             Description = hit.description,
           //             ImageUrl = hit.mediaImageUrl,
           //             PartnerName = "Farmatodo"
           //         });
           //     }
           //}

           // //this.context.SaveChanges();

           // var helperL=new LocatelHelper();

           // foreach (var item in productos)
           // {
           //     var resultado = await helperL.FindProduct(item);

           //     foreach (var p in resultado.Products)
           //     {
           //         this.context.Products.Add(new Product
           //         {
           //             Description = p.Description,
           //             ImageUrl = "",
           //            PartnerName = "Locatel"
           //         });
           //     }
           // }

           //var Availavilities= await helperL.GetAvailability("2018027");

           // this.context.SaveChanges();
            
            var separator = Path.DirectorySeparatorChar;
            var dirSeedPath = "Data" + separator + "Seed" + separator;
           
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
                
                //Ciudades
                filePath = dirSeedPath + "Cities.json";
                var cities = JsonConvert.DeserializeObject<List<City>>(File.ReadAllText(filePath));
              
                context.AddRange(states);
                context.AddRange(cities);
                await context.SaveChangesAsync();

            }


            //---------------------------------------------------------
            //  Imagenes Precargadas de los Partners (Farmacias)
            //----------------------------------------------------------
            var dirSource = Path.Combine(Directory.GetCurrentDirectory(),
                                        "Data//Seed//images//");

            var dirDestiny = Path.Combine(Directory.GetCurrentDirectory(),
                                        "wwwroot//images//partners//");

            if (!Directory.Exists(dirDestiny))
            {
                Directory.CreateDirectory(dirDestiny);
            }

                var images = Directory.EnumerateFiles(dirSource);

                foreach (var filepath in images)
                {
                    var filedest = dirDestiny + Path.GetFileName(filepath);

                    if (!File.Exists(filedest))
                    { File.Copy(filepath, filedest); }
                }
            
            //-------------------------------------------------------------

            //Detalles de Partners
            if (!this.context.Partners.Any())
            {
                var filePath = dirSeedPath + "Partners.json";
                var partners = JsonConvert.DeserializeObject<List<Partner>>(File.ReadAllText(filePath));
                context.AddRange(partners);
                context.SaveChanges();


            }

            //Detalles de Pharmacies
            if (!this.context.Pharmacies.Any())
            {
                var filePath = dirSeedPath + "Pharmacies.json";
                var pharmacies = JsonConvert.DeserializeObject<List<Pharmacy>>(File.ReadAllText(filePath));
                context.AddRange(pharmacies);
                context.SaveChanges();
            }


            await this.userHelper.CheckRoleAsync("Admin");
            await this.userHelper.CheckRoleAsync("Customer");
            await this.userHelper.CheckRoleAsync("Partner");


            //Administradores Web
            var admin1 = new User
            {
                FirstName = "Bladimir",
                LastName = "Velásquez",
                Email = "bladi135@gmail.com",
                UserName = "bladi135@gmail.com",
                PhoneNumber = "04261820882",
                Address = "Calle 8, Boyaca 3, Barcelona, Edo. Anzoategui",
                City = this.context.Cities.Find(1597),
            };

            addAUser(admin1, "123456", "Admin");


            var admin2 = new User
            {
                FirstName = "Miguel",
                LastName = "Rojas",
                Email = "miguel@gmail.com",
                UserName = "miguel@gmail.com",
                PhoneNumber = "04166828016",
                Address = "Boyaca 5, detras del Liceo ETA, Barcelona, Edo. Anzoategui",
                City = this.context.Cities.Find(1597),
            };

            addAUser(admin2, "123456", "Admin");

            //Clientes Farma.App
            var customer1 = new User
            {
                FirstName = "Yeniret",
                LastName = "Yeguez",
                Email = "yeni@gmail.com",
                UserName = "yeni@gmail.com",
                PhoneNumber = "04121907221",
                Address = "Calle Sucre, Santa Fe, Edo.Sucre",
                City = this.context.Cities.Find(925),
                Donations =new List<Donation>
                {
                    new Donation{ MedicineId = 1},
                    new Donation{ MedicineId = 3},
                    new Donation{ MedicineId = 5},
                    new Donation{ MedicineId = 100},
                    new Donation{ MedicineId = 150},
                    new Donation{ MedicineId = 200}
                },
                Exchanges = new List<Exchange>
                {
                    new Exchange{ MedicineId = 500 },
                    new Exchange{ MedicineId = 400 },
                    new Exchange{ MedicineId = 800 },
                    new Exchange{ MedicineId = 900 },
                    new Exchange{ MedicineId = 1000 }
                },
                WantedMedicines = new List<WantedMedicine>
                {
                    new WantedMedicine{ MedicineId = 3000 },
                    new WantedMedicine{ MedicineId = 4000 },
                    new WantedMedicine{ MedicineId = 5000 },
                    new WantedMedicine{ MedicineId = 6000 },
                    new WantedMedicine{ MedicineId = 7001 },
                }
              
            };

            addAUser(customer1, "123456", "Customer");

            var customer2 = new User
            {
                FirstName = "Leila",
                LastName = "Guzman",
                Email = "leila@gmail.com",
                UserName = "leila@gmail.com",
                PhoneNumber = "04248265399",
                Address = "Calle 8, Boyaca 3, Barcelona, Anzoategui",
                City = this.context.Cities.Find(1597),
                Donations =new List<Donation>
                {
                    new Donation{ MedicineId = 22},
                    new Donation{ MedicineId = 24},
                    new Donation{ MedicineId = 26},
                    new Donation{ MedicineId = 1000},
                    new Donation{ MedicineId = 1500},
                    new Donation{ MedicineId = 2000}
                },
                Exchanges = new List<Exchange>
                {
                    new Exchange{ MedicineId = 1500 },
                    new Exchange{ MedicineId = 1400 },
                    new Exchange{ MedicineId = 1800 },
                    new Exchange{ MedicineId = 1900 },
                    new Exchange{ MedicineId = 2000 }
                },
                WantedMedicines = new List<WantedMedicine>
                {
                    new WantedMedicine{ MedicineId = 6500 },
                    new WantedMedicine{ MedicineId = 555 },
                    new WantedMedicine{ MedicineId = 888 },
                    new WantedMedicine{ MedicineId = 666 },
                    new WantedMedicine{ MedicineId = 856 },
                }
                
            };

            addAUser(customer2, "123456", "Customer");


            /* Usuarios de Socios (Farmacias)
             var userpartner1 = new User
             {
                 FirstName = "Farmatodo",
                 Email = "farmatodo@gmail.com",
                 UserName = "farmatodo@gmail.com",
                 PhoneNumber = "04xxxxxxxxx",
                 City = this.context.Cities.FirstOrDefault()
             };
             addAUser(userpartner1, "123456", "Partner");

             var userpartner2 = new User
             {
                 FirstName = "Locatel",
                 Email = "locatel@gmail.com",
                 UserName = "locatel@gmail.com",
                 PhoneNumber = "04xxxxxxxxx",
                 City = this.context.Cities.FirstOrDefault()
             };

             addAUser(userpartner2, "123456", "Partner");

             var userpartner3 = new User
             {
                 FirstName = "Fundafarmacia",
                 Email = "fundafarmacia@gmail.com",
                 UserName = "fundafarmacia@gmail.com",
                 PhoneNumber = "04xxxxxxxxx",
                 City = this.context.Cities.FirstOrDefault()
             };

             addAUser(userpartner3, "123456", "Partner");

             var userpartner4 = new User
             {
                 FirstName = "Farmacia SAAS",
                 Email = "farmacia.saas@gmail.com",
                 UserName = "farmacia.saas@gmail.com",
                 PhoneNumber = "04xxxxxxxxx",
                 City = this.context.Cities.FirstOrDefault()
             };

             addAUser(userpartner4, "123456", "Partner");
          */

          


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


        private  async void addAUser(User newuser, String password, String role)
        {
            var user = await this.userHelper.GetUserByEmailAsync(newuser.Email);
            
            if (user == null)
            {
                user = newuser;
                
                var result = await this.userHelper.AddUserAsync(user, password);
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }

                await this.userHelper.AddUserToRoleAsync(newuser, role);
            }

            var isInRole = await this.userHelper.IsUserInRoleAsync(user, role);
            if (!isInRole)
            {
                await this.userHelper.AddUserToRoleAsync(user, role);
            }
            
            //Generando Token y Confirmando Email de Usuario
            var token = await this.userHelper.GenerateEmailConfirmationTokenAsync(user);
            await this.userHelper.ConfirmEmailAsync(user, token);
        }
    }

}
