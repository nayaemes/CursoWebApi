using System.ComponentModel.DataAnnotations;
using WebApiAutores.Validaciones;

namespace WebApiAutores.DTO
{
    public class LibroCreacionDTO
    {
        [PrimeraLetraMayusculaAtribute]
        [Required]
        public string Titulo { get; set; }
        public DateTime FechaPublicacion { get; set; }
         
        public List<int> AutoresIds { get; set; }
    }
}
