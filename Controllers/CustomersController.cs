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
        static List<CustomerViewModel> lst = new List<CustomerViewModel>();
        static CustomersController()
        {
            for (int i = 0; i < 1; i++)
            {
                lst.Add(new CustomerViewModel() { Id = i + 1, DOB = DateTime.Now.AddYears(-i), Name = "Customer " + i, Email="shri.mca2010@gmail.com" });
            }
            for (int i = 1; i < 2; i++)
            {
                lst.Add(new CustomerViewModel() { Id = i + 1, DOB = DateTime.Now.AddYears(-i), Name = "Customer " + i, Email = "shreenivas.kushwah@rws.com" });
            }
        }
        [Route("getCustomers")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerViewModel>>> GetCustomers()
        {
            await Task.CompletedTask;
            return Ok(lst);
        }
        [Route("getCustomerById/{id}")]
        [HttpGet]
        public async Task<ActionResult<CustomerViewModel>> GetCustomerById(int id)
        {
            var cust = lst.Where(p => p.Id == id).SingleOrDefault();
            if (cust == null)
                return NotFound();
            else
                await Task.CompletedTask;
            return Ok(cust);
        }
        [Route("updateCustomerById/{id}")]
        [HttpPut]
        public async Task<ActionResult> updateCustomerById(int id, CustomerViewModel modal)
        {
            var cust = lst.Where(p => p.Id == id).SingleOrDefault();
            if (cust == null)
                return NotFound();
            else
            {
                cust.Name = modal.Name;
                cust.DOB = modal.DOB;
            }
            await Task.CompletedTask;
            return NoContent();
        }
        [Route("addCustomers")]
        [HttpPost]
        public async Task<ActionResult<string>> AddCustomers(CustomerViewModel model)
        {
            lst.Add(new CustomerViewModel() { Id = lst.Count + 1, DOB = model.DOB, Name = model.Name });
            await Task.CompletedTask;
            return Ok("Customer Added successfully !!!");
        }
    }
}
