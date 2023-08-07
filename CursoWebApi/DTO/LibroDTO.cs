using System.ComponentModel.DataAnnotations;
using CursoWebApi.Validaciones;

namespace CursoWebApi.DTO
{
    public class LibroDTO
    {
        public int id { get; set; }
        [PrimeraLetraMayusculaAtribute]
        [Required]
        public string Titulo { get; set; }
        public DateTime FechaPublicacion { get; set; } 
      
       
        //public  List <ComentarioDTO > Comentarios { get; set; }
       
    }
}
 