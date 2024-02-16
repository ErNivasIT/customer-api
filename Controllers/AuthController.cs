using Customer_API.Models;
using Customer_API.Services;
using DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Customer_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> logger;
        private readonly IConfiguration configuration;
        private readonly IServicesBAL servicesBAL;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AuthController(ILogger<AuthController> logger, IConfiguration configuration, IServicesBAL servicesBAL, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.servicesBAL = servicesBAL;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        [HttpPost]
        [Route("add-user")]
        public async Task<ActionResult<UserResponse>> AddUser(UserRequest oUser)
        {
            UserResponse response = new UserResponse();
            var userResult = await userManager.CreateAsync(new ApplicationUser()
            {
                FirstName = oUser.FirstName,
                MiddleName = oUser.MiddleName,
                LastName = oUser.LastName,
                Email = oUser.Email,
                UserName = oUser.UserName,
            }, oUser.Password);

            if (userResult.Succeeded)
            {
                bool isExists = await roleManager.RoleExistsAsync(oUser.Role);
                if (!isExists)
                    await roleManager.CreateAsync(new IdentityRole() { Name = oUser.Role });
                var userFromDb = await userManager.FindByEmailAsync(oUser.Email);
                await userManager.AddToRoleAsync(userFromDb, oUser.Role);
                response.UserName = oUser.UserName;
                response.IsSuccess = true;
            }
            else
            {
                response.UserName = string.Empty;
                response.IsSuccess = false;
                response.Errors = userResult.Errors.Select(p => p.Description).ToArray();
            }
            return Ok(response);
        }
        [HttpGet]
        [Route("all-users")]
        public async Task<ActionResult<ApplicationUser>> GetAllUsers()
        {
            List<ApplicationUser> lstUsers = new List<ApplicationUser>();
            lstUsers = await userManager.Users.ToListAsync();
            return Ok(lstUsers);
        }
        [HttpPost]
        public async Task<ActionResult<AuthResponse>> GetToken(AuthRequest authRequest)
        {
            AuthResponse response = new AuthResponse();
            var tokenString = await servicesBAL.GenerateAccessToken(authRequest.UserName);
            response.Token = tokenString;
            response.IsSuccess = true;
            response.RefreshToken = await servicesBAL.GetRefreshToken();
            return Ok(response);
        }
        [HttpPost]
        [Route("GetRefreshToken")]
        public async Task<ActionResult<AuthResponse>> GetRefreshToken(AuthRefreshRequest authRefreshRequest)
        {
            AuthResponse response = new AuthResponse();
            var res = await servicesBAL.GetPrincipalFromExpiredToken(authRefreshRequest.Token);

            var newAccessToken = await servicesBAL.GenerateAccessToken(res.Identity.Name);
            var newRefreshToken = await servicesBAL.GetRefreshToken();

            // Update the stored refresh token with the new one
            await servicesBAL.Update(res.Identity.Name, newRefreshToken);

            response.Token = newAccessToken;
            response.RefreshToken = newRefreshToken;

            return Ok(response);
        }
    }
}
