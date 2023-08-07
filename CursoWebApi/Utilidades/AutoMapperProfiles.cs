using AutoMapper;
using CursoWebApi.DTO;
using CursoWebApi.Entidades;

namespace CursoWebApi.Utilidades
{
    public class AutoMapperProfiles:Profile
    {
         public AutoMapperProfiles()
         {
            //mapaeamos de la clase capa a la clase padre
            CreateMap<AutorCreacionDTO, Autor>();

            CreateMap<Autor, AutorDTO>();

            CreateMap<Autor, AutorDTOconLibros>()
                .ForMember(autorDTO => autorDTO.Libros, opciones => opciones.MapFrom(MapAutorDTOLibros));


            CreateMap<LibroCreacionDTO, Libro>()
                .ForMember(libro => libro.AutoresLibros, opciones => opciones.MapFrom(MapAutoresLibros));

            CreateMap<Libro, LibroDTO>().ReverseMap ();

            CreateMap<Libro, LibroDTOconAutores>()
                .ForMember(libroDTO => libroDTO.Autores, opciones => opciones.MapFrom(MapLibroDTOAutores));

            CreateMap<LibroPatchDTO, Libro>().ReverseMap();

            CreateMap<ComentarioCreacionDTO, Comentario>();

            CreateMap<Comentario, ComentarioDTO>();

         }

        private List<AutorLibro> MapAutoresLibros(LibroCreacionDTO libroCreacionDTO, Libro libro)
        {
            var resultado = new List<AutorLibro>();
            if (libroCreacionDTO.AutoresIds == null)
            {
                return resultado;
            }
            foreach (var autorid in libroCreacionDTO.AutoresIds)
            {
                resultado.Add(new AutorLibro { Autorid = autorid });
            }
            return resultado;
        }
        private List<AutorDTO> MapLibroDTOAutores(Libro libro, LibroDTOconAutores libroDTO)
        {
            var resultado = new List<AutorDTO>();
            if (libro.AutoresLibros == null)
            {
                return resultado;
            }

            foreach (var auotrlibro in libro.AutoresLibros)
            {
                resultado.Add(new AutorDTO
                {
                    id = auotrlibro.Autorid,
                    Nombre = auotrlibro.Autor.Nombre
                });
            }
            return resultado;
        }

        private List<LibroDTO> MapAutorDTOLibros(Autor autor, AutorDTO autorDTO)
        {
            var resultado = new List<LibroDTO>();
            if (autor.AutoresLibros == null)
            {
                return resultado;
            }
            foreach (var autorlibro in autor.AutoresLibros)
            {
                resultado.Add(new LibroDTO()
                {
                    id = autorlibro.Libroid,
                    Titulo = autorlibro.Libro.Titulo
                });

            }
            return resultado;

        }
    }
}
