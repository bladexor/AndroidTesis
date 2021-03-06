﻿
namespace Farma.Web.Data
{
    using Farma.Web.Data.Entities;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class DataContext : IdentityDbContext<User>
    {
        public DbSet<State> States { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<Medicine> Medicines { get; set; }

        public DbSet<Donation> Donations { get; set; }

        public DbSet<Exchange> Exchanges { get; set; }

        public DbSet<WantedMedicine> WantedMedicines { get; set; }

        public DbSet<MedicineLocation> MedicineLocations { get; set; }
        
        public DbSet<Pharmacy> Pharmacies { get; set; }
        
        public DbSet<Partner> Partners { get; set; }
        
        public DbSet<Product> Products { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
    }

}
