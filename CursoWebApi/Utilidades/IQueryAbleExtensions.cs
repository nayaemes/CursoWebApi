using CursoWebApi.DTO;

namespace CursoWebApi.Utilidades
{
    public static class IQueryAbleExtensions
    {

        public static IQueryable <T> Paginar<T> (this IQueryable<T> query, PaginacionDTO paginacionDTO)
        {
            return query
                .Skip((paginacionDTO.Pagina - 1) * paginacionDTO.RecordsPorPaginas)
                .Take(paginacionDTO.RecordsPorPaginas);
            
        }
    }
}
