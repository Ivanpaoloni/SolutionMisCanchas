using Microsoft.AspNetCore.Identity;

namespace MisCanchas.Models
{
    public class UserViewModel
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public bool IsAdmin { get; set; }
        public string? Role { get; set; }

    }
}
