using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MisCanchas.Domain.Entities;

namespace MisCanchas.Data
{
    public class MisCanchasDbContext : IdentityDbContext
    {
        public MisCanchasDbContext(DbContextOptions<MisCanchasDbContext> options) : base(options)
        { 
        }
        public MisCanchasDbContext() // Agrega este constructor sin parámetros
        {
        }


        public DbSet<Turn> Turns { get; set; }
        public DbSet<Client> Clients { get; set; } 
        public DbSet<Field> Fields { get; set; } 
        public DbSet<Report> Reports { get; set; }
    }
}
