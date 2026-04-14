namespace DotNetCoreSqlDb.ViewModels
{
public class DataProcessingViewModel
{
    public string Q1Answer { get; set; }
    public string Q2Answer { get; set; }
    public string Q3Answer { get; set; }

    public int CurrentStep { get; set; } = 1;

    public bool ShowHint { get; set; }
    public bool ShowSolution { get; set; }

    public bool? IsCorrect { get; set; }
    public string FeedbackMessage { get; set; }
}
}
