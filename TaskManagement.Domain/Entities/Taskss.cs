using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Domain.Entities
{
    public class Taskss
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }  // Pendiente, En Proceso, Completada
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Relación con User
        public int? AssignedToUserId { get; set; }  // Relación con usuario
        public User AssignedToUser { get; set; }     // Usuario asignado a la tarea
    }
}
