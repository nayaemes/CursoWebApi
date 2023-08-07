using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using WebApiAutores.Filtros;
using WebApiAutores.Middleware;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using WebApiAutores.Servicios;
using WebApiAutores.Utilidades;
using Microsoft.AspNetCore.Mvc.Infrastructure;



namespace WebApiAutores
{
    public class Startup
    {
        public  Startup(IConfiguration configuration) {
            //limpiamos las cabeceras de los clims
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();   
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //con este filtro puedo controlar todas las expeciones a nivel global de la aplicacion
            services.AddControllers(opciones =>
            {
                opciones.Filters.Add(typeof(FiltrodeExcepcion));
            }).AddJsonOptions(x =>
            x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles).AddNewtonsoftJson();
            // INSTANCIAMOS LA CONFIGURACION DE LA CONEXION A LA BBDD
            services.AddDbContext<ApplicationDBContextcs>(options => 
            options.UseSqlServer(Configuration.GetConnectionString ("defaultConnection")));        

           

            //INSTACIOAMOS LA INTERFACE Y QUIERE DECIR QUE CUANDO UNA CLASE REQUIERA UN SERVICIO LE PASAREMOS LA CLASE A Y DE ESTA MANERA TAMBIEN EN ESTE CASO INTACIOAMOS LA DEPENDECIA iLOGGER

            // services.AddTransient<IServicios, ServiciosA>();

            
            //services.AddTransient<ServicioTransient>();
            //services.AddScoped<ServicioScoped>();
            //services.AddSingleton<ServicioSingleton>();

            //services .AddTransient<MiFiltrodeAccion >();
            //services.AddHostedService<EscribirenArchivo >();

            //Filtro de cache
           // services.AddResponseCaching();

            //Filtro de Autenticacion
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opciones => opciones .TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer=false ,
                    ValidateAudience=false ,
                    ValidateLifetime=false ,
                    ValidateIssuerSigningKey=true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(Configuration["llavejwt"])),
                    ClockSkew =TimeSpan.Zero                        
                });


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
           services.AddSwaggerGen(c =>
           {
               c.SwaggerDoc("v1", new  OpenApiInfo { Title = "WebApiAutores", Version = "v1" });
               c.OperationFilter<AgregarParametroHATEOAS>();

               //configurar Swager para que utilize jwt
               c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
               {
                   Name = "Authorization",
                   Type = SecuritySchemeType.ApiKey,
                   Scheme = "Bearer",
                   BearerFormat = "jwt",
                   In = ParameterLocation.Header
               });
               c.AddSecurityRequirement(new OpenApiSecurityRequirement
               {
                   {
                       new OpenApiSecurityScheme
                       {
                           Reference =new OpenApiReference
                           {
                               Type= ReferenceType.SecurityScheme,
                               Id= "Bearer"
                           }
                       },
                       new string []{}
                   }
               });

           });


            services.AddAutoMapper(typeof(Startup));

            //servicios de IdentityFrameworks
            services .AddIdentity<IdentityUser,  IdentityRole>()
                .AddEntityFrameworkStores <ApplicationDBContextcs >()
                .AddDefaultTokenProviders();

            //servicio que se otroga a traves de Cliam para dar parmisos especiales por eje: admin para borrar y hacer tareas admistrativas que unusuario normal no debe hacer
            services.AddAuthorization(opciones =>
            {
                opciones.AddPolicy("EsAdmin", politica => politica.RequireClaim("EsAdmin"));

            });

            // servicio para proteger los datos ecritados
            services.AddDataProtection();
           services.AddTransient<HashService>();

            //los Cors solo sirven para aplicacion web, para aplicacines moviles no es necesario
            //servicios para permitir que la api se llame desde otro dominio o origenes(intercambio de recursos de origen cruzados ) se puede añadir esta politica "WithExposedHeaders()" para exponer cabeceras que se van a devolver
            services.AddCors(opciones =>
            {
                opciones.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("https://apirequest.io").AllowAnyMethod().AllowAnyHeader();
                     });
            });

            services.AddTransient<GeneradorEnlaces>();
            services.AddTransient<HATEOASAutorFiltrerAttribute>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor >();



        }

        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment  env, ILogger <Startup > logger ) {
            // Configure the HTTP request pipeline.


            //forma  de llamar a un middleware desde una clase instanciada, pero exponiendo la clase que utilizamos
            //app.UseMiddleware<LoguearRspuestaHTTPMiddleware>();

            //y con esta forma llamamos solo a la clase publica sin exponer el nombre de la clase que utilizamod

            app.UseLoguearRespuestaHTTP();

            //con MAP puedo cre4ar rutas especificas al iniciar la api, una bifurcacion 
            //app.Map("/ruta1", app =>
            //{
            //    app.Run(async contexto =>
            //    {
            //        await contexto.Response.WriteAsync("prueba de wirelles");
            //    });
            //}  );

            

            if (env.IsDevelopment ())
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
