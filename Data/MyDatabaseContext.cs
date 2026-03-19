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
    }
}
