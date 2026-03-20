namespace DotNetCoreSqlDb.ViewModels
{
    public class LessonsIndexViewModel
    {
        public List<UnitLessonsViewModel> Units { get; set; } = new();

        public int? CurrentLessonId { get; set; }
        public int? CurrentUnitId { get; set; }
        public string? CurrentUnitTitle { get; set; }
        public string? CurrentLessonTitle { get; set; }
        public string? CurrentLessonDescription { get; set; }
        public bool CurrentLessonCompleted { get; set; }
        public string? CurrentLessonContent { get; set; }

        public int? PreviousLessonId { get; set; }
        public int? NextLessonId { get; set; }
    }
}