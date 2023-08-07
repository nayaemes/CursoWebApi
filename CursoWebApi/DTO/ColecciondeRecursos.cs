namespace CursoWebApi.DTO
{
    public class ColecciondeRecursos<T>: Recurso where T: Recurso
    {
         public List<T> Valores { get; set; }
    }
}
