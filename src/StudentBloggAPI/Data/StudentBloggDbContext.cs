using Microsoft.EntityFrameworkCore;
using StudentBloggAPI.Models.Entities;

namespace StudentBloggAPI.Data;

public class StudentBloggDbContext : DbContext
{
    public StudentBloggDbContext(DbContextOptions<StudentBloggDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Post> Post { get; set; }
    public DbSet<Comment> Comments { get; set; }
}
