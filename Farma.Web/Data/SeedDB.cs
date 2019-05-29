using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Farma.Web.Data
{
    using Farma.Web.Data.Entities;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    
    public class SeedDb
    {
        private readonly DataContext context;
        private Random random;

        public SeedDb(DataContext context)
        {
            this.context = context;
            this.random = new Random();
        }

        public async Task SeedAsync()
        {
            await this.context.Database.EnsureCreatedAsync();

            if (!this.context.States.Any())
            {
                //Amazonas State and Cities
                var citiesAma = new List<City>();
                citiesAma.Add(new City { Name = "Maroa" });
                citiesAma.Add(new City { Name = "Puerto Ayacucho" });
                citiesAma.Add(new City { Name = "Puerto Paez" });

                citiesAma.Add(new City { Name = "San Carlos de Rio Negro" });
                citiesAma.Add(new City { Name = "San Fernando de Atabapo" });
                citiesAma.Add(new City { Name = "San Fernando de Guainia" });
                citiesAma.Add(new City { Name = "San Juan de Manapiare" });
                citiesAma.Add(new City { Name = "San Simon de Cocuy" });
                citiesAma.Add(new City { Name = "Santa Rosa de Amanadona" });

                this.AddState("Amazonas",citiesAma);

                //Anzoátegui State and Cities
                var citiesAnz = new List<City>();
                    citiesAnz.Add(new City { Name = "Anaco" });
                    citiesAnz.Add(new City { Name = "Aragua de Barcelona" });
                    citiesAnz.Add(new City { Name = "Atapirire" });

                    citiesAnz.Add(new City { Name = "Barbacoa" });
                    citiesAnz.Add(new City { Name = "Barcelona" });
                    citiesAnz.Add(new City { Name = "Bergantin" });
                    citiesAnz.Add(new City { Name = "Boca de Uchire" });

                    citiesAnz.Add(new City { Name = "Cachipo" });
                    citiesAnz.Add(new City { Name = "Caigua" });
                    citiesAnz.Add(new City { Name = "Cantaura" });
                    citiesAnz.Add(new City { Name = "Clarines" });

                    citiesAnz.Add(new City { Name = "El Carito" });
                    citiesAnz.Add(new City { Name = "El Hatillo" });
                    citiesAnz.Add(new City { Name = "El Morro de Barcelona" });
                    citiesAnz.Add(new City { Name = "El Pao" });
                    citiesAnz.Add(new City { Name = "El Pilar" });
                    citiesAnz.Add(new City { Name = "El Tigre" });

                    citiesAnz.Add(new City { Name = "Guanape" });
                    citiesAnz.Add(new City { Name = "Guanta" });
                    citiesAnz.Add(new City { Name = "Guaribe de Cajigal" });

                    citiesAnz.Add(new City { Name = "La Margarita" });
                    citiesAnz.Add(new City { Name = "Lecherias" });
                    citiesAnz.Add(new City { Name = "Los Altos de Santa Fe" });
                    citiesAnz.Add(new City { Name = "Los Pilones" });

                    citiesAnz.Add(new City { Name = "Mapire" });
                    citiesAnz.Add(new City { Name = "Modulo de Boyacá" });
                    citiesAnz.Add(new City { Name = "Modulo de Chuparin" });
                    citiesAnz.Add(new City { Name = "Mundo Nuevo" });

                    citiesAnz.Add(new City { Name = "Naricual" });
                    citiesAnz.Add(new City { Name = "Onoto" });

                    citiesAnz.Add(new City { Name = "Pariaguan" });
                    citiesAnz.Add(new City { Name = "Pertigalete" });
                    citiesAnz.Add(new City { Name = "Piritu" });
                    citiesAnz.Add(new City { Name = "Pozuelos" });
                    citiesAnz.Add(new City { Name = "Puerto La Cruz" });
                    citiesAnz.Add(new City { Name = "Puerto Piritu" });

                    citiesAnz.Add(new City { Name = "Sabana de Uchire" });
                    citiesAnz.Add(new City { Name = "San Diego de Cabrutica" });
                    citiesAnz.Add(new City { Name = "San Joaquin" });
                    citiesAnz.Add(new City { Name = "San Jose de Guanipa" });
                    citiesAnz.Add(new City { Name = "San Mateo" });
                    citiesAnz.Add(new City { Name = "San Miguel" });
                    citiesAnz.Add(new City { Name = "San Pablo" });
                    citiesAnz.Add(new City { Name = "San Tome" });
                    citiesAnz.Add(new City { Name = "Santa Ana" });
                    citiesAnz.Add(new City { Name = "Santa Clara" });
                    citiesAnz.Add(new City { Name = "Santa Cruz del Orinoco" });
                    citiesAnz.Add(new City { Name = "Santa Ines" });
                    citiesAnz.Add(new City { Name = "Santa Rosa" });

                    citiesAnz.Add(new City { Name = "Urica" });
                    citiesAnz.Add(new City { Name = "Uverito" });

                    citiesAnz.Add(new City { Name = "Valle Guanape" });
                    citiesAnz.Add(new City { Name = "Zuata" });

                this.AddState("Anzoátegui", citiesAnz);

                //Anzoátegui State and Cities
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
    }

}
