using MvcAppAws_Daniel_delaCruz.Models;
using Microsoft.EntityFrameworkCore;

namespace MvcAppAws_Daniel_delaCruz.Context
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Alumno> Alumno { get; set; }
    }
}
