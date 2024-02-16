using Customer_API.Models;
using Customer_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Customer_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderServiceBAL orderServiceBAL;

        public OrdersController(IOrderServiceBAL orderServiceBAL)
        {
            this.orderServiceBAL = orderServiceBAL;
        }
        [HttpGet]
        [Route("getOrdersByUserName/{userName}")]
        public async Task<ActionResult<IEnumerable<OrderViewModel>>> GetOrdersByUserName(string userName)
        {
            var orders = await orderServiceBAL.GetOrdersByUserName(userName);
            return Ok(orders);
        }
    }
}
