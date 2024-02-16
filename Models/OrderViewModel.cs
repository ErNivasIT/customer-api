namespace Customer_API.Models
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public int Qty { get; set; }
        public int Amount { get; set; }
        public DateTime Order_Date { get; set; }
        public string Order_Placed_By{ get; set; }
    }
}
