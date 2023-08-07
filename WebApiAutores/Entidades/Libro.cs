using System.ComponentModel.DataAnnotations;
using WebApiAutores.Validaciones;

namespace WebApiAutores.Entidades
{
    public class Libro
    {
        public int id { get; set; }
        [Required]
        [StringLength(maximumLength: 250)]
        [PrimeraLetraMayusculaAtribute]       
        public string Titulo { get; set;}

        
        public List<Comentario> Comentarios { get; set; }
        public List<AutorLibro> AutoresLibros { get; set; }

    }
}
