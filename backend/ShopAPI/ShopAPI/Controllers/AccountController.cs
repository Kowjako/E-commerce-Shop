using AutoMapper;
using Core.Entities.Identity;
using Core.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopAPI.DTO;
using ShopAPI.Errors;
using System.Security.Claims;

namespace ShopAPI.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userMngr;
        private readonly SignInManager<AppUser> _signInMngr;
        private readonly ITokenService _tokenSvc;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper)
        {
            _userMngr = userManager;
            _signInMngr = signInManager;
            _tokenSvc = tokenService;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            // during generation jwt token we add email claim
            // so here we have access to authenticated user claims
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userMngr.FindByEmailAsync(email);

            return new UserDTO()
            {
                Email = user.Email,
                JwtToken = _tokenSvc.CreateToken(user),
                DisplayName = user.DisplayName,
            };
        }

        // Or we can turn off inside IdentityServiceExtensions
        // AddIdentityCore -> opt.User.RequireUniqueEmail = false;
        [HttpGet("email-exists")]
        public async Task<ActionResult<bool>> CheckUserEmailExists([FromQuery] string email)
        {
            return await _userMngr.FindByEmailAsync(email) != null;
        }

        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDTO>> GetUserAddress()
        {
            // we will not have user populated without [Authorize] attribute
            var email = User.FindFirstValue(ClaimTypes.Email);

            var user = await _userMngr.Users.Include(p => p.Address)
                .FirstOrDefaultAsync(p => p.Email.Equals(email));

            return Ok(_mapper.Map<AddressDTO>(user.Address));
        }

        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDTO>> UpdateAddress([FromBody] AddressDTO address)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var user = await _userMngr.Users.Include(p => p.Address)
                .FirstOrDefaultAsync(p => p.Email.Equals(email));

            user.Address = _mapper.Map<Address>(address);
            var result = await _userMngr.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok(_mapper.Map<AddressDTO>(user.Address));
            }
            return BadRequest("Problem updating user");
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDTO))]
        public async Task<ActionResult<UserDTO>> Login([FromBody] LoginDTO login)
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
        public async Task<ActionResult<UserDTO>> Register([FromBody] RegisterDTO reg)
        {
            if (await _userMngr.FindByEmailAsync(reg.Email) != null)
            {
                return BadRequest(new ApiValidationErrorResponse()
                {
                    Errors = new[]
                    {
                        "Email address is in use"
                    }
                });
            }

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
