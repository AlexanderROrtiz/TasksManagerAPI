namespace TaskManagement.Application.DTOs.TasksDTOs
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }  // Pendiente, En Proceso, Completada
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Información del usuario asignado
        public int? AssignedToUserId { get; set; }
        public string AssignedToUsername { get; set; }
    }
}
