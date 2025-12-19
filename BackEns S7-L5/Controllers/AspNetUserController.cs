using BackEns_S7_L5.Models.DTOs.request;
using BackEns_S7_L5.Models.DTOs.response;
using BackEns_S7_L5.Models.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BackEns_S7_L5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AspNetUserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AspNetUserController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto registerRequestDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationUser user = new ApplicationUser()
                    {
                        UserName = registerRequestDto.UserName,
                        Email = registerRequestDto.Email,
                        Nome = registerRequestDto.Nome,
                        Cognome = registerRequestDto.Cognome,
                        PhoneNumber = registerRequestDto.PhoneNumber,
                        Birthday = registerRequestDto.Birthday,
                        CreateAt = DateTime.UtcNow,
                        Id = Guid.NewGuid().ToString(),
                        IsDeleted = false,
                        EmailConfirmed = true,
                        LockoutEnabled = false,


                    };

                    IdentityResult result = await _userManager.CreateAsync(user, registerRequestDto.Password);
                    if (result.Succeeded)
                    {
                        var roleExist = await this._roleManager.RoleExistsAsync("User");
                        if (!roleExist)
                        {
                            await this._roleManager.CreateAsync(new IdentityRole("User"));
                        }
                        await this._userManager.AddToRoleAsync(user, "User");
                        return Ok();
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return BadRequest();
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            try
            {
                ApplicationUser user = await _userManager.FindByNameAsync(loginRequestDto.UserName);
                if (user is null)
                {
                    return BadRequest();
                }

                Microsoft.AspNetCore.Identity.SignInResult result = await this._signInManager.PasswordSignInAsync(user, loginRequestDto.Password, false, false);
                if (!result.Succeeded)
                {
                    return BadRequest();

                }
                List<string> roles = (await this._userManager.GetRolesAsync(user)).ToList();

                List<Claim> userClaims = new List<Claim>
                    {
                 new Claim(ClaimTypes.NameIdentifier, user.Id),
                 new Claim(ClaimTypes.Name, user.UserName),
                 new Claim(ClaimTypes.Email, user.Email)
                 };

                foreach (string roleName in roles)
                {
                    userClaims.Add(new Claim(ClaimTypes.Role, roleName));

                }


                var key = System.Text.Encoding.UTF8.GetBytes("3ebb6406a32122abfdb20acff8738aff72076a7254b9e209e49a5f6ecb2ef3c2");

                SigningCredentials cred = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256);


                var tokenExpiration = DateTime.Now.AddMinutes(30);

                JwtSecurityToken jwt = new JwtSecurityToken(
                    "https://",
                    "https://",
                     claims: userClaims,
                      expires: tokenExpiration,
                    signingCredentials: cred

                    );

                string token = new JwtSecurityTokenHandler().WriteToken(jwt);
                return Ok(new LoginResponseDto()
                {
                    Token = token,
                    Expiration = tokenExpiration

                });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);

            }

        }
    }
}
