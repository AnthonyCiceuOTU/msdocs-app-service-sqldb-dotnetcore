namespace DotNetCoreSqlDb.ViewModels
{
    public class ComputerScienceViewModel
    {
        public string PbSandwichOrder { get; set; } = "";
        public string CardSortOrder { get; set; } = "";

        public bool RecipeSelected { get; set; }
        public bool GroceryListSelected { get; set; }
        public bool GpsSelected { get; set; }
        public bool PhotoSelected { get; set; }

        public string RecipeExplanation { get; set; } = "";
        public string GroceryListExplanation { get; set; } = "";
        public string GpsExplanation { get; set; } = "";
        public string PhotoExplanation { get; set; } = "";

        public bool? PbCorrect { get; set; }
        public bool? CardCorrect { get; set; }
        public bool? IdentifyCorrect { get; set; }

        public string PbFeedback { get; set; } = "";
        public string CardFeedback { get; set; } = "";
        public string IdentifyFeedback { get; set; } = "";

        public string FeedbackMessage { get; set; } = "";
        public bool? IsCorrect { get; set; }

        public bool ShowHint { get; set; }
        public bool ShowSolution { get; set; }

        public int CurrentStep { get; set; } = 0;
    }
}