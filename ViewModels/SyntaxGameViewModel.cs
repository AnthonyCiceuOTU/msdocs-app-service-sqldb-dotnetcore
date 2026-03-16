using System.ComponentModel.DataAnnotations;

namespace DotNetCoreSqlDb.ViewModels
{
    public class SyntaxGameViewModel
    {
        public int QuestionNumber { get; set; }

        public string Pseudocode { get; set; } = "";

        [Required]
        public string SelectedLanguage { get; set; } = "Python";

        [Required]
        public string UserAnswer { get; set; } = "";

        public string? FeedbackMessage { get; set; }

        public bool IsCorrect { get; set; }
    }
}