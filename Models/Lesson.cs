using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetCoreSqlDb.Models
{
    public class Lesson
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UnitId { get; set; }

        [ForeignKey(nameof(UnitId))]
        public virtual Unit Unit { get; set; } = null!;

        [Required]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        // no longer used for actual lesson rendering
        public string? Content { get; set; }

        public int SortOrder { get; set; }

        public bool IsPublished { get; set; } = true;

        [Required]
        [StringLength(100)]
        public string ControllerName { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string ActionName { get; set; } = null!;

        public virtual ICollection<UserLessonProgress> UserProgress { get; set; } = new List<UserLessonProgress>();
    }
}