using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagement.Application.DTOs.TasksDTOs;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Responses;

namespace TaskManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet("verify-token")]
        [Authorize]
        public IActionResult VerifyToken()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("El ID de usuario no se encuentra o es inválido.");
            }

            return Ok(new { UserId = userId, message = "token Correcto." });
        }

        // Solo el Administrador puede crear tareas
        [HttpPost("/CreateTask - Rol Administrador")]
        [Authorize(Roles = "Administrador")]

        public async Task<IActionResult> CreateTask([FromBody] TaskDto taskDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Los datos proporcionados son inválidos.", errors = ModelState });

            var result = await _taskService.CreateTaskAsync(taskDto);

            if (result == null)
            {
                return BadRequest(new { message = "Error al crear la tarea." });
            }

            return CreatedAtAction(nameof(GetTaskById), new { id = result.Id }, new { message = "La tarea se creó correctamente.", task = result });
        }

        // Administradores pueden obtener todas las tareas
        [HttpGet("/GetAllTasks - Rol Administrador")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> GetAllTasksAsync()
        {
            var tasks = await _taskService.GetAllTasksAsync();

            if (tasks == null || !tasks.Any())
            {
                return NotFound(new NotFoundResponse { Message = "No se encontraron tareas." });
            }

            return Ok(new TaskResponse
            {
                Message = "Tareas encontradas correctamente.",
                Tasks = tasks
            });
        }

        // Supervisores y Administradores pueden obtener las tareas
        [HttpGet("/GetTaskById - Rol Administrador y Supervisor")]
        [Authorize(Roles = "Supervisor,Administrador")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);

            if (task == null)
                return NotFound(new { message = $"La tarea con ID {id} no fue encontrada." });

            return Ok(new { message = "Tarea encontrada correctamente.", task });
        }

        // Los empleados solo pueden ver sus propias tareas
        [HttpGet("/GetUserTasks - Rol Empleado")]
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> GetUserTasks()
        {
            var userId = User.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("El ID de usuario no se encuentra o es inválido.");
            }

            if (!int.TryParse(userId, out int userIdParsed))
            {
                return BadRequest("El ID de usuario no es un número válido.");
            }

            var tasks = await _taskService.GetTasksByUserIdAsync(userIdParsed);

            if (tasks == null || tasks.Count == 0)
            {
                return Ok(new { message = "No se encontraron tareas para este usuario." });
            }

            return Ok(new { message = "Las tareas se trajeron correctamente.", data = tasks });
        }

        // El Supervisor puede asignar tareas a empleados
        [HttpPut("/AssignTask/{id} - Rol Supervisor")]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> AssignTask(int id, [FromBody] AssignTaskDto assignTaskDto)
        {
            var result = await _taskService.AssignTaskAsync(id, assignTaskDto);
            if (!result)
            {
                return BadRequest("Error al asignar la tarea");
            }

            return Ok(new { message = "La tarea se asignó correctamente." });
        }

        // El Administrador, Supervisor y Empleado pueden actualizar las tareas
        [HttpPut("/UpdateTaskStatus - Rol Administrador, Supervisor y Empleado")]
        [Authorize(Roles = "Administrador,Empleado,Supervisor")]
        public async Task<IActionResult> UpdateTaskStatus(int id, [FromBody] UpdateTaskStatusDto statusDto)
        {
            var userId = User.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("El ID de usuario no se encuentra o es inválido.");
            }

            var result = await _taskService.UpdateTaskStatusAsync(id, statusDto, int.Parse(userId));

            if (!result)
                return BadRequest("No se pudo actualizar el estado de la tarea");

            return Ok(new { message = "La tarea se actualizó correctamente." });
        }

        // Solo el Administrador puede eliminar tareas
        [HttpDelete("/DeleteTask - Rol Administrador")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var result = await _taskService.DeleteTaskAsync(id);
            if (!result)
                return BadRequest("No se pudo eliminar la tarea");

            return Ok(new { message = "La tarea se eliminó correctamente." });
        }
    }
}
