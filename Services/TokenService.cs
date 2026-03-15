using CS_APIServerProject.Data;
using CS_APIServerProject.DTO;
using Microsoft.AspNetCore.Identity;
using System.Net.WebSockets;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CS_APIServerProject.Models;
using Microsoft.EntityFrameworkCore;
using Auth.Data;
using CS_APIServerProject.Model;

namespace CS_APIServerProject.Services
{
    public class TokenService : ITokenService   
    {
        //Declaring connection Classes 
        public readonly IConfiguration _configuration;
        public readonly DataBaseContext _dbContext;
        public readonly UserManager<AppUser> _userManager;

        //Ussual constructor
        public TokenService(UserManager<AppUser> userManager, 
            IConfiguration configuration, DataBaseContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _configuration = configuration;
        }

        // New interface-compatible methods that accept userId to avoid exposing AppUser in the service contract
        public async Task<AccessTokenResult> CreateAccessTokenAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) throw new ArgumentNullException(nameof(user));
            return await CreateAccessTokenAsync(user);
        }

        public async Task<RefreshTokenResult> CreateRefreshTokenAsync(Guid userId) 
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) throw new ArgumentNullException(nameof(user));
            return await CreateRefreshTokenAsync(user);
        }

        //
        public async Task<AccessTokenResult> CreateAccessTokenAsync(AppUser user)
        {

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim(ClaimTypes.Name, user.UserName ?? "")
            };

            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }


            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
                );

            var minutes = int.Parse(_configuration["Jwt:AccessTokenMinutes"] ?? "30");
            var expire = DateTime.UtcNow.AddMinutes(minutes);


            var jwt = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expire,
                signingCredentials: credentials
                );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(jwt);


            return new AccessTokenResult
            {
                Token = tokenString,
                ExpiresAtUtc = expire
            };
        }

        public async Task<RefreshTokenResult> CreateRefreshTokenAsync(AppUser user)
        {

            var bytes = RandomNumberGenerator.GetBytes(32);
            var refreshToken = Convert.ToBase64String(bytes);

            var days = int.Parse(_configuration["Jwt:RefreshTokenDays"] ?? "7");
            var expires = DateTime.UtcNow.AddDays(days);

            var entity = new RefreshToken
            {
                Token = refreshToken,
                ExpireAtUtc = expires,
                IsRevoke = false,
                UserId = user.Id
            };

            _dbContext.RefreshTokens.Add(entity);
            await _dbContext.SaveChangesAsync();

            return new RefreshTokenResult
            {
                Token = refreshToken,
                ExpireAtUtc = expires
            };
        }

        public async Task<AuthResponse?> RefreshAsync(string refreshToken)
        {
            var tokenEntity = await _dbContext.RefreshTokens
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Token == refreshToken);

            if (tokenEntity == null) 
                return null;

            if(tokenEntity.IsRevoke)
                return null;

            if(tokenEntity.ExpireAtUtc <= DateTime.UtcNow) return null;

            var user = tokenEntity.User;

            var access = await CreateAccessTokenAsync(user);
            var refresh = await CreateRefreshTokenAsync(user);

            await _dbContext.SaveChangesAsync();
            return new AuthResponse
            {
                Accesstoken = access.Token,
                AccessExpiresAtUtc = access.ExpiresAtUtc,
                RefreshToken = refresh.Token,
                RefreshExpiresAtUtc = refresh.ExpireAtUtc,
            };
        }

        public async Task<bool> RevokeAsync(string refreshToken)
        {
            var tokenEnitity = await _dbContext.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == refreshToken);
            
            if (tokenEnitity == null) return false;
            
            tokenEnitity.IsRevoke = true;

            await _dbContext.SaveChangesAsync();
            return true;

        }
    }
}
