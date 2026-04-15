using Microsoft.AspNetCore.Mvc;
using DotNetCoreSqlDb.ViewModels;

namespace DotNetCoreSqlDb.Controllers
{
    public class UnitFiveController : Controller
    {
        [HttpGet]
        public IActionResult Lists()
        {
            return View(new ListsViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Lists(ListsViewModel vm, string actionType)
        {
            Normalize(vm);

            if (HandleCommonActions(
                vm,
                actionType,
                "Hint: think about what a list stores.",
                "Solution: a list stores multiple related values in one variable.",
                IsListsQ1Correct,
                IsListsQ2Correct,
                "Correct! A list stores multiple related values in one variable.",
                "Not quite yet. A list is used to keep several related values together.",
                "Lesson complete!"))
            {
                return View(vm);
            }

            return View(vm);
        }

        [HttpGet]
        public IActionResult Accessing()
        {
            return View(new AccessingViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Accessing(AccessingViewModel vm, string actionType)
        {
            Normalize(vm);

            if (HandleCommonActions(
                vm,
                actionType,
                "Hint: the first item in a list is at index 0.",
                "Solution: use square brackets with index 0 to access the first item, like scores[0].",
                IsAccessingQ1Correct,
                IsAccessingQ2Correct,
                "Correct! Indexing starts at 0, so the first item is accessed with [0].",
                "Not quite yet. Remember that list indexing starts at 0.",
                "Lesson complete!"))
            {
                return View(vm);
            }

            return View(vm);
        }

        [HttpGet]
        public IActionResult Looping()
        {
            return View(new LoopingViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Looping(LoopingViewModel vm, string actionType)
        {
            Normalize(vm);

            if (HandleCommonActions(
                vm,
                actionType,
                "Hint: use a loop when you want to go through every item in a list.",
                "Solution: a FOR loop is commonly used to go through each item in a list one by one.",
                IsLoopingQ1Correct,
                IsLoopingQ2Correct,
                "Correct! A loop helps you go through each item in a list without repeating code.",
                "Not quite yet. Think about which structure repeats for every item in a list.",
                "Lesson complete!"))
            {
                return View(vm);
            }

            return View(vm);
        }

        [HttpGet]
        public IActionResult Searching()
        {
            return View(new SearchingViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Searching(SearchingViewModel vm, string actionType)
        {
            Normalize(vm);

            if (HandleCommonActions(
                vm,
                actionType,
                "Hint: searching usually means checking items one by one until you find a match.",
                "Solution: use a loop to check each item and an IF statement to see whether it matches the target value.",
                IsSearchingQ1Correct,
                IsSearchingQ2Correct,
                "Correct! Searching usually uses a loop and an IF check to find a target value.",
                "Not quite yet. Think about checking each item until one matches.",
                "Lesson complete!"))
            {
                return View(vm);
            }

            return View(vm);
        }

        public IActionResult Index()
        {
            return View();
        }

        private static void Normalize(dynamic vm)
        {
            vm.UserAnswer = vm.UserAnswer?.Trim() ?? string.Empty;
            vm.ExplanationAnswer = vm.ExplanationAnswer?.Trim() ?? string.Empty;
        }

        private bool HandleCommonActions(
            dynamic vm,
            string actionType,
            string hintMessage,
            string solutionMessage,
            Func<string, bool> firstQuestionChecker,
            Func<string, bool> secondQuestionChecker,
            string successMessage,
            string retryMessage,
            string completionMessage)
        {
            if (actionType == "hint")
            {
                vm.ShowHint = true;
                vm.ShowSolution = false;
                vm.FeedbackMessage = hintMessage;
                return true;
            }

            if (actionType == "solution")
            {
                vm.ShowHint = false;
                vm.ShowSolution = true;
                vm.FeedbackMessage = solutionMessage;
                return true;
            }

            bool firstCorrect = firstQuestionChecker(vm.UserAnswer);
            bool secondCorrect = secondQuestionChecker(vm.ExplanationAnswer);
            bool allCorrect = firstCorrect && secondCorrect;

            vm.IsCorrect = allCorrect;
            vm.ExplanationCorrect = secondCorrect;
            vm.ShowHint = false;
            vm.ShowSolution = false;
            vm.ExplanationFeedback = secondCorrect ? "Correct!" : "Try the second question again.";
            vm.FeedbackMessage = allCorrect ? successMessage : retryMessage;

            if (actionType == "submit")
            {
                vm.FeedbackMessage = allCorrect
                    ? completionMessage
                    : "Please answer both multiple-choice questions correctly before marking the lesson complete.";
            }

            return true;
        }

        private static bool IsListsQ1Correct(string answer) => answer == "B";
        private static bool IsListsQ2Correct(string answer) => answer == "A";

        private static bool IsAccessingQ1Correct(string answer) => answer == "C";
        private static bool IsAccessingQ2Correct(string answer) => answer == "B";

        private static bool IsLoopingQ1Correct(string answer) => answer == "A";
        private static bool IsLoopingQ2Correct(string answer) => answer == "B";

        private static bool IsSearchingQ1Correct(string answer) => answer == "C";
        private static bool IsSearchingQ2Correct(string answer) => answer == "A";
    }
}
