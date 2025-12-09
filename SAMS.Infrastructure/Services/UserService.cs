using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SAMS.Core.DTOs;
using SAMS.Core.Entities;
using SAMS.Core.Services;
using SAMS.Infrastructure.Data;

namespace SAMS.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _context;

        public UserService(UserManager<AppUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IdentityResult> CreateAdminAsync(CreateUserDto dto)
        {
            var user = new AppUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Admin");
            }
            return result;
        }

        public async Task<IdentityResult> CreateTeacherAsync(CreateTeacherDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var user = new AppUser
                {
                    UserName = dto.Email,
                    Email = dto.Email,
                    FullName = dto.FullName,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, dto.Password);
                if (!result.Succeeded) return result;

                var roleResult = await _userManager.AddToRoleAsync(user, "Teacher");
                if (!roleResult.Succeeded)
                {
                    throw new Exception("Failed to assign role: " + string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                }

                var teacher = new Teacher
                {
                    AppUserId = user.Id,
                    Department = dto.Department,
                    Designation = dto.Designation
                };

                _context.Teachers.Add(teacher);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                // Log error
                return IdentityResult.Failed(new IdentityError { Description = ex.Message });
            }
        }

        public async Task<IdentityResult> CreateStudentAsync(CreateStudentDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var user = new AppUser
                {
                    UserName = dto.Email,
                    Email = dto.Email,
                    FullName = dto.FullName,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, dto.Password);
                if (!result.Succeeded) return result;

                var roleResult = await _userManager.AddToRoleAsync(user, "Student");
                if (!roleResult.Succeeded)
                {
                    throw new Exception("Failed to assign role: " + string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                }

                var student = new Student
                {
                    AppUserId = user.Id,
                    EnrollmentNo = dto.EnrollmentNo,
                    ParentPhone = dto.ParentPhone
                };

                _context.Students.Add(student);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return IdentityResult.Failed(new IdentityError { Description = ex.Message });
            }
        }
        public async Task<IReadOnlyList<UserDto>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var userDtos = new List<UserDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userDtos.Add(new UserDto
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Role = roles.FirstOrDefault() ?? "None",
                    IsActive = user.IsActive
                });
            }

            return userDtos;
        }
    }
}
