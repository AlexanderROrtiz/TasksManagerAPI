using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Application.DTOs
{
    public class UpdateUserDto
    {
        [StringLength(50)]
        public string Username { get; set; }

        [StringLength(50)]
        [EmailAddress(ErrorMessage = "Email Invalido")]
        public string Email { get; set; }

        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password debe contener minimo 8 caracteres.")]
        public string Password { get; set; } 

        public int? RoleId { get; set; }
    }
}
