namespace DotNetCoreSqlDb.ViewModels
{
    public class LessonLinkViewModel
    {
        public int LessonId { get; set; }
        public string Title { get; set; } = null!;
        public int SortOrder { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsCurrent { get; set; }
    }
}