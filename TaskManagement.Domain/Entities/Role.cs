using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Domain.Entities
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }    // Administrador, Supervisor, Empleado

        public ICollection<User> Users { get; set; }
    }
}
