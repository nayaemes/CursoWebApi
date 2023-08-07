using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.DTO;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers.V1
{
    [ApiController]
    [Route("api/v1/libros/{libroid:int}/comentarios")]
    public class ComentariosController : ControllerBase
    {
        private readonly ApplicationDBContextcs contextcs;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;

        public ComentariosController(ApplicationDBContextcs contextcs, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            this.contextcs = contextcs;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        [HttpGet(Name = "ObtenerComentariosLibro")]
        public async Task<ActionResult<List<ComentarioDTO>>> Get(int libroid)
        {
            var exiteLibro = await contextcs.Libros.AnyAsync(libroDB => libroDB.id == libroid);
            if (!exiteLibro)
            {
                return NotFound();
            }
            var comentarios = await contextcs.Comentarios.Where(comenentarioDB => comenentarioDB.LibroID == libroid).ToListAsync();
            return mapper.Map<List<ComentarioDTO>>(comentarios);
        }


        [HttpGet("id:int", Name = "ObtenerCometarios")]
        public async Task<ActionResult<ComentarioDTO>> GetporID(int id)
        {
            var comentario = await contextcs.Comentarios.FirstOrDefaultAsync(comentarioBD => comentarioBD.id == id);
            if (comentario == null)
            {
                return NotFound();
            }
            return mapper.Map<ComentarioDTO>(comentario);

        }

        [HttpPost(Name = "CrearComentario")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post(int libroid, ComentarioCreacionDTO comentarioCreacionDTO)
        {
            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;

            var usuario = await userManager.FindByEmailAsync(email);
            var usuarioid = usuario.Id;
            var exitelibro = await contextcs.Libros.AnyAsync(libroBD => libroBD.id == libroid);

            if (!exitelibro)
            {
                return NotFound();
            }

            var comentario = mapper.Map<Comentario>(comentarioCreacionDTO);
            comentario.LibroID = libroid;
            comentario.Usuarioid = usuarioid;
            contextcs.Add(comentario);
            await contextcs.SaveChangesAsync();

            var comentarioDTO = mapper.Map<ComentarioDTO>(comentario);


            return CreatedAtRoute("ObtenerComentarios", new { comentario.id, libroid }, comentarioDTO);

        }
        [HttpPut("id:int", Name = "ActualizarComentario")]
        public async Task<ActionResult> Put(int libroid, int id, ComentarioCreacionDTO comentarioCreacionDTO)
        {
            var exitelibro = await contextcs.Libros.AnyAsync(libroBD => libroBD.id == libroid);

            if (!exitelibro)
            {
                return NotFound();
            }
            var exitecomentario = await contextcs.Comentarios.AnyAsync(comentarioBD => comentarioBD.id == id);
            if (!exitecomentario)
            {
                return NotFound();
            }

            var comentario = mapper.Map<Comentario>(comentarioCreacionDTO);
            comentario.id = id;
            comentario.LibroID = libroid;

            contextcs.Update(comentario);
            await contextcs.SaveChangesAsync();
            return NoContent();

        }
        }
}
