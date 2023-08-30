using CursoWebApi;

var builder = WebApplication.CreateBuilder(args);

//Servicio Application InsightsTelemetry desde azure
builder.Services.AddApplicationInsightsTelemetry(op =>
op.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionStrings"]);
var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);



var app = builder.Build();
//instanciamos esta clase para Starup.cs

var servicioLogger = (ILogger<Startup>)app.Services.GetService(typeof(ILogger<Startup>));


//y pasamos serviciologger a la clase Cifigure
startup.Configure(app, app.Environment, servicioLogger);

app.Run();
