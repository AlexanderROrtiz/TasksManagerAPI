namespace TaskManagement.Application.DTOs
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public RoleResponseDto Role { get; set; }
    }

    public class RoleResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
