using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using WebApiAutores.DTO;

namespace WebApiAutores.Servicios
{
    public class GeneradorEnlaces
    {
        private readonly IAuthorizationService authorizationService;
        private readonly HttpContextAccessor httpContextAccessor;
        private readonly IActionContextAccessor actionContextAccessor;

        public GeneradorEnlaces( IAuthorizationService authorizationService, HttpContextAccessor httpContextAccessor, IActionContextAccessor actionContextAccessor)
        {
            this.authorizationService = authorizationService;
            this.httpContextAccessor = httpContextAccessor;
            this.actionContextAccessor = actionContextAccessor;
        }

        private IUrlHelper  ConstruirURLHelper()
        {
            var factoria = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IUrlHelperFactory>();
            return factoria.GetUrlHelper(actionContextAccessor.ActionContext);

        }

        private async  Task<bool> EsAdmin()
        {
         var httpcontext = httpContextAccessor. HttpContext;
            var resultado = await authorizationService.AuthorizeAsync(httpcontext.User  , "esAdmin");
            return resultado.Succeeded;

        }
           
            
            public  async  Task GenerarEnlaces(AutorDTO autorDTO )
            {

            var Url = ConstruirURLHelper();
            var esAdmin = await EsAdmin();

                autorDTO.Enlaces.Add(new DatoHATEOAS(
                    Url.Link("ObtenerAutor", new { id = autorDTO.id }),
                    descrpcion: "self",
                    metodo: "GET"));

                if (esAdmin)
                {
                    autorDTO.Enlaces.Add(new DatoHATEOAS(
                  Url.Link("ActualizarAutor", new { id = autorDTO.id }),
                  descrpcion: "self",
                  metodo: "PUT"));

                    autorDTO.Enlaces.Add(new DatoHATEOAS(
                      Url.Link("BorrarAutor", new { id = autorDTO.id }),
                      descrpcion: "self",
                      metodo: "PUT"));
                }


            }
    }
}
