namespace DotNetCoreSqlDb.ViewModels
{
    public class ArithmeticExpressionsViewModel
    {
        public string UserAnswer { get; set; } = "";
        public bool? IsCorrect { get; set; }
        public string FeedbackMessage { get; set; } = "";
        public bool ShowHint { get; set; }
        public bool ShowSolution { get; set; }
    }
}