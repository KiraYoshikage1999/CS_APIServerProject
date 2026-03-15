using Auth.Data;

using CS_APIServerProject.Services;
using CS_APIServerProject.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AuthController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerDto)
        {
            if (registerDto == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = new AppUser
            {
                Id = Guid.NewGuid(),
                UserName = registerDto.Email,
                Email = registerDto.Email
            };
            var result = await _userManager.
                CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _userManager.AddToRoleAsync(user, "User");

            var accessToken = await _tokenService.
                CreateAccessTokenAsync(user.Id);
            var refreshToken = await _tokenService.
                CreateRefreshTokenAsync(user.Id);

            return Ok(new
            {
                AccessToken = accessToken.Token,
                AccessTokenExpireAtUtc = accessToken.ExpiresAtUtc,
                RefreshToken = refreshToken.Token,
                RefreshExpireAtUtc = refreshToken.ExpireAtUtc
            });



        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginDto)
        {
            var user = await _userManager.
                FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            var passwordCheckResult = await _signInManager.
                CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!passwordCheckResult.Succeeded)
                return Unauthorized("Invalid email or password.");

            var accessToken = await _tokenService.
                CreateAccessTokenAsync(user.Id);
            var refreshToken = await _tokenService.
                CreateRefreshTokenAsync(user.Id);

            return Ok(new AuthResponse
            {
                Accesstoken = accessToken.Token,
                AccessExpiresAtUtc = accessToken.ExpiresAtUtc,
                RefreshToken = refreshToken.Token,
                RefreshExpiresAtUtc = refreshToken.ExpireAtUtc
            });


        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest refreshRequest)
        {
            if (refreshRequest == null ||
                string.IsNullOrEmpty(refreshRequest.RefreshToken))
            {
                return BadRequest("Invalid refresh token.");
            }
            var response = await _tokenService
                .RefreshAsync(refreshRequest.RefreshToken);
            if (response == null)
            {
                return Unauthorized("Invalid or expired refresh token.");
            }
            return Ok(response);
        }

        [Authorize]//logout
        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke(RefreshRequest revokeRequest)
        {
            var succes = await _tokenService
                .RevokeAsync(revokeRequest.RefreshToken);
            if (!succes)
            {
                return NotFound();
            }
            return Ok();
        } 

    }
}
