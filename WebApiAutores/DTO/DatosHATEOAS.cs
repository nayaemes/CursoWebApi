namespace WebApiAutores.DTO
{
    public class DatoHATEOAS
    {
        public   string Elnace { get; set; }
        public   string  Descripcion { get; set; }
        public  string Metodo { get; set; }

        public DatoHATEOAS(string enlace, string descrpcion , string metodo) {
        
        Elnace = enlace;
            Descripcion = descrpcion;   
            Metodo = metodo;    
            }
    }
}
