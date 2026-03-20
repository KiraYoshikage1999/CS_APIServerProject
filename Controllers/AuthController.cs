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
        private readonly ILogger _logger;

        public AuthController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService, ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _logger = logger;
        }

        //Registering new user with email and password and returning AccessToken and RefreshToken
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerDto)
        {
            _logger.LogInformation("Trying proceed with registration for email {Email}", registerDto?.Email);

            if (registerDto == null || !ModelState.IsValid)
            {
                _logger.LogWarning("Invalid registration attempt but failed ", registerDto);
                return BadRequest(ModelState);
            }
            //Creating new User by AppUser class and assigning values from registerDto
            var user = new AppUser
            {
                Id = Guid.NewGuid(),
                UserName = registerDto.Email,
                Email = registerDto.Email,
                //Password = registerDto.Password,
            };

            //Creating user with UserManager and passing the user and password from registerDto
            var result = await _userManager.
                CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                _logger.LogWarning("User registration failed for email {Email} with errors: {Errors}", registerDto.Email, result.Errors);
                return BadRequest(result.Errors);
            }
            //Adding to role "User" to the newly created user
            await _userManager.AddToRoleAsync(user, "User");

            //Creating new Tokens.
            var accessToken = await _tokenService.
                CreateAccessTokenAsync(user.Id);
            var refreshToken = await _tokenService.
                CreateRefreshTokenAsync(user.Id);

            //Returning object with AccessToken, AccessTokenExpireAtUtc, RefreshToken and RefreshExpireAtUtc
            return Ok(new
            {
                AccessToken = accessToken.Token,
                AccessTokenExpireAtUtc = accessToken.ExpiresAtUtc,
                RefreshToken = refreshToken.Token,
                RefreshExpireAtUtc = refreshToken.ExpireAtUtc
            });



        }

        //Login user with email and password and returning AccessToken and RefreshToken
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginDto)
        {
            _logger.LogInformation("Trying to Login with email {Email}", loginDto?.Email);

            var user = await _userManager.
                FindByEmailAsync(loginDto.Email);

            if (user == null)
            {
                _logger.LogWarning("Login attempt failed for email {Email}", loginDto?.Email);
                return Unauthorized("Invalid email or password.");
            }

            //Checking password with SignInManager and passing the user, password from loginDto and lockoutOnFailure as false
            var passwordCheckResult = await _signInManager.
                CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!passwordCheckResult.Succeeded)
            {
                _logger.LogWarning("Login attempt failed for email {Email}", loginDto?.Email);
                return Unauthorized("Invalid email or password.");
            }

            //Creating new Tokens.
            var accessToken = await _tokenService.
                CreateAccessTokenAsync(user.Id);
            var refreshToken = await _tokenService.
                CreateRefreshTokenAsync(user.Id);

            //Returning object with AccessToken, AccessTokenExpireAtUtc, RefreshToken and RefreshExpireAtUtc
            return Ok(new AuthResponse
            {
                Accesstoken = accessToken.Token,
                AccessExpiresAtUtc = accessToken.ExpiresAtUtc,
                RefreshToken = refreshToken.Token,
                RefreshExpiresAtUtc = refreshToken.ExpireAtUtc
            });


        }

        //Refreshing AccessToken and RefreshToken with the provided RefreshToken
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest refreshRequest)
        {
            if (refreshRequest == null ||
                string.IsNullOrEmpty(refreshRequest.RefreshToken))
            {
                _logger.LogWarning("Invalid refresh token attempt.");
                return BadRequest("Invalid refresh token.");
            }

            // Calling RefreshAsync method from ITokenService and passing the RefreshToken from refreshRequest
            var response = await _tokenService
                .RefreshAsync(refreshRequest.RefreshToken);

            if (response == null)
            {
                _logger.LogWarning("Refresh token attempt failed for token {Token}", refreshRequest.RefreshToken);
                return Unauthorized("Invalid or expired refresh token.");
            }
            return Ok(response);
        }

        
        [Authorize]//logout
        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke(RefreshRequest revokeRequest)
        {
            // Calling RevokeAsync method from ITokenService and passing the RefreshToken from revokeRequest
            var succes = await _tokenService
                .RevokeAsync(revokeRequest.RefreshToken);

            if (!succes)
            {
                _logger.LogWarning("Revoke token attempt failed for token {Token}", revokeRequest.RefreshToken);
                return NotFound();
            }
            return Ok();
        } 

    }
}
