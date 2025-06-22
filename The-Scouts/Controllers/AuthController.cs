using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using The_Scouts.DTOs;
using The_Scouts.Services.AuthenticationService;
using TheScouts.Data;

namespace The_Scouts.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;

        public AuthController(UserManager<IdentityUser> userManager,
                              AppDbContext context,
                              TokenService tokenService)
        {
            _userManager = userManager;
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegistrationRequestDTO request)
        {
            var userExist = await _userManager.FindByEmailAsync(request.Email);
            if (userExist != null)
                return BadRequest("User already exists!");

            var user = new IdentityUser
            {
                UserName = request.Email,
                Email = request.Email
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Add to "User" role (not admin)
            await _userManager.AddToRoleAsync(user, "User");

            return Ok("User created successfully");
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDTO>> Authenticate([FromBody] AuthRequestDTO request)
        {
            var managedUser = await _userManager.FindByEmailAsync(request.Email);
            if (managedUser == null)
                return BadRequest("No user was found");

            var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, request.Password);
            if (!isPasswordValid)
                return BadRequest("Not valid credentials");

            var userInDb = _context.Users.FirstOrDefault(u => u.UserName == request.Email);
            if (userInDb is null)
                return Unauthorized();

            var accessToken = await _tokenService.CreateToken(userInDb);
            await _context.SaveChangesAsync();

            return Ok(new AuthResponseDTO
            {
                Id = userInDb.Id,
                Email = userInDb.UserName,
                Token = accessToken
            });
        }
    }
}
