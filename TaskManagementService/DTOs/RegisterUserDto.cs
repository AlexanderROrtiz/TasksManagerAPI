using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Application.DTOs
{
    public class RegisterUserDto
    {
        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(50)]
        [EmailAddress(ErrorMessage = "Email registrado invalido")]
        public string Email { get; set; }

        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password debe contener minimo 8 caracteres.")]
        public string Password { get; set; }

        public int RoleId { get; set; }
    }
}
