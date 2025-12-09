using SAMS.Core.DTOs;
using SAMS.Core.Entities;

namespace SAMS.Core.Services
{
    public interface IQRService
    {
        Task<QRSessionDto> CreateSessionAsync(CreateQRSessionDto dto);
        Task<QRSession?> GetSessionByTokenAsync(string token);
        Task<bool> ValidateSessionAsync(string token);
    }
}
