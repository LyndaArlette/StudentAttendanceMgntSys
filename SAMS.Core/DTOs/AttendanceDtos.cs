using System.ComponentModel.DataAnnotations;
using SAMS.Core.Entities;

namespace SAMS.Core.DTOs
{
    public class MarkAttendanceDto
    {
        [Required]
        public int StudentId { get; set; }

        [Required]
        public int QRSessionId { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? DeviceInfo { get; set; }
    }

    public class AttendanceDto
    {
        public int Id { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string EnrollmentNo { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public AttendanceStatus Status { get; set; }
        public bool LocationVerified { get; set; }
    }
}
