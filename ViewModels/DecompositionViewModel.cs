namespace DotNetCoreSqlDb.ViewModels
{
    public class DecompositionViewModel
    {
        public string UserAnswer1 { get; set; } = "";
        public string UserAnswer2 { get; set; } = "";
        public string UserAnswer3 { get; set; } = "";

        public bool? IsCorrect { get; set; }

        public string FeedbackMessage { get; set; } = "";

        public bool ShowHint { get; set; }
        public bool ShowSolution { get; set; }
    }
}