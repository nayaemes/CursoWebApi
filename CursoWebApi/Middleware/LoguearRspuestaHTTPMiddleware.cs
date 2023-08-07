using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

namespace CursoWebApi.Middleware
{

    //con esta forma instancioamos una clase Middleware sin poner el nombre de la clase que lo ejecuta
    public static class LoguearRspuestaHTTPMiddlewareExtendida
    {
        public static IApplicationBuilder UseLoguearRespuestaHTTP(this IApplicationBuilder app)
        {
            return app.UseMiddleware<LoguearRspuestaHTTPMiddleware>();
        }
    }

    public class LoguearRspuestaHTTPMiddleware
    {
        private readonly RequestDelegate siguiente;
        private readonly ILogger<LoguearRspuestaHTTPMiddleware> logger;

        public LoguearRspuestaHTTPMiddleware (RequestDelegate siguiente, ILogger <LoguearRspuestaHTTPMiddleware> logger )
        {
            this.siguiente = siguiente;
            this.logger = logger;
        }
        //para usar esta clase como un Middelware es que tiene que tener un método publico Invoke o InvokeAsyn, como usamos progamacion Asincrona esntoces es InvokeAsyn

        public async Task InvokeAsync(HttpContext contexto)
        {
            using (var ms = new MemoryStream())
            {
                var cuerpooriginalRetspuesta = contexto.Response.Body;
                contexto.Response.Body = ms;

                await siguiente(contexto );
                //ahora se ejecutara cuando los middleware posteriores devuelvan una respuesta
                ms.Seek(0, SeekOrigin.Begin);
                string respuesta = new StreamReader(ms).ReadToEnd();
                //ahora ponemos el string en la posicion inicial  para enviar la respuesta al usuario
                ms.Seek(0, SeekOrigin.Begin);

                await ms.CopyToAsync(cuerpooriginalRetspuesta);
                contexto.Response.Body = cuerpooriginalRetspuesta;

                //ahora necesitamos una instacnia de Ilogger para eso vamos a Program
                logger.LogInformation(respuesta);
            }
        }

    }
}
