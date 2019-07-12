
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

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
    }

}
