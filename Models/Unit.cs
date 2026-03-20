using System.ComponentModel.DataAnnotations;

namespace DotNetCoreSqlDb.Models
{
    public class Unit
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public int SortOrder { get; set; }

        public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
    }
}