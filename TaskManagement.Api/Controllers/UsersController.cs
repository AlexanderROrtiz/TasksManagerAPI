using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Services;

namespace TaskManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserServiceHandler _userService;        

        public UsersController(UserServiceHandler userService)
        {
            _userService = userService;            
        }

        [HttpPost("/RegisterUser")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var responseDto = await _userService.RegisterUserAsync(userDto);

                return CreatedAtAction(nameof(Register), new { id = responseDto.Id }, responseDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Ha ocurrido un error al registrar al usuario." });
            }
        }

        [HttpPost("/loginUser")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var authenticatedUser = await _userService.LoginAsync(userDto.Username, userDto.Password);
                var token = _userService.GenerateJwtToken(authenticatedUser);

                return Ok(new
                {
                    message = "Inicio de sesión exitoso.",
                    user = authenticatedUser,
                    token
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return Unauthorized(new { message = "Usuario o contraseña invalido" });
            }
        }        

        [HttpPut("/UpdateUser")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedUser = await _userService.UpdateUserAsync(id, userDto);
                return Ok(new { message = "Usuario actualizado correctamente.", user = updatedUser });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "No se pudo actualizar el usuario. " + ex.Message });
            }
        }

        [HttpDelete("/DeleteUser")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
                return Ok(new { message = "Usuario eliminado correctamente." });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Usuario no encontrado." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "No se pudo eliminar el usuario. " + ex.Message });
            }
        }

        [HttpGet("/GetUserById")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                return Ok(user);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Usuario no encontrado." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "No se pudo obtener el usuario. " + ex.Message });
            }
        }

        [HttpGet("/GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ha ocurrido un error al obtener los usuarios." });
            }
        }
    }
}
