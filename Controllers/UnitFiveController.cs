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
                "Solution: Question 1 = B) Storing multiple related values in one variable. Question 2 = A) A list can hold many items together.",
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
                "Hint: the first item in a list uses index 0.",
                "Solution: Question 1 = C) scores[0]. Question 2 = B) Because list indexes start at 0.",
                IsAccessingQ1Correct,
                IsAccessingQ2Correct,
                "Correct! The first item in a list is usually accessed with index 0.",
                "Not quite yet. Remember that most lists start indexing at 0.",
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
                "Hint: think about what helps you repeat the same action for every item in a list.",
                "Solution: Question 1 = A) A loop. Question 2 = B) They let you process each item without repeating the same code.",
                IsLoopingQ1Correct,
                IsLoopingQ2Correct,
                "Correct! Loops are useful because they let you go through list items one by one.",
                "Not quite yet. Think about what repeats code for each item in a list.",
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
                "Hint: searching means checking items until you find the one you want.",
                "Solution: Question 1 = C) Checking items to find a target value. Question 2 = A) Use a loop and compare each item with the target.",
                IsSearchingQ1Correct,
                IsSearchingQ2Correct,
                "Correct! Searching a list means checking items to find a target value.",
                "Not quite yet. Think about checking each item one by one until there is a match.",
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