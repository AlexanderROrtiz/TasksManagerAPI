using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Taskss> Taskss { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id); 

            modelBuilder.Entity<Role>()
                .HasKey(r => r.Id);  

            modelBuilder.Entity<Taskss>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)  
                .WithMany(r => r.Users)  
                .HasForeignKey(u => u.RoleId)  
                .OnDelete(DeleteBehavior.Restrict);          

            // Inserta roles iniciales en la base de datos
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Administrador" },
                new Role { Id = 2, Name = "Supervisor" },
                new Role { Id = 3, Name = "Empleado" }
            );
            base.OnModelCreating(modelBuilder);
        }
    }
}
