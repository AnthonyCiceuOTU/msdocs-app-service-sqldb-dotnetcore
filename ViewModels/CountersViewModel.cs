namespace DotNetCoreSqlDb.ViewModels
{
    public class CountersViewModel
    {
        public string UserAnswer { get; set; }

        public bool ShowHint { get; set; }
        public bool ShowSolution { get; set; }

        public bool? IsCorrect { get; set; }

        public string FeedbackMessage { get; set; }
    }
}