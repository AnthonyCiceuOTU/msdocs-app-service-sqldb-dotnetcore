namespace Unit3DecisionMaking.ViewModels
{
public class IfElseViewModel : LessonBaseViewModel
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
}
}