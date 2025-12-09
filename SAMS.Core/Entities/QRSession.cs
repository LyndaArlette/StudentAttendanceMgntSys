using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAMS.Core.Entities
{
    public class QRSession : BaseEntity
    {
        [Required]
        public string SessionToken { get; set; } = string.Empty; // Unique ID for this specific 15-30s window

        public int ClassId { get; set; }

        [ForeignKey("ClassId")]
        public Class? Class { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpiresAt { get; set; }

        // Location constraints for this session
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double? RadiusMeters { get; set; }
    }
}
