namespace CursoWebApi.DTO
{
    public class PaginacionDTO
    {
        public int Pagina { get; set; } = 1;
        private int recordsPorPaginas = 10;
        private readonly int cantidadMaxPaginas = 50;

        public int RecordsPorPaginas
        {
            get
            {
                return recordsPorPaginas;
            }
            set
            {
                recordsPorPaginas = (value > cantidadMaxPaginas) ? cantidadMaxPaginas :value  ;
            }
        }

    }
}
