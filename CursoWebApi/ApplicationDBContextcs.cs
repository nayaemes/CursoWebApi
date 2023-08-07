using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CursoWebApi.Entidades;

namespace CursoWebApi
{
    public class ApplicationDBContextcs : IdentityDbContext 

    {
        public ApplicationDBContextcs(DbContextOptions options) : base(options)
        {
        }
       // funcion para crear llaves primarias compuestas
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AutorLibro>().HasKey(al => new { al.Autorid, al.Libroid });
        }

        public DbSet<Autor> Autores { get; set; }
        public DbSet<Libro> Libros { get; set; }

        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<AutorLibro> AutoresLibros { get; set; }
    }

}

