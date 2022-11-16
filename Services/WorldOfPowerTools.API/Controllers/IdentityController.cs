using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WorldOfPowerTools.API.Services;
using WorldOfPowerTools.Domain.Services;

namespace WorldOfPowerTools.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IdentityService _identityService;
        private readonly JwtService _jwtService;

        public IdentityController(IdentityService identityService, JwtService jwtService)
        {
            _identityService = identityService;
            _jwtService = jwtService;
        }

        [HttpPost("auth")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Authorization([Required] string login, [Required] string password)
        {
            try
            {
                var user = await _identityService.Authorization(login, password);
                return user != null ? Ok(_jwtService.GenerateToken(user)) : NotFound("Аутентификация не пройдена");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("registration")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Registration([Required] string login, [Required] string password)
        {
            try
            {
                var user = await _identityService.Registration(login, password);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
