namespace DotNetCoreSqlDb.Models
{
    public class PseudocodeCourseViewModel
    {
        public string CourseTitle { get; set; } = "";
        public List<PseudocodeUnit> Units { get; set; } = new();
        public PseudocodeLesson SelectedLesson { get; set; } = new();
    }

    public class PseudocodeUnit
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public bool IsExpanded { get; set; }
        public List<PseudocodeLesson> Lessons { get; set; } = new();
    }

    public class PseudocodeLesson
    {
        public int Id { get; set; }
        public int UnitId { get; set; }
        public string Slug { get; set; } = "";
        public string Title { get; set; } = "";
        public string LessonType { get; set; } = "Lesson";
        public string Summary { get; set; } = "";
        public string ChallengeTitle { get; set; } = "";
        public string ChallengePrompt { get; set; } = "";
        public string StarterCode { get; set; } = "";
        public string ExpectedAnswer { get; set; } = "";
        public string Hint { get; set; } = "";
        public string Solution { get; set; } = "";
        public bool IsAvailable { get; set; } = true;
    }
}