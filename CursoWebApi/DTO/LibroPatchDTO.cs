using System.ComponentModel.DataAnnotations;
using CursoWebApi.Validaciones;

namespace CursoWebApi.DTO
{
    public class LibroPatchDTO
    {
        [PrimeraLetraMayusculaAtribute]
        [Required]
        public string Titulo { get; set; }
        public DateTime FechaPublicacion { get; set; }
    }
}
