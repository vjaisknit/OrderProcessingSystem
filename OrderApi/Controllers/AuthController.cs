using Application.Services;
using Application.Services.Contract;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ViewModel.Auth;

namespace OrderApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterVM model)
        {
            var response = await _authService.RegisterAsync(model);
            if (response.StatusCode == StatusCodes.Status400BadRequest)
            {
                return BadRequest(response);
            }

            return CreatedAtAction(nameof(Register), new { }, response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginVM model)
        {
            var response = await _authService.LoginAsync(model);
            if (response.StatusCode == StatusCodes.Status401Unauthorized)
            {
                return Unauthorized(response);
            }

            return Ok(response);
        }
    }


}
