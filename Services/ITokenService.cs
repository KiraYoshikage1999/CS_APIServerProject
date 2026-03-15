using CS_APIServerProject.DTO;

namespace CS_APIServerProject.Services
{
    public interface ITokenService
    {
        // Use user identifier instead of AppUser to avoid referencing Auth.Data from this interface
        Task<AccessTokenResult> CreateAccessTokenAsync(Guid userId);
        Task<RefreshTokenResult> CreateRefreshTokenAsync(Guid userId);
        Task<AuthResponse?> RefreshAsync(string refreshToken);
        Task<bool> RevokeAsync(string refreshToken);
    }
}
