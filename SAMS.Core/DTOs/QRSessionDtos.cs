using System.ComponentModel.DataAnnotations;

namespace SAMS.Core.DTOs
{
    public class CreateQRSessionDto
    {
        [Required]
        public int ClassId { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double? RadiusMeters { get; set; }
    }

    public class QRSessionDto
    {
        public int Id { get; set; }
        public string SessionToken { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}
