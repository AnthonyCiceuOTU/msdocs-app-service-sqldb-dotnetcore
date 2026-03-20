using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetCoreSqlDb.Models
{
    public class UserLessonProgress
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        [Required]
        public int LessonId { get; set; }

        [ForeignKey(nameof(LessonId))]
        public virtual Lesson Lesson { get; set; } = null!;

        public bool IsCompleted { get; set; }

        public DateTime? CompletedAtUtc { get; set; }

        public DateTime LastAccessedAtUtc { get; set; } = DateTime.UtcNow;
    }
}