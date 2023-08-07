using System.ComponentModel.DataAnnotations;
using WebApiAutores.Validaciones;

namespace WebApiAutores.DTO
{
    public class AutorCreacionDTO
    {

        //CREAMOS TODAS LAS PROPIEDAdesS QUE NECESITMAOS PARA CREAR UN AUTOR
    
        [Required(ErrorMessage = "el campo{0} es requerido")] //para que sea un valor requerido para validar
        [StringLength(maximumLength: 5, ErrorMessage = "el campo {0} no debe tner mas de {1} carateres")]
        [PrimeraLetraMayusculaAtribute]
        public string Nombre { get; set; }
        

    }
}
