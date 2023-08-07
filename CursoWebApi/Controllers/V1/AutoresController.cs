using AutoMapper;
//using Azure.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CursoWebApi.DTO;
using CursoWebApi.Entidades;
using CursoWebApi.Utilidades;
using CursoWebApi.Filtros;
using CursoWebApi.Servicios;

namespace CursoWebApi.Controllers.V1
{
    [ApiController]
    [Route("api/vautores")]
    [CabeceraEstaPresente ("x-version", "1")]
    //[ApiConventionType(typeof (DefaultApiConventions))]
    //[Route("api/v1/autores")] //el corchete[controller] sustituye el nombre base del controlador en este caso "autores" se puede cambiar el nombre del controlador pero no es recoemndable pq los clientes del web api ambien tendria que actualizar la url
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]//con este filtro protegemos todo los edpoint y ningun usuario prodría acceder a las rutas

    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDBContextcs contexts;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly IAuthorizationService authorizationService;

        public AutoresController(ApplicationDBContextcs contexts, IMapper mapper, IConfiguration configuration, IAuthorizationService authorizationService)
        {
            this.contexts = contexts;
            this.mapper = mapper;
            this.configuration = configuration;
            this.authorizationService = authorizationService;
        }

        //[HttpGet("configuraciones")]
        //public ActionResult <string> ObtenerConfiguraion()
        //{
        //    return configuration["apellido"];
        //}

        //[HttpGet("Guid")]
        ////Filtro de cache, que durante 10 segundos al usuario se le estrá dando la misma respuesta
        //[ResponseCache (Duration =10)]
        //public ActionResult ObtenerGuids()
        //{
        //    return Ok(new
        //    {
        //        AutoresController_Transient = servicioTransient.Guid,
        //        AutoresController_Scopded = servicioScoped.Guid,
        //        AutoresController_Singlrton = servicioSingleton.Guid,
        //        ServiciosA_Transient =  servicio.ObtenerTransient(),
        //        ServiciosA_Scopded= servicio .ObtenerScoped (),
        //        ServiciosA_Singleton = servicio .ObtenerSingleton ()
        //    });
        //}

        //[HttpGet] // ruta: api/autores
        //[HttpGet ("listado")] // podemos agregar otro tipo de http para concatener otra ruta peroo el resultado es la misma ruta: api/autores/listado
        //[HttpGet ("/listado")] // con esta ruta sustituimos toda la ruta del web api ruta: /listado
        ///// [ResponseCache (Duration =10)] con este filtro mantendremos la cache durante 10 seg y la informacion será la misma
        //// [ServiceFilter (typeof (MiFiltrodeAccion ))] //fitrlo de accion

        [HttpGet(Name = "ObtenerAutoresV1")] //api/autores
        [AllowAnonymous] //con esto desprotegemos este endpont creando una ecepcxion 
        [ServiceFilter(typeof(HATEOASAutorFiltrerAttribute))]
        public async Task<ActionResult<List<AutorDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable =contexts.Autores.AsQueryable();

            await HttpContext.InsertarParametrosPaginacionenCabecera(queryable);

            var autores = await queryable.OrderBy (autor => autor .Nombre).Paginar(paginacionDTO ).ToListAsync(); 

            return mapper.Map<List<AutorDTO>>(autores);


        }
        ////Obtener primer registro de la base de datos
        //[HttpGet("primero")] //con esta ruta definida evitamos errores en sawger y ambas peticiones se pueden cargar
        //public async Task <ActionResult<Autor>> PrimerAutor()
        //{
        //    return await contexts .Autores .FirstOrDefaultAsync ();
        //}

        //[HttpGet ("{id : int}")] //con esto vamos a la ruta del web api directamente con un id especifio
        //[ HttpGet ("{id:int}/{param2?}")]// con esta ruta podemos pasar 2 o mas parametros y el segundo es opcional por el signo "?" o ("{id : int}/{param2=persona}") con este le damos un valor para que no sea null
        //  public async Task <ActionResult <Autor>> Get(int id, string param2)
        //  {
        //      var autor = await contexts.Autores .FirstOrDefaultAsync(x => x.Id == id);

        //      if (autor == null)
        //      {
        //          return NotFound ();
        //      }

        //      return autor;
        //  }
        //**** PROGRAMACION NO ASINCRONA QUITAMOS EL  async y el task para devolver un oebjeto directamente pero si debemos mantener ActionResul ya que el NotFund pertenece a ese obejto
        //[HttpGet ("{id :int}")]
        //public ActionResult <Autor> GetAutor(int id)
        //{
        //    var autor = contexts .Autores .FirstOrDefault (x => x.Id == id);

        //    if (autor == null)
        //    {
        //        return NotFound ();
        //    }
        //    return autor;
        //}

        //**** PROGRAMACION NO ASINCRONA QUITAMOS EL  async y el task para devolver un oebjeto directamente pero cambiamos por IActionResult para reternar un OK y luego el objeto
        //[HttpGet("{id :int}")]
        //public IActionResult Get(int id)
        //{
        //    var autor = contexts.Autores.FirstOrDefault(x => x.Id == id);

        //    if (autor == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok( 7); //esto retorna el tipo de valor definido 
        //}
        //************ ESTOS DOS TIPO DE METODOS EXISTEN PQ SE AGREGARON EN VERSIONES DIFERENTES DEL CORE, PERO AMBAS FUNCIONAN CASI IGUAL**************

        //********* EJEMPLOS DE BINDING*****************
        //[HttpGet("nombre") ]
        //public async Task<ActionResult <Autor>> GetBinding([FromServices] string nombre, [FromServices] string libros, [FromRoute] string param2, [FromQuery] string apellido, [FromHeader] int mivalor )
        //{
        //    var autor = await contexts.Autores.FirstOrDefaultAsync();

        //    if (autor == null)
        //    {
        //        return NotFound();
        //    }

        //    return autor;
        //}

        [HttpGet("{id:int}", Name = "ObtenerAutorV1")]// con esta ruta podemos pasar 2 o mas parametros y el segundo es opcional por el signo "?" o ("{id : int}/{param2=persona}") con este le damos un valor para que no sea null
        [AllowAnonymous]
        [ServiceFilter(typeof(HATEOASAutorFiltrerAttribute))]
        //[ProducesResponseType (404)]
        //[ProducesResponseType(200)]
        public async Task<ActionResult<AutorDTOconLibros>> Get(int id)
        {

            var autor = await contexts.Autores
                .Include(autorBD => autorBD.AutoresLibros)
                .ThenInclude(autorlibroBD => autorlibroBD.Libro)
                .FirstOrDefaultAsync(autorBD => autorBD.Id == id);

            if (autor == null)
            {
                return NotFound();
            }

            var dto = mapper.Map<AutorDTOconLibros>(autor);

            return dto;
        }




        [HttpGet("{nombre}", Name = "ObtenerAutoresporNombreV1")] //con esto vamos a la ruta del web api directamente con un nombre especifio
        public async Task<ActionResult<List<AutorDTO>>> GetbyName([FromRoute] string nombre)
        {
            var autores = await contexts.Autores.Where(autorBD => autorBD.Nombre.Contains(nombre)).ToListAsync();

            return mapper.Map<List<AutorDTO>>(autores);


        }
        // en este metodo POST sustituimos por la capa DTO que hemos creado para que los clientes vean las propiedades que necesitan para crear un autor
        [HttpPost(Name = "CrearAutorV1")]
        public async Task<ActionResult> Post([FromBody] AutorCreacionDTO autorCreacionDTO)
        {
            var ExisteautorconmismoNombre = await contexts.Autores.AnyAsync(x => x.Nombre == autorCreacionDTO.Nombre);

            if (ExisteautorconmismoNombre)
            {
                return BadRequest($"Ya existe un auitr con ese nombre {autorCreacionDTO.Nombre}");
            }

            //con este codigo le estamos pasando los datos de la instancias del padre a la clase capa
            var autor = mapper.Map<Autor>(autorCreacionDTO);

            contexts.Add(autor);
            await contexts.SaveChangesAsync();

            var autorDTO = mapper.Map<AutorDTO>(autor);

            return CreatedAtRoute("obtenerAutorV1", new { id = autor.Id }, autorDTO);

        }
        [HttpPut("{id:int}", Name = "ActualizarAutorV1")]
        public async Task<ActionResult> Put(AutorCreacionDTO autorCreacionDTO, int id)
        {
            var exite = await contexts.Autores.AnyAsync(x => x.Id == id);
            if (!exite)
            {
                return BadRequest();
            }
            var autor = mapper.Map<Autor>(autorCreacionDTO);
            autor.Id = id;
            contexts.Update(autor);

            await contexts.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Borrar un Autor
        /// </summary>
        /// <param name="id">Id del autor a borrar</param>
        /// <returns></returns>
        [HttpDelete("{id:int}", Name = "BorrarAutorV1")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await contexts.Autores.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }
            contexts.Remove(new Autor() { Id = id });
            await contexts.SaveChangesAsync();
            return NoContent();
        }


    }
}
