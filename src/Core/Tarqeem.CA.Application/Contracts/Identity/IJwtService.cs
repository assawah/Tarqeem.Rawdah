using System.Security.Claims;
using Tarqeem.CA.Application.Models.Jwt;
using Tarqeem.CA.Domain.Entities.User;

namespace Tarqeem.CA.Application.Contracts.Identity;

public interface IJwtService
{
    Task<AccessToken> GenerateAsync(User user);
    Task<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token);
    Task<AccessToken> RefreshToken(Guid refreshTokenId);
}