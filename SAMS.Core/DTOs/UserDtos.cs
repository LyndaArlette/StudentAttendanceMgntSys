using System.ComponentModel.DataAnnotations;

namespace SAMS.Core.DTOs
{
    public class CreateUserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = string.Empty; // Admin, Teacher, Student
    }

    public class CreateStudentDto : CreateUserDto
    {
        [Required]
        public string EnrollmentNo { get; set; } = string.Empty;
        public string? ParentPhone { get; set; }
    }

    public class CreateTeacherDto : CreateUserDto
    {
        [Required]
        public string Department { get; set; } = string.Empty;
        public string? Designation { get; set; }
    }
}
