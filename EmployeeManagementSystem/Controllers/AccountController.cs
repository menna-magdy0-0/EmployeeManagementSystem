using EmployeeManagementSystem.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using EmployeeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration config;
        private readonly EmployeeContext context;

        //Inject UserManager
        public AccountController(UserManager<ApplicationUser> UserManager, IConfiguration config,EmployeeContext context)
        {
            userManager = UserManager;
            this.config = config;
            this.context = context;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO UserFromRequest)
        {
            //Console.WriteLine("Register");
            //Validation
            if (ModelState.IsValid)
            {

                // Ensure the default role exists
                var defaultRole = await context.ApplicationRoles
                    .FirstOrDefaultAsync(r => r.Name == "User");

                if (defaultRole == null)
                {
                    return BadRequest("Default role 'User' not found.");
                }


                //Save DB
                ApplicationUser user = new ApplicationUser();
                
                //Mapping
                user.UserName = UserFromRequest.UserName;
                user.Email = UserFromRequest.Email;
                user.ApplicationRoleId = defaultRole.Id;
                IdentityResult result =
                await userManager.CreateAsync(user, UserFromRequest.Password);
                if (result.Succeeded)
                {
                    return Ok("Create");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("Password", error.Description);
                    }
                    return BadRequest(ModelState);
                }

            }
            return BadRequest(ModelState);
        }
        [HttpPost("Login")]//Post api/Account/Login
        public async Task<IActionResult> Login(LoginDTO userFromRequest)
        {
            if (ModelState.IsValid)
            {
                //check
                ApplicationUser userFromDb = await userManager.FindByNameAsync(userFromRequest.UserName);
                if (userFromDb != null)
                {
                    bool found = await userManager.CheckPasswordAsync(userFromDb, userFromRequest.Password);
                    if (found == true)
                    {

                        //generate token <==
                        List<Claim> UserClaims = new List<Claim>();

                        //Token Generated id change (JWT Predefiened Claims)
                        UserClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                        UserClaims.Add(new Claim(ClaimTypes.NameIdentifier, userFromDb.Id.ToString()));
                        UserClaims.Add(new Claim(ClaimTypes.Name, userFromDb.UserName));
                        var UserRoles = await userManager.GetRolesAsync(userFromDb);
                        foreach (var roleName in UserRoles)
                        {
                            UserClaims.Add(new Claim(ClaimTypes.Role, roleName));
                        }
                        SymmetricSecurityKey SignInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:SecritKey"]));
                        SigningCredentials signingCred =
                            new SigningCredentials(SignInKey, SecurityAlgorithms.HmacSha256);

                        //Design Token
                        JwtSecurityToken mytoken = new JwtSecurityToken(
                            issuer: config["JWT:IssuerIP"],
                            audience: config["JWT:AudienceIP"], //consumer -> angular 
                            expires: DateTime.Now.AddHours(2),
                            claims: UserClaims,
                            signingCredentials: signingCred
                            );
                        //generate token response 
                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(mytoken),
                            expiration = DateTime.Now.AddHours(2)
                            //mytoken.ValidTo
                        });
                    }

                }
                ModelState.AddModelError("Username", "Username OR Password Invalid");

            }
            return BadRequest(ModelState);
        }
    }
}
