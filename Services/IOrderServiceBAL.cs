using Customer_API.Models;

namespace Customer_API.Services
{
    public interface IOrderServiceBAL
    {
        Task<IEnumerable<OrderViewModel>> GetOrdersByUserName(string userName);
    }
}
