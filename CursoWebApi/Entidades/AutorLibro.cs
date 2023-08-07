namespace CursoWebApi.Entidades
{
    public class AutorLibro
    {
        public  int Libroid { get; set; }

        public int Autorid { get; set; }    
        public int Orden   { get; set;}

        public Libro Libro { get; set; }
         public Autor Autor { get; set; }
    }
}
