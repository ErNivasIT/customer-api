namespace Customer_API.Models
{
    public class AuthResponse
    {
        public string RequestId { get; set; }=Guid.NewGuid().ToString();
        public bool IsSuccess { get; set; } = true;
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
