namespace DotNetCoreSqlDb.ViewModels
{
    public class UnitFourLessonViewModel
    {
        public string UserAnswer1 { get; set; } = "";
        public string UserAnswer2 { get; set; } = "";
        public string UserAnswer3 { get; set; } = "";
        public string UserAnswer4 { get; set; } = "";

        public bool? IsQ1Correct { get; set; }
        public bool? IsQ2Correct { get; set; }
        public bool? IsQ3Correct { get; set; }
        public bool? IsQ4Correct { get; set; }

        public string Feedback1 { get; set; } = "";
        public string Feedback2 { get; set; } = "";
        public string Feedback3 { get; set; } = "";
        public string Feedback4 { get; set; } = "";

        public bool ShowHint { get; set; }
        public bool ShowSolution { get; set; }

        public string ExplanationAnswer { get; set; } = "";
        public bool? ExplanationCorrect { get; set; }
        public string ExplanationFeedback { get; set; } = "";

        // Compatibility properties for older Unit Four views
        public string UserAnswer
        {
            get => UserAnswer1;
            set => UserAnswer1 = value ?? "";
        }

        public bool? IsCorrect
        {
            get => IsQ1Correct;
            set => IsQ1Correct = value;
        }

        public string FeedbackMessage
        {
            get => Feedback1;
            set => Feedback1 = value ?? "";
        }
    }
}