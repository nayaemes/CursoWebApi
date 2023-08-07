using CursoWebApi.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace CursoWebApi.Tests.PruebasUnitarias
{
    [TestClass]
    public class PrimeraLetraMayusculaAtributeTest
    {
        [TestMethod]
        public void PrimeraLetraminusculadevuelveError()
        {
            //Preparacion
            var primeraLetraMayuscula = new PrimeraLetraMayusculaAtribute();
            var valor = "naiara";
            var valContext = new ValidationContext(new { Nombre = valor });

            //Ejecucion
            var resultado = primeraLetraMayuscula.GetValidationResult(valor, valContext);

            //Verificacion

            //****** Assert permite hacer verificaciones y si no es correcta salta un error y la prueba queda como que no ha sido aprobada
            Assert.AreEqual("La primera letras debe ser mayusculas", resultado.ErrorMessage);
        }


        [TestMethod]
        public void ValorNuloDevuelveError()
        {
            //Preparacion
            var primeraLetraMayuscula = new PrimeraLetraMayusculaAtribute();
            string valor =null;
            var valContext = new ValidationContext(new { Nombre = valor });

            //Ejecucion
            var resultado = primeraLetraMayuscula.GetValidationResult(valor, valContext);

            //Verificacion

            //****** Assert permite hacer verificaciones y si no es correcta salta un error y la prueba queda como que no ha sido aprobada
            Assert.IsNull(resultado);
        }
    }
}