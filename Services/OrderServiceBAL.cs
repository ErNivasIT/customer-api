using Customer_API.Models;

namespace Customer_API.Services
{
    public class OrderServiceBAL : IOrderServiceBAL
    {
        static List<OrderViewModel> lst=new List<OrderViewModel>(); 
        static OrderServiceBAL()
        {
            for (int i = 0; i < 50; i++)
            {
                lst.Add(new OrderViewModel() {  Id=i+10000, Order_Date=DateTime.Now.AddDays(-i), Amount=1500+i, Order_Placed_By="shri.mca2010@gmail.com", Qty=(i%10)+1 });
            }
            for (int i = 0; i < 50; i++)
            {
                lst.Add(new OrderViewModel() { Id = i + 20000, Order_Date = DateTime.Now.AddDays(-i), Amount = 2500 + i, Order_Placed_By = "shreenivas.kushwah@rws.com", Qty = (i % 10) + 1 });
            }
        }
        public async Task<IEnumerable<OrderViewModel>> GetOrdersByUserName(string userName)
        {
            var res= lst.Where(p=>p.Order_Placed_By==userName).ToList();
            return res;
        }
    }
}
