
namespace DGPCE.Sigemad.Domain.Modelos
{
    public class Distrito
    {      
        public int Id { get; set; }   
        public int IdPais { get; set; }
        public string Descripcion { get; set; }  = null!;
        public virtual Pais Pais { get; set; } = null!;

    }
}
