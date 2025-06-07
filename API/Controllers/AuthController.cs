using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Register a new user account
        /// </summary>
        /// <param name="registerDto">User registration data</param>
        /// <returns>Authentication token</returns>
        [HttpPost("register")]
        [AllowAnonymous]
        [SwaggerOperation(OperationId = "RegisterUser")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                var result = await _authService.RegisterAsync(registerDto);

                if (!result.Success)
                {
                    _logger.LogWarning("Registration failed for {Email}: {Message}", 
                        registerDto.Email, result.Message);
                    return BadRequest(new { result.Message });
                }

                _logger.LogInformation("New user registered: {Email}", registerDto.Email);
                return Ok(new AuthResponseDto
                {
                    Token = result.Token,
                    ExpiresIn = (int)(result.TokenExpiration?.Subtract(DateTime.UtcNow).TotalSeconds ?? 3600),  // Convert expiration to seconds
                    UserId = result.User?.Id.ToString(),  // Assuming UserDto has an Id
                    Email = registerDto.Email,
                    Role = result.Role.ToString()  // Convert Role (int) to string
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for {Email}", registerDto.Email);
                return StatusCode(StatusCodes.Status500InternalServerError, new { 
                    Message = "An error occurred during registration" 
                });
            }
        }

        /// <summary>
        /// Authenticate user and get JWT token
        /// </summary>
        /// <param name="loginDto">User credentials</param>
        /// <returns>Authentication token</returns>
        [HttpPost("login")]
        [AllowAnonymous]
        [SwaggerOperation(OperationId = "LoginUser")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResponseDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var result = await _authService.LoginAsync(loginDto);

                if (!result.Success)
                {
                    _logger.LogWarning("Login failed for {Email}: {Message}", loginDto.Email, result.Message);
                    return Unauthorized(new { result.Message });
                }

                _logger.LogInformation("User logged in: {Email}", loginDto.Email);
                return Ok(new AuthResponseDto
                {
                    Token = result.Token,
                    ExpiresIn = (int)(result.TokenExpiration?.Subtract(DateTime.UtcNow).TotalSeconds ?? 3600),  // Convert expiration to seconds
                    UserId = result.User?.Id.ToString(),  // Assuming UserDto has an Id
                    Email = loginDto.Email,
                    Role = result.Role.ToString()  // Convert Role (int) to string
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for {Email}", loginDto.Email);
                return StatusCode(StatusCodes.Status500InternalServerError, new { 
                    Message = "An error occurred during login" 
                });
            }
        }

        // Other methods will be similarly adjusted.
    }
}
