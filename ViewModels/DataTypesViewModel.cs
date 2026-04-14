namespace DotNetCoreSqlDb.ViewModels
{
    public class DataTypesViewModel
    {
        public string UserAnswer { get; set; } = "";
        public bool? IsCorrect { get; set; }
        public string FeedbackMessage { get; set; } = "";

        public bool ShowHint { get; set; }
        public bool ShowSolution { get; set; }

        public string ExplanationAnswer { get; set; } = "";
        public bool? ExplanationCorrect { get; set; }
        public string ExplanationFeedback { get; set; } = "";

        public int CurrentStep { get; set; } = 0;
    }
}