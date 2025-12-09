using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAMS.Core.Entities
{
    public class Class : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string ClassName { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string SubjectCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Semester { get; set; } = string.Empty;

        public int TeacherId { get; set; }

        [ForeignKey("TeacherId")]
        public Teacher? Teacher { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public ICollection<StudentClassEnrollment> Enrollments { get; set; } = new List<StudentClassEnrollment>();
        public ICollection<QRSession> QRSessions { get; set; } = new List<QRSession>();
    }
}
