using Microsoft.EntityFrameworkCore;

namespace CursoWebApi.Utilidades
{
    public static class HttpContextExtensions
    {
        public async static Task InsertarParametrosPaginacionenCabecera<T>(this HttpContext httpContext ,IQueryable <T> queryable)
        {
            if(httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }
            double cantidad =await queryable.CountAsync();
            httpContext.Response.Headers.Add("cantidadTotalRegistros", cantidad .ToString());
        }
    }
}
