using GestionaleBiblioteca.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionaleBiblioteca.Data
{
    public class GestionaleBibliotecaDbContext : DbContext
    {
        public GestionaleBibliotecaDbContext(DbContextOptions<GestionaleBibliotecaDbContext>
            options) : base(options) { }

        public DbSet<Libro> Libri { get; set; }
    }
}
