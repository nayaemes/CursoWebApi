namespace WebApiAutores.Servicios
{
    
    public class EscribirenArchivo : IHostedService

    {
        private readonly IWebHostEnvironment env;
        private readonly string NombreArchivo = "Arvhivo.txt";
        private Timer timer;


        public EscribirenArchivo(IWebHostEnvironment env)
        {
            this.env = env;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            Escribir("Proceso iniciado");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer.Dispose();
            Escribir("Proceso finalizado");
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            Escribir ("prueba de ejecucion" + DateTime .Now.ToString ("dd/mm/aaaa hh:mm:ss"));
        }

        private void Escribir(string mensaje)
        { 
            var ruta =$@"{env.ContentRootPath }\txt\{NombreArchivo}";
            using (StreamWriter writer = new StreamWriter (ruta ,append:true ))
            {
                writer.WriteLine(mensaje);
            }
        }
    }
}
