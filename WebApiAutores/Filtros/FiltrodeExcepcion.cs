using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiAutores.Filtros
{

    //con este filtro recogeremos todos los errores de la apalicacion 
    public class FiltrodeExcepcion :ExceptionFilterAttribute 
    {
        private readonly ILogger<FiltrodeExcepcion> logger;

        public FiltrodeExcepcion(ILogger <FiltrodeExcepcion> logger )
        {
            this.logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            logger.LogError(context.Exception, context.Exception.Message);
            base.OnException(context);  
        }
    }
}
