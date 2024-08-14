namespace DGPCE.Sigemad.Identity.Models
{
    public class ApplicationUser : BaseDomainModel
    {
        public Guid IdentityId { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Apellidos { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
    }
}
