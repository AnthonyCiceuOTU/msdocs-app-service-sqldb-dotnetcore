namespace DotNetCoreSqlDb.ViewModels
{
    public class AlgorithmsViewModel
    {
        public string UserAnswer1 { get; set; } = "";
        public string UserAnswer2 { get; set; } = "";

        public bool? IsCorrect { get; set; }
        public string FeedbackMessage { get; set; } = "";

        public string ExplanationAnswer { get; set; } = "";
        public bool? ExplanationCorrect { get; set; }
        public string ExplanationFeedback { get; set; } = "";

        public bool ShowHint { get; set; }
        public bool ShowSolution { get; set; }

        public int CurrentStep { get; set; } = 0;
    }
}