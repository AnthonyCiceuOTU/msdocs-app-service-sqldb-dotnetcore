namespace DotNetCoreSqlDb.ViewModels
{
    public class DataProcessingViewModel
    {

        public string Q1Answer { get; set; }
        public string Q2Answer { get; set; }
        public string Q3Answer { get; set; }


        public bool? IsQ1Correct { get; set; }
        public bool? IsQ2Correct { get; set; }
        public bool? IsQ3Correct { get; set; }

        public string Feedback1 { get; set; }
        public string Feedback2 { get; set; }
        public string Feedback3 { get; set; }


        public string ExplanationAnswer { get; set; }
        public bool? ExplanationCorrect { get; set; }
        public string ExplanationFeedback { get; set; }


        public bool ShowHint { get; set; }
        public bool ShowSolution { get; set; }
    }
}
