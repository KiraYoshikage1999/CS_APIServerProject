using Auth.Data;
using CS_APIServerProject.Data;
using CS_APIServerProject.DTO;

namespace CS_APIServerProject.Services
{
    public interface ITokenService
    {
        Task<AccessTokenResult> CreateAccessTokenAsync(AppUser user);
        Task<RefreshTokenResult> CreateRefreshTokenAsync(AppUser user);
        Task<AuthResponse?> RefreshAsync(string refreshToken);
        Task<bool> RevokeAsync(string refreshToken);
    }
}
