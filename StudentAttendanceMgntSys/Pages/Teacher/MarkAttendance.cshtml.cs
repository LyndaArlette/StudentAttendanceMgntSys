using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using StudentAttendanceMgntSys.Pages.Models;
using Microsoft.Data.SqlClient;

namespace StudentAttendanceMgntSys.Pages.Teacher
{
    public class MarkAttendanceModel : PageModel
    {
        private readonly IConfiguration _config;
        public List<StudentRecord> Students { get; set; } = new List<StudentRecord>();

        public MarkAttendanceModel(IConfiguration config)
        {
            _config = config;
        }

        public void OnGet()
        {
            string connString = _config.GetConnectionString("DefaultConnection");
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string sql = @"SELECT Students.StudentId, Users.Name 
                               FROM Students
                               INNER JOIN Users ON Students.UserId = Users.UserId";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Students.Add(new StudentRecord
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        });
                    }
                }
            }
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("/Teacher/TeacherDashboard");
        }
    }
}