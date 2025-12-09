using Microsoft.EntityFrameworkCore;
using SAMS.Core.DTOs;
using SAMS.Core.Entities;
using SAMS.Core.Services;
using SAMS.Infrastructure.Data;
using System.Security.Cryptography;

namespace SAMS.Infrastructure.Services
{
    public class QRService : IQRService
    {
        private readonly ApplicationDbContext _context;

        public QRService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<QRSessionDto> CreateSessionAsync(CreateQRSessionDto dto)
        {
            // Generate a secure random token
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            
            // Set expiry (e.g., 120 seconds for easier testing)
            var expiresAt = DateTime.UtcNow.AddSeconds(120);

            var session = new QRSession
            {
                ClassId = dto.ClassId,
                SessionToken = token,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = expiresAt,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                RadiusMeters = dto.RadiusMeters
            };

            _context.QRSessions.Add(session);
            await _context.SaveChangesAsync();

            return new QRSessionDto
            {
                Id = session.Id,
                SessionToken = session.SessionToken,
                ExpiresAt = session.ExpiresAt
            };
        }

        public async Task<QRSession?> GetSessionByTokenAsync(string token)
        {
            return await _context.QRSessions
                .FirstOrDefaultAsync(s => s.SessionToken == token);
        }

        public async Task<bool> ValidateSessionAsync(string token)
        {
            var session = await GetSessionByTokenAsync(token);
            if (session == null) return false;

            return session.ExpiresAt > DateTime.UtcNow;
        }
    }
}
