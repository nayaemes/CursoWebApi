namespace CursoWebApi.Servicios
{
    public interface IServicios
    {
        Guid ObtenerScoped();
        Guid ObtenerSingleton();
        Guid ObtenerTransient();
        void RealizarTareas();
    }

    public class ServiciosA : IServicios
    {
        private readonly ILogger <ServiciosA > logger;
        private readonly ServicioTransient servicioTransient;
        private readonly ServicioScoped servicioScoped;
        private readonly ServicioSingleton servicioSingleton;

        public ServiciosA (ILogger <ServiciosA > logger, ServicioTransient servicioTransient , ServicioScoped servicioScoped ,
            ServicioSingleton servicioSingleton)
        {
            this.logger = logger;
            this.servicioTransient = servicioTransient;
            this.servicioScoped = servicioScoped;
            this.servicioSingleton = servicioSingleton;
        }

        public Guid ObtenerTransient() { return servicioTransient .Guid ; }

        public Guid ObtenerScoped() { return servicioScoped .Guid; }
        
        public Guid ObtenerSingleton() { return servicioSingleton .Guid; }
        
        public void RealizarTareas()
        {
            throw new NotImplementedException();
        }
    }

    public class ServiciosB : IServicios
    {
        public Guid ObtenerScoped()
        {
            throw new NotImplementedException();
        }

        public Guid ObtenerSingleton()
        {
            throw new NotImplementedException();
        }

        public Guid ObtenerTransient()
        {
            throw new NotImplementedException();
        }

        public void RealizarTareas()
        {
            throw new NotImplementedException();
        }
    }

    public class ServicioTransient
    {
        public Guid Guid = Guid.NewGuid();
    }
    public class ServicioScoped
    {
        public Guid Guid = Guid.NewGuid();
    }

    public class ServicioSingleton
    {
        public Guid Guid = Guid.NewGuid();
    }

}
