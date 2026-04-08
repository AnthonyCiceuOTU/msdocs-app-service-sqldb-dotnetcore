namespace DotNetCoreSqlDb.Models.AI
{
    public class AiShortAnswerGradeResult
    {
        public bool IsCorrect { get; set; }
        public int Score { get; set; } // 0-100
        public string Feedback { get; set; } = "";
        public string ReasoningSummary { get; set; } = "";
    }
}