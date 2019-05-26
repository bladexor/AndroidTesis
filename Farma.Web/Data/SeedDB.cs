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
                this.AddState("Amazonas");
                this.AddState("Anzoátegui");
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
    }

}
