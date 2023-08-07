using System.ComponentModel.DataAnnotations;

namespace WebApiAutores.Validaciones
{
    public class PrimeraLetraMayusculaAtribute:ValidationAttribute

    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            if(value == null  || string .IsNullOrEmpty (value.ToString() ) )
            {
                return ValidationResult.Success;
            }
               
            var primerletra = value .ToString()[0].ToString ();

            if ( primerletra != primerletra .ToUpper ())
            {
                return new ValidationResult("La primera letras debe ser mayusculas");
            }

            return ValidationResult.Success;

        }
    }
}
