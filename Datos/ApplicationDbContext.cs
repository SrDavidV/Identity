using CursoIdentity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CursoIdentity.Datos
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options):base(options)
        {
            
        }

        public DbSet<AppUsuario> AppUsuario { get; set; }

    }
}
