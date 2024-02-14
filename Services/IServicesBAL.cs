using System.Security.Claims;

namespace Customer_API.Services
{
    public interface IServicesBAL
    {
        Task<string> GetRefreshToken();
        Task<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token);
        Task<string> GenerateAccessToken(string? name);
        Task Update(string? name, string newRefreshToken);
    }
}
