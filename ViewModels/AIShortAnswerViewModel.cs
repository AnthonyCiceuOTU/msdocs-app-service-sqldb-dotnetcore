using System.ComponentModel.DataAnnotations;

namespace DotNetCoreSqlDb.ViewModels
{
    public class AiShortAnswerViewModel
    {
        public string QuestionText { get; set; } = "";
        public string ExpectedAnswer { get; set; } = "";
        public string GradingRubric { get; set; } = "";

        [Required]
        [Display(Name = "Your Answer")]
        public string UserAnswer { get; set; } = "";

        public bool? IsCorrect { get; set; }
        public int? Score { get; set; }
        public string FeedbackMessage { get; set; } = "";
        public string ReasoningSummary { get; set; } = "";
    }
}