using Customer_API.Models;
using Customer_API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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

        public AuthController(ILogger<AuthController> logger, IConfiguration configuration, IServicesBAL servicesBAL)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.servicesBAL = servicesBAL;
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

            var newAccessToken =await servicesBAL.GenerateAccessToken(res.Identity.Name);
            var newRefreshToken = await servicesBAL.GetRefreshToken();

            // Update the stored refresh token with the new one
            await servicesBAL.Update(res.Identity.Name, newRefreshToken);

            response.Token = newAccessToken;
            response.RefreshToken = newRefreshToken;

            return Ok(response);
        }
    }
}
