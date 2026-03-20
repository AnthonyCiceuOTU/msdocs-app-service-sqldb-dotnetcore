using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DotNetCoreSqlDb.Models;

namespace DotNetCoreSqlDb.Data
{
    public class MyDatabaseContext : DbContext
    {
        public MyDatabaseContext (DbContextOptions<MyDatabaseContext> options) : base(options)
        {
        }

        public DbSet<DotNetCoreSqlDb.Models.Todo> Todo { get; set; } = default!;
        public DbSet<DotNetCoreSqlDb.Models.User> User { get; set; } = default!;
        public DbSet<DotNetCoreSqlDb.Models.SyntaxGameQuestions> SyntaxGameQuestions { get; set; } = default!;
        public DbSet<DotNetCoreSqlDb.Models.SignInLog> SignInLog { get; set; } = default!;
        public DbSet<Unit> Units { get; set; } = null!;
        public DbSet<Lesson> Lessons { get; set; } = null!;
        public DbSet<UserLessonProgress> UserLessonProgresses { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Unit>()
                .HasMany(u => u.Lessons)
                .WithOne(l => l.Unit)
                .HasForeignKey(l => l.UnitId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Lesson>()
                .HasMany(l => l.UserProgress)
                .WithOne(p => p.Lesson)
                .HasForeignKey(p => p.LessonId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserLessonProgress>()
                .HasIndex(p => new { p.UserId, p.LessonId })
                .IsUnique();
        }
    }

    
}