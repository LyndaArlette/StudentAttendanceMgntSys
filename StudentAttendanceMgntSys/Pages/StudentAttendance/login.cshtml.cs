using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace StudentAttendanceMgntSys.Pages.StudentAttendance
{
    public class loginModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public loginModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [BindProperty]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        [BindProperty]
        public string Role { get; set; } = "teacher";

        public string ErrorMessage { get; set; } = string.Empty;
        public string DebugInfo { get; set; } = string.Empty;

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            try
            {
                DebugInfo = $"Starting login for: {Email}";

                if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
                {
                    ErrorMessage = "Please fill in all fields.";
                    return Page();
                }

                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                DebugInfo += $" | Connection string: {(string.IsNullOrEmpty(connectionString) ? "NULL" : "Found")}";

                if (string.IsNullOrEmpty(connectionString))
                {
                    ErrorMessage = "Database connection not configured.";
                    return Page();
                }

                using var connection = new SqlConnection(connectionString);
                DebugInfo += " | Creating connection...";

                connection.Open();
                DebugInfo += " | Connection opened...";

                var query = "SELECT UserId, Name, Email, Password, Role FROM Users WHERE Email = @Email";
                DebugInfo += " | Query prepared...";

                using var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", Email);

                using var reader = command.ExecuteReader();
                DebugInfo += " | Query executed...";

                if (reader.Read())
                {
                    // Defensive reads and safe conversion for UserId
                    int userId = 0;
                    if (!reader.IsDBNull(reader.GetOrdinal("UserId")))
                    {
                        try
                        {
                            userId = Convert.ToInt32(reader["UserId"]);
                        }
                        catch
                        {
                            // fallback to parsing as string then converting
                            int.TryParse(reader["UserId"]?.ToString(), out userId);
                        }
                    }

                    var name = reader["Name"]?.ToString();
                    var dbEmail = reader["Email"]?.ToString();
                    var storedPassword = reader["Password"]?.ToString();
                    var dbRole = reader["Role"]?.ToString();

                    DebugInfo += $" | User found: {name}, DB Role: {dbRole}, Stored Password: {storedPassword}";

                    // Simple password comparison (replace with hash check in production)
                    if (Password == storedPassword)
                    {
                        DebugInfo += " | Password matched!";

                        if (dbRole?.ToLower() != Role.ToLower())
                        {
                            ErrorMessage = $"This email is registered as {dbRole}, not {Role}. Please select the correct role.";
                            return Page();
                        }

                        HttpContext.Session.SetInt32("UserId", userId);
                        HttpContext.Session.SetString("UserName", name ?? "");
                        HttpContext.Session.SetString("UserEmail", dbEmail ?? "");
                        HttpContext.Session.SetString("UserRole", dbRole ?? "");

                        DebugInfo += " | Login successful! Redirecting...";

                        return dbRole?.ToLower() switch
                        {
                            "teacher" => RedirectToPage("/Teacher/Dashboard"),
                            "student" => RedirectToPage("/Student/Dashboard"),
                            "admin" => RedirectToPage("/Admin/Dashboard"),
                            _ => RedirectToPage("/Index")
                        };
                    }
                    else
                    {
                        DebugInfo += " | Password mismatch!";
                        ErrorMessage = "Invalid email or password. Please try again.";
                        return Page();
                    }
                }
                else
                {
                    DebugInfo += " | No user found with this email!";
                    ErrorMessage = "Invalid email or password. Please try again.";
                    return Page();
                }
            }
            catch (SqlException sqlEx)
            {
                DebugInfo += $" | SQL Error: {sqlEx.Message}";
                ErrorMessage = $"Database error: {sqlEx.Message}";
                return Page();
            }
            catch (Exception ex)
            {
                DebugInfo += $" | General Error: {ex.Message}";
                ErrorMessage = $"An error occurred: {ex.Message}";
                return Page();
            }
        }
    }
}       