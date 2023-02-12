using Core.Entities.Identity;
using Core.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.DTO;
using ShopAPI.Errors;

namespace ShopAPI.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userMngr;
        private readonly SignInManager<AppUser> _signInMngr;
        private readonly ITokenService _tokenSvc;

        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, ITokenService tokenService)
        {
            _userMngr = userManager;
            _signInMngr = signInManager;
            _tokenSvc = tokenService;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDTO))]
        public async Task<ActionResult<UserDTO>> Login([FromBody]LoginDTO login)
        {
            var user = await _userMngr.FindByEmailAsync(login.Email);
            if (user == null) return Unauthorized(new ApiResponse(401));

            var result = await _signInMngr.CheckPasswordSignInAsync(user, login.Password, false);

            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

            return Ok(new UserDTO()
            {
                Email = user.Email,
                JwtToken = _tokenSvc.CreateToken(user),
                DisplayName = user.DisplayName,
            });
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDTO))]
        public async Task<ActionResult<UserDTO>> Register([FromBody]RegisterDTO reg)
        {
            var user = new AppUser
            {
                DisplayName = reg.DisplayName,
                Email = reg.Email,
                UserName = reg.Email
            };

            var result = await _userMngr.CreateAsync(user, reg.Password);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));

            return Ok(new UserDTO()
            {
                Email = user.Email,
                JwtToken = _tokenSvc.CreateToken(user),
                DisplayName = user.DisplayName
            });
        }
    }
}
