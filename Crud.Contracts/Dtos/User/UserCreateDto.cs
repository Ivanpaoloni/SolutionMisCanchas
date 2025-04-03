
namespace MisCanchas.Contracts.Dtos.User
{
    public class UserCreateDto
    {
        public required string Email { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}
