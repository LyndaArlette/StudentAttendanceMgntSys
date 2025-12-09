using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAMS.Core.Entities
{
    public class Student : BaseEntity
    {
        [Required]
        public string AppUserId { get; set; } = string.Empty;

        [ForeignKey("AppUserId")]
        public AppUser? AppUser { get; set; }

        [Required]
        [MaxLength(50)]
        public string EnrollmentNo { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? ParentPhone { get; set; }

        // Navigation properties
        public ICollection<StudentClassEnrollment> Enrollments { get; set; } = new List<StudentClassEnrollment>();
        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    }
}
