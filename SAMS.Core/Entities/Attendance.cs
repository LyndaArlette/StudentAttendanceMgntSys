using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAMS.Core.Entities
{
    public enum AttendanceStatus
    {
        Present,
        Absent,
        Late,
        Excused
    }

    public class Attendance : BaseEntity
    {
        public int StudentId { get; set; }

        [ForeignKey("StudentId")]
        public Student? Student { get; set; }

        public int QRSessionId { get; set; }

        [ForeignKey("QRSessionId")]
        public QRSession? QRSession { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public AttendanceStatus Status { get; set; } = AttendanceStatus.Present;

        public bool LocationVerified { get; set; }
        public double? StudentLatitude { get; set; }
        public double? StudentLongitude { get; set; }
        
        [MaxLength(255)]
        public string? DeviceInfo { get; set; }
    }
}
