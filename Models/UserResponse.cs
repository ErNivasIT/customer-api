namespace Customer_API.Models
{
    public class UserResponse
    {
        public string RequestId { get; set; } = Guid.NewGuid().ToString();
        public bool IsSuccess { get; set; } = true;
        public string UserName { get; set; }
        public string[] Errors { get; set; }
    }
}
