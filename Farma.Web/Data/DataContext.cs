
namespace Farma.Web.Data
{
    using Farma.Web.Data.Entities;
    using Microsoft.EntityFrameworkCore;

    public class DataContext : DbContext
    {
        public DbSet<State> States { get; set; }

        public DbSet<City> Cities { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
    }

}
