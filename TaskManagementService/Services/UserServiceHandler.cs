using Castle.Core.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagement.Application.DTOs;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Domain.SeedWork;
using TaskManagement.Infrastructure.Data;

namespace TaskManagement.Application.Services
{
    public class UserServiceHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly JwtSettings _jwtSettings;
        private readonly AppDbContext _context;

        public UserServiceHandler(IUserRepository userRepository, IRoleRepository roleRepository, IOptions<JwtSettings> jwtSettings, AppDbContext context)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _jwtSettings = jwtSettings.Value;
            _context = context;
        }
        public async Task<UserResponseDto> RegisterUserAsync(RegisterUserDto userDto)
        {
            var user = new User
            {
                Username = userDto.Username,
                Password = HashPassword(userDto.Password),
                Email = userDto.Email,
                RoleId = userDto.RoleId
            };

            var existingRole = await _roleRepository.GetByIdAsync(user.RoleId);
            if (existingRole == null)
            {
                throw new ArgumentException("Rol inválido.");
            }

            var existingUser = await _userRepository.GetByUsernameAsync(user.Username);
            if (existingUser != null)
            {
                throw new Exception("El usuario ya existe.");
            }

            var createdUser = await _userRepository.AddUserAsync(user);

            return MapUserToDto(createdUser, existingRole);
        }
        public async Task<UserResponseDto> UpdateUserAsync(int id, UpdateUserDto userDto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new Exception("El usuario no existe.");
            }

            user.Username = userDto.Username;
            user.Email = userDto.Email;

            // Solo actualiza la contraseña si se proporciona una nueva
            if (!string.IsNullOrEmpty(userDto.Password))
            {
                user.Password = HashPassword(userDto.Password);
            }

            if (userDto.RoleId.HasValue)
            {
                user.RoleId = userDto.RoleId.Value;
            }

            await _userRepository.UpdateUserAsync(user);

            var updatedRole = await _roleRepository.GetByIdAsync(user.RoleId);
            return MapUserToDto(user, updatedRole);
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new Exception("El usuario no existe.");
            }

            await _userRepository.DeleteUserAsync(id);
        }

        public async Task<UserResponseDto> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new Exception("El usuario no existe.");
            }
            var role = await _roleRepository.GetByIdAsync(user.RoleId);

            return MapUserToDto(user, role);
        }
        public async Task<List<UserResponseDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return users.Select(u => new UserResponseDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                Role = new RoleResponseDto
                {
                    Id = u.Role.Id,
                    Name = u.Role.Name
                }
            }).ToList();
        }
        public async Task<UserResponseDto> LoginAsync(string username, string password)
        {
            var authenticatedUser = await AuthenticateUserAsync(username, password);

            var responseDto = new UserResponseDto
            {
                Id = authenticatedUser.Id,
                Username = authenticatedUser.Username,
                Email = authenticatedUser.Email,
                Role = authenticatedUser.Role != null
                    ? new RoleResponseDto
                    {
                        Id = authenticatedUser.Role.Id,
                        Name = authenticatedUser.Role.Name
                    }
                    : null
            };

            return responseDto;
        }

        public async Task<User> AuthenticateUserAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Usuario o contraseña vacios");

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null || !VerifyPassword(password, user.Password))
            {
                throw new Exception("Usuario o contraseña invalido");
            }

            return user;
        }
        public string GenerateJwtToken(UserResponseDto user)
        {
            if (user == null || string.IsNullOrEmpty(user.Id.ToString()))
            {
                throw new ArgumentException("ID de usuario invalido.");
            }

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username), 
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), 
                new Claim("UserId", user.Id.ToString()), 
                new Claim(ClaimTypes.Name, user.Username) 
            };

            // Agregar rol si existe
            if (user.Role != null && !string.IsNullOrEmpty(user.Role.Name))
            {
                claims.Add(new Claim(ClaimTypes.Role, user.Role.Name)); 
            }

            // Generación de la clave
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Construcción del token
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer, 
                audience: _jwtSettings.Audience, 
                claims: claims,
                expires: DateTime.Now.AddMinutes(30), 
                signingCredentials: creds); 

            // Retorna el token JWT
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        private string HashPassword(string password)
        {
            // Método que genera el hash usando BCrypt
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string password, string hash)
        {
            // Método que verifica el hash de la contraseña
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
        private UserResponseDto MapUserToDto(User user, Role updatedRole)
        {
            return new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = updatedRole != null
                    ? new RoleResponseDto
                    {
                        Id = updatedRole.Id,
                        Name = updatedRole.Name
                    } : null
            };
        }
    }
}
