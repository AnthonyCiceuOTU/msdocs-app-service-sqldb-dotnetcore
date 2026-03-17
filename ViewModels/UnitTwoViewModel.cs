namespace DotNetCoreSqlDb.Models
{
    public class UnitTwoViewModel
    {
        public string UnitTitle { get; set; } = "";
        public string UnitDescription { get; set; } = "";
        public List<UnitTwoLessonViewModel> Lessons { get; set; } = new();
        public UnitTwoLessonViewModel SelectedLesson { get; set; } = new();
        public UnitTwoLessonViewModel? PreviousLesson { get; set; }
        public UnitTwoLessonViewModel? NextLesson { get; set; }
    }

    public class UnitTwoLessonViewModel
    {
        public int Id { get; set; }
        public string Slug { get; set; } = "";
        public string Title { get; set; } = "";
        public string LessonType { get; set; } = "Lesson";
        public int ExerciseCount { get; set; }
        public string ChallengeTitle { get; set; } = "";
        public string ChallengePrompt { get; set; } = "";
        public string StarterCode { get; set; } = "";
        public string ExpectedAnswer { get; set; } = "";
        public string Hint { get; set; } = "";
        public string Solution { get; set; } = "";
    }
}