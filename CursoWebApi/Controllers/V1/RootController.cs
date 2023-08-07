using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CursoWebApi.DTO;

namespace CursoWebApi.Controllers.V1
{
    [ApiController]
    [Route("api")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RootController : ControllerBase
    {
        private readonly IAuthorizationService authorizationService;

        public RootController(IAuthorizationService authorizationService)
        {
            this.authorizationService = authorizationService;
        }


        [HttpGet(Name = "ObtenerRoot")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<DatoHATEOAS>>> Get()
        {
            var esAdmin = await authorizationService.AuthorizeAsync(User, "esAdmin");
            var datosHeateoas = new List<DatoHATEOAS>();

            datosHeateoas.Add(new DatoHATEOAS(enlace: Url.Link("ObtenerRoot", new { }), descrpcion: "self", metodo: "GET"));

            datosHeateoas.Add(new DatoHATEOAS(enlace: Url.Link("ObtenerAutores", new { }), descrpcion: "autores", metodo: "GET"));

            if (esAdmin.Succeeded)
            {
                datosHeateoas.Add(new DatoHATEOAS(enlace: Url.Link("CrearAutor", new { }), descrpcion: "autor-crear", metodo: "POST"));

                datosHeateoas.Add(new DatoHATEOAS(enlace: Url.Link("CrearLibro", new { }), descrpcion: "crear-libro", metodo: "POST"));
            }


            return datosHeateoas;
        }
    }
}
