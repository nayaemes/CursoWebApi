using Microsoft.AspNetCore.Identity;

namespace CursoWebApi.Entidades
{
    public class Comentario
    {
        public int id { get; set; }

        public string Contenido { get; set; }


        public int Libroid { get; set; }

        //propiedad de navgacion
        public Libro Libro { get; }

        public string Usuarioid { get; set; }

        public IdentityUser Usuario { get; set; }
        
    }
}
