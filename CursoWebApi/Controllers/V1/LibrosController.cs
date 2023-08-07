using AutoMapper;
//using Azure;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CursoWebApi.DTO;
using CursoWebApi.Entidades;

namespace CursoWebApi.Controllers.V1
{
    [ApiController]
    [Route("api/v1/libros")]
    public class LibrosController : ControllerBase
    {
        private readonly ApplicationDBContextcs contexts;
        private readonly IMapper mapper;

        public LibrosController(ApplicationDBContextcs contexts, IMapper mapper)
        {
            this.contexts = contexts;
            this.mapper = mapper;
        }

        [HttpGet("{id:int}", Name = "ObtenerLibros")]
        public async Task<ActionResult<LibroDTOconAutores>> Get(int id)
        {
            var libro = await contexts.Libros
                .Include(libroBD => libroBD.AutoresLibros)
                .ThenInclude(autorLibroBD => autorLibroBD.Autor)
                .FirstOrDefaultAsync(x => x.id == id);

            if (libro == null)
            {
                return NotFound();
            }
            libro.AutoresLibros = libro.AutoresLibros.OrderBy(x => x.Orden).ToList();
            return mapper.Map<LibroDTOconAutores>(libro);
        }

        [HttpPost(Name = "CrearLibro")]
        public async Task<ActionResult> Post(LibroCreacionDTO libroCreacionDTO)
        {
            if (libroCreacionDTO.AutoresIds == null)
            {
                return BadRequest("No se puede crear un libro sin autores");
            }

            var autoresIds = await contexts.Autores
                .Where(autorBD => libroCreacionDTO.AutoresIds.Contains(autorBD.Id)).Select(x => x.Id).ToListAsync();

            if (libroCreacionDTO.AutoresIds.Count != autoresIds.Count)
            {
                return BadRequest("no exite uno de los autores enviados");
            }

            var libro = mapper.Map<Libro>(libroCreacionDTO);
            AsignarOrdenAutores(libro);

            contexts.Add(libro);
            await contexts.SaveChangesAsync();

            var libroDTO = mapper.Map<LibroDTO>(libro);

            return CreatedAtRoute("ObtenerLibro", new { libro.id }, libroDTO);
        }

        [HttpPut("id:int", Name = "ActualizarLibro")]
        public async Task<ActionResult> Put(int id, LibroCreacionDTO libroCreacionDTO)
        {
            var libroBD = await contexts.Libros
                .Include(x => x.AutoresLibros)
                .FirstOrDefaultAsync(X => X.id == id);

            if (libroBD == null)
            {
                return NotFound();
            }

            libroBD = mapper.Map(libroCreacionDTO, libroBD);

            AsignarOrdenAutores(libroBD);
            await contexts.SaveChangesAsync();
            return NoContent();
        }

        private void AsignarOrdenAutores(Libro libro)
        {
            if (libro.AutoresLibros != null)
            {
                for (int i = 0; i < libro.AutoresLibros.Count; i++)
                {
                    libro.AutoresLibros[i].Orden = i;
                }
            }
        }

        ////con el JdonPatch actualizamos de forma parcial los regsitros que son necesarios

        [HttpPatch("{id:int}", Name = "PatchLibro")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<LibroPatchDTO> jsonPatchDocument)
        {
            if (jsonPatchDocument == null)
            {
                return BadRequest();
            }
            var libroDB = await contexts.Libros.FirstOrDefaultAsync(x => x.id == id);

            if (libroDB == null)
            {
                return NotFound();
            }
            var libroDTO = mapper.Map<LibroPatchDTO>(libroDB);

            jsonPatchDocument.ApplyTo(libroDTO, ModelState);

            var esValido = TryValidateModel(libroDTO);

            if (!esValido)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(libroDTO, libroDB);
            await contexts.SaveChangesAsync();

            return NoContent();

        }

        [HttpDelete("{id:int}", Name = "EliminarLibro")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await contexts.Libros.AnyAsync(x => x.id == id);
            if (!existe)
            {
                return NotFound();
            }
            contexts.Remove(new Libro() { id = id });
            await contexts.SaveChangesAsync();
            return NoContent();
        }

    }
}

