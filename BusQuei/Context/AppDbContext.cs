using BusQuei.Models;
using Microsoft.EntityFrameworkCore;

namespace BusQuei.Context
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Models.Bus> Buses { get; set; }
        public DbSet<Models.Maintenance> Maintenances { get; set; }
        public DbSet<Models.Route> Routes { get; set; }
    }
}
