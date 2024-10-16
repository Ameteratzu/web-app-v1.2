namespace DGPCE.Sigemad.Domain.Modelos
{
    public class ImpactoClasificado
    {
        public ImpactoClasificado(){}

        public int Id { get; set; }
        public string TipoImpacto { get; set; }
        public string GrupoImpacto { get; set; }
        public string SubgrupoImpacto { get; set; }
        public string ClaseImpacto { get; set; }
        public string Descripcion { get; set; }
        public bool RelevanciaGeneral { get; set; }
    }
}
