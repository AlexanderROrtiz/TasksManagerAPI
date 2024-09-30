namespace TaskManagement.Application.DTOs.TasksDTOs
{
    public class CreateTaskDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; } = "Pendiente";  // Inicializa en Pendiente
        public int? AssignedToUserId { get; set; }
    }
}
