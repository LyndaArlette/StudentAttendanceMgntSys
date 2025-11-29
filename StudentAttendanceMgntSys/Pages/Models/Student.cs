namespace StudentAttendanceMgntSys.Pages.Models
{
    public class Student
    {
        public int StudentId { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }
        public int CourseId { get; set; }

        public Course? Course { get; set; }
    }
}
