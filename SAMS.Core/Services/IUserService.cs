using Microsoft.AspNetCore.Identity;
using SAMS.Core.DTOs;
using SAMS.Core.Entities;

namespace SAMS.Core.Services
{
    public interface IUserService
    {
        Task<IdentityResult> CreateAdminAsync(CreateUserDto dto);
        Task<IdentityResult> CreateTeacherAsync(CreateTeacherDto dto);
        Task<IdentityResult> CreateStudentAsync(CreateStudentDto dto);
        Task<IReadOnlyList<UserDto>> GetAllUsersAsync();
    }
}
