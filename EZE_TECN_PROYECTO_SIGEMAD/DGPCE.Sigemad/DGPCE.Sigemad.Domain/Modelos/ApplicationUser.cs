using DGPCE.Sigemad.Domain.Common;

namespace DGPCE.Sigemad.Domain.Modelos
{
    public class ApplicationUser : BaseDomainModel<Guid>
    {
        public Guid IdentityId { get; set; }

        public string Nombre { get; set; }

        public string Apellidos { get; set; }

        public string Email { get; set; }
        public string Telefono { get; set; }
    }
}
