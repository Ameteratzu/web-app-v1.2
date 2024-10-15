namespace DGPCE.Sigemad.Domain.Common
{
    public abstract class BaseDomainModel
    {
        public int Id { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public Guid? CreadoPor { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public Guid? ModificadoPor { get; set; }
        public DateTime? FechaBorrado { get; set; }
        public Guid? BorradoPor { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
