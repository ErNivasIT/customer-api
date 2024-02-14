using Customer_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Customer_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomersController : ControllerBase
    {
        [Route("getCustomers")]
        [HttpGet]
        public async Task<ActionResult<CustomerViewModel>> GetCustomers()
        {
            CustomerViewModel customerViewModel = new CustomerViewModel { Id = 100, DOB = DateTime.Now.AddYears(-30), Name = "Shree Nivas Kushwah" };
            await Task.CompletedTask;
            return Ok(customerViewModel);
        }
    }
}
