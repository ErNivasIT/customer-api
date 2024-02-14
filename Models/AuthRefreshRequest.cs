using System.ComponentModel.DataAnnotations;

namespace Customer_API.Models
{
    public class AuthRefreshRequest
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }
}
