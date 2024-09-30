using TaskManagement.Application.DTOs.TasksDTOs;

namespace TaskManagement.Application.Responses
{
    public class TaskResponse
    {
        public string Message { get; set; }
        public List<TaskDto> Tasks { get; set; }
    }
}
