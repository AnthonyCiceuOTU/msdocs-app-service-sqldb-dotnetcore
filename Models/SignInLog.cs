using System;
using System.ComponentModel.DataAnnotations;

namespace DotNetCoreSqlDb.Models
{
    public class SignInLog
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid? UserId { get; set; }
        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        public DateTime DateTime { get; set; }
        
    }
}