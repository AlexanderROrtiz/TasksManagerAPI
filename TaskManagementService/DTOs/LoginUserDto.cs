using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Application.DTOs
{
    public class LoginUserDto
    {
        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }
    }
}
