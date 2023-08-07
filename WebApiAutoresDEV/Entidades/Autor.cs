using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiAutoresDEV.Validaciones;

namespace WebApiAutoresDEV.Entidades
{
    public class Autor /*:IValidatableObject*/ 
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="el campo{0} es requerido")] //para que sea un valor requerido para validar
        [StringLength (maximumLength: 120, ErrorMessage ="el campo {0} no debe tner mas de {1} carateres")]
        [PrimeraLetraMayusculaAtribute]
        public string Nombre { get; set; }

        //public List<AutorLibro> AutoresLibros { get; set; }
        //[Range (0, 10)]// validar rangos de valores
        //[NotMapped]  //propiedades que no se correspoenden a una columa de la tabla correspondente 
        //public string Edad { get; set; }
        //[NotMapped]
        //[CreditCard ]// para validar solamente que la tarjeta tenga una numeracion valida
        //public string CreditCard { get; set; }
        //[NotMapped]
        //[Url ] public string Url { get; set; }

        //public  int Menor { get; set; }
        //public int Mayor { get; set; }
        //public List <Libro> Libros { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)        

        //{
        //   if (! string .IsNullOrEmpty(Nombre ))
        //    {
        //        var primeraletra = Nombre[0].ToString ();

        //        if(primeraletra != primeraletra .ToUpper ())
        //        {
        //            yield return  new ValidationResult ("la primera letra debe ser mayuscula",
        //                new string[] {nameof (Nombre) });
        //        }
        //        if (Menor > Mayor )
        //        {
        //            yield return new ValidationResult("este valor no piede ser mas gtande qu rl campo mayir",
        //                new string[] { nameof(Menor) });
        //        }
        //    }
        //}
    }
}
