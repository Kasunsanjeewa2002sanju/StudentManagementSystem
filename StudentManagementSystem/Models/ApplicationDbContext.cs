using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Models;
using System.Collections.Generic;

namespace StudentManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; } = null!;
    }
}
