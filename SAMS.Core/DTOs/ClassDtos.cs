using System.ComponentModel.DataAnnotations;

namespace SAMS.Core.DTOs
{
    public class CreateClassDto
    {
        [Required]
        public string ClassName { get; set; } = string.Empty;

        [Required]
        public string SubjectCode { get; set; } = string.Empty;

        [Required]
        public string Semester { get; set; } = string.Empty;

        [Required]
        public int TeacherId { get; set; }
    }

    public class ClassDto : CreateClassDto
    {
        public int Id { get; set; }
        public string TeacherName { get; set; } = string.Empty;
        public int StudentCount { get; set; }
    }
}
