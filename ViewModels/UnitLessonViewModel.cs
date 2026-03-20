namespace DotNetCoreSqlDb.ViewModels
{
    public class UnitLessonsViewModel
    {
        public int UnitId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int SortOrder { get; set; }

        public List<LessonLinkViewModel> Lessons { get; set; } = new();
    }
}