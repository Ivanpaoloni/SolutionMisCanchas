using Microsoft.AspNetCore.Identity;

namespace MisCanchas.Models
{
    public class UserViewModel
    {
        public string Email { get; set; }
        public bool IsAdmin { get; set; }

    }
}
