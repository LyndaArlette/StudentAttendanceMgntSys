using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAMS.Core.Entities
{
    public class StudentClassEnrollment : BaseEntity
    {
        public int StudentId { get; set; }

        [ForeignKey("StudentId")]
        public Student? Student { get; set; }

        public int ClassId { get; set; }

        [ForeignKey("ClassId")]
        public Class? Class { get; set; }

        public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
    }
}
