namespace DotNetCoreSqlDb.Models
{
    public class Unit3DecisionMakingViewModel
    {
        public string UnitTitle { get; set; } = "Unit 3 — Decision Making";
        public string UnitDescription { get; set; } = "Programs that can choose between alternatives.";

        public List<UnitThreeLessonViewModel> Lessons { get; set; } = new();
        public UnitThreeLessonViewModel SelectedLesson { get; set; } = new();

        public UnitThreeLessonViewModel? PreviousLesson { get; set; }
        public UnitThreeLessonViewModel? NextLesson { get; set; }
    }

    public class Unit3LessonViewModel
    {
        public int Id { get; set; }

        
        public string Slug { get; set; } = "";

        public string Title { get; set; } = "";

        //  Lesson, Practice, Quiz
        public string LessonType { get; set; } = "Lesson";

        public int ExerciseCount { get; set; }

        // Lesson Content
        public string TopicSummary { get; set; } = "";
        public string ExampleCode { get; set; } = "";
        public string Explanation { get; set; } = "";

        // Challenge Section
        public string ChallengeTitle { get; set; } = "";
        public string ChallengePrompt { get; set; } = "";
        public string StarterCode { get; set; } = "";
        public string ExpectedAnswer { get; set; } = "";

        // Learning Support
        public string Hint { get; set; } = "";
        public string Solution { get; set; } = "";
    }
}
