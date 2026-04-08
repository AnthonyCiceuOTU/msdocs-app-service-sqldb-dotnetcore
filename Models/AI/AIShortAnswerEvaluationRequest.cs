namespace DotNetCoreSqlDb.Models.AI
{
    public class ShortAnswerEvaluationRequest
    {
        public string QuestionText { get; set; } = "";
        public string StudentAnswer { get; set; } = "";
        public string ExpectedAnswer { get; set; } = "";
        public string GradingRubric { get; set; } = "";
    }
}