using Microsoft.EntityFrameworkCore;

namespace WebApiAutoresDEV
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            //limpiamos las cabeceras de los clims
           // JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            // INSTANCIAMOS LA CONFIGURACION DE LA CONEXION A LA BBDD
            services.AddDbContext<ApplicationDBContextcs>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));

        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            // Configure the HTTP request pipeline.


            //forma  de llamar a un middleware desde una clase instanciada, pero exponiendo la clase que utilizamos
            //app.UseMiddleware<LoguearRspuestaHTTPMiddleware>();

            //y con esta forma llamamos solo a la clase publica sin exponer el nombre de la clase que utilizamod

            //app.UseLoguearRespuestaHTTP();

            //con MAP puedo cre4ar rutas especificas al iniciar la api, una bifurcacion 
            //app.Map("/ruta1", app =>
            //{
            //    app.Run(async contexto =>
            //    {
            //        await contexto.Response.WriteAsync("prueba de wirelles");
            //    });
            //}  );



            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }

            // mirelwalles
            app.UseSwagger();
            app.UseSwaggerUI(c =>

            c.SwaggerEndpoint("/swagger/v1/swagger.json", "WepApiAutores v1"));

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseCors();

            //Middleware de cache para filtros
            //app.UseResponseCaching();


            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


        }


    }
}
