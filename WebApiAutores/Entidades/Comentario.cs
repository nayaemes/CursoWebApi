using Microsoft.AspNetCore.Identity;

namespace WebApiAutores.Entidades
{
    public class Comentario
    {
        public int id { get; set; }

        public string Contenido { get; set; }

       
        public int LibroID { get; set;}

        //propiedad de navgacion
        public Libro  Libro { get;}

        public string Usuarioid { get; set; }

        public IdentityUser Usuario { get; set; }
    }
}
