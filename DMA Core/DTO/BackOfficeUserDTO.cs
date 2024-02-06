using System.ComponentModel.DataAnnotations;

namespace Core.DTO
{
    public class BackOfficeUserDTO
    {
        public string Username { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Role { get; set; }

        public string Id { get; set; }

        public bool IsAuthenticated { get; set; }  
    }
}
