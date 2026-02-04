// Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;
using DailyExpensesTracker.Interfaces;
using DailyExpensesTracker.Models;
using DailyExpensesTracker.Services;

namespace DailyExpensesTracker.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;

        public AuthController(IUserRepository userRepository, IAuthService authService)
        {
            _userRepository = userRepository;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthRequest request)
        {
            // Validate request
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if user exists
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null)
                return Unauthorized(new { message = "Invalid email or password" });

            // Verify password
            if (!_authService.VerifyPassword(request.Password, user.PasswordHash))
                return Unauthorized(new { message = "Invalid email or password" });

            // Generate token
            var token = _authService.GenerateJwtToken(user);

            // Return user info and token
            return Ok(new
            {
                token,
                user = new
                {
                    user.Id,
                    user.Name,
                    user.Email
                }
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            // Validate request
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if user already exists
            if (await _userRepository.UserExistsAsync(request.Email))
                return BadRequest(new { message = "User with this email already exists" });

            // Create user
            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = _authService.HashPassword(request.Password)
            };

            await _userRepository.CreateAsync(user);

            // Generate token
            var token = _authService.GenerateJwtToken(user);

            // Return user info and token
            return Ok(new
            {
                token,
                user = new
                {
                    user.Id,
                    user.Name,
                    user.Email
                }
            });
        }
    }
}