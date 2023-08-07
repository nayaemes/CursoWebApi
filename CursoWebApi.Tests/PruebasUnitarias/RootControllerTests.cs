using CursoWebApi.Controllers.V1;
using CursoWebApi.Tests.Mosks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CursoWebApi.Tests.PruebasUnitarias
{
    [TestClass]
    public class RootControllerTests
    {
        [TestMethod]
        public async Task SiUsuariosEsAdmin_Obtenemos4Links()
        {
            //Preparacion
            var authorizacionservices = new AutorizationServicesMock();
            authorizacionservices.Resultado = AuthorizationResult.Success();
            var rootController = new RootController(authorizacionservices);
            rootController.Url = new UrlHelperMock();

            //Ejecucion

            var resultado = await rootController.Get();
            //Verificacion
            //****** Assert permite hacer verificaciones y si no es correcta salta un error y la prueba queda como que no ha sido aprobada
            Assert.AreEqual (4, resultado.Value .Count ());
        }

        [TestMethod]
        public async Task SiUsuariosNoEsAdmin_Obtenemos2Links()
        {
            //Preparacion
            var authorizacionservices = new AutorizationServicesMock();
            authorizacionservices.Resultado = AuthorizationResult.Failed();
            var rootController = new RootController(authorizacionservices);
            rootController.Url = new UrlHelperMock();

            //Ejecucion

            var resultado = await rootController.Get();
            //Verificacion
            //****** Assert permite hacer verificaciones y si no es correcta salta un error y la prueba queda como que no ha sido aprobada
            Assert.AreEqual(2, resultado.Value.Count());
        }

        [TestMethod]
        public async Task SiUsuariosNoEsAdmin_Obtenemos2LinksUsandoLibreriaMoq()
        {
            //Preparacion
            var mockAuthorizationServices = new Mock<IAuthorizationService>();
            mockAuthorizationServices .Setup (x=> x.AuthorizeAsync (
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<object>(),
                It.IsAny<IEnumerable<IAuthorizationRequirement >>()            
            )).Returns (Task.FromResult(AuthorizationResult.Failed()));


            
            mockAuthorizationServices.Setup(x => x.AuthorizeAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<object>(),
                It.IsAny<string>()                 
            )).Returns(Task.FromResult(AuthorizationResult.Failed()));


            var mockURLHelper = new Mock<IUrlHelper >();
            mockURLHelper.Setup(x => x.Link (
                It.IsAny<string>(),
                It.IsAny<object>()                
            )).Returns(string .Empty);

            var rootController = new RootController(mockAuthorizationServices.Object);
            rootController.Url =   mockURLHelper.Object;
            //Ejecucion

            var resultado = await rootController.Get();
            //Verificacion
            //****** Assert permite hacer verificaciones y si no es correcta salta un error y la prueba queda como que no ha sido aprobada
            Assert.AreEqual(2, resultado.Value.Count());
        }
    }
}
