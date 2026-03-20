using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetCoreSqlDb.Models
{
    public class User
    {
        [Key]
        public Guid ID { get; set; }

        [DisplayName("Username")]
        [Required]
        public required string Username { get; set; }

        [Required]
        public byte[] PasswordHash { get; set; } = null!;

        [Required]
        public byte[] PasswordSalt { get; set; } = null!;

    }
}