namespace DotNetCoreSqlDb.ViewModels
{
    public class DesigningProgramViewModel
    {
        public string UserAnswer { get; set; }

        public bool ShowHint { get; set; }
        public bool ShowSolution { get; set; }

        public bool? IsCorrect { get; set; }
        public string FeedbackMessage { get; set; }

        public string ActionType { get; set; }
    }
}