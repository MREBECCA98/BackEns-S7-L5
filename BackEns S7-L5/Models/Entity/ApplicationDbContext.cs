using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;



namespace BackEns_S7_L5.Models.Entity
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }


        public DbSet<Artista> Artisti { get; set; }
        public DbSet<Evento> Eventi { get; set; }
        public DbSet<Biglietto> Biglietti { get; set; }
    }
}
