namespace DotNetCoreSqlDb.ViewModels
{
    public class AlgorithmsViewModel
    {
        public string UserAnswer1 { get; set; } = "";
        public string UserAnswer2 { get; set; } = "";

        public bool? IsCorrect { get; set; }

        public string FeedbackMessage { get; set; } = "";

        public bool ShowHint { get; set; }
        public bool ShowSolution { get; set; }
    }
}