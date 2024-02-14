using System.ComponentModel.DataAnnotations;

namespace Customer_API.Models
{
    public class AuthRequest
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string GrantType { get; set; }
    }
}
