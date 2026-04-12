using Microsoft.AspNetCore.Mvc;
using DotNetCoreSqlDb.ViewModels;
using System;

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
            vm.UserAnswer = vm.UserAnswer?.Trim() ?? "";
            vm.ExplanationAnswer = vm.ExplanationAnswer?.Trim() ?? "";

            if (actionType == "hint")
            {
                vm.ShowHint = true;
                vm.ShowSolution = false;
                vm.IsCorrect = null;
                vm.FeedbackMessage = "Hint: lists store multiple values in one variable.";
                return View(vm);
            }

            if (actionType == "solution")
            {
                vm.ShowHint = false;
                vm.ShowSolution = true;
                vm.IsCorrect = null;
                vm.FeedbackMessage = "Solution shown below.";
                return View(vm);
            }

            if (actionType == "checkExplanation")
            {
                vm.IsCorrect = true;

                bool explanationCorrect =
                    vm.ExplanationAnswer.Contains("multiple", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("many", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("items", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("values", StringComparison.OrdinalIgnoreCase);

                explanationCorrect = explanationCorrect && vm.ExplanationAnswer.Length >= 10;

                vm.ExplanationCorrect = explanationCorrect;
                vm.ExplanationFeedback = explanationCorrect
                    ? "Good explanation. Lists are useful because they store multiple values together."
                    : "Add a bit more detail about how lists help store multiple values in one place.";

                return View(vm);
            }

            if (actionType == "submit")
            {
                vm.IsCorrect = true;
                vm.ExplanationCorrect = true;
                vm.FeedbackMessage = "Lesson complete!";
                return View(vm);
            }

            bool isCorrect = vm.UserAnswer.Contains("multiple", StringComparison.OrdinalIgnoreCase);

            vm.IsCorrect = isCorrect;
            vm.ShowHint = false;
            vm.ShowSolution = false;
            vm.FeedbackMessage = isCorrect
                ? "Correct! Lists store multiple values."
                : "Not quite. Think about what lists allow you to store.";

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
            vm.UserAnswer = vm.UserAnswer?.Trim() ?? "";
            vm.ExplanationAnswer = vm.ExplanationAnswer?.Trim() ?? "";

            if (actionType == "hint")
            {
                vm.ShowHint = true;
                vm.ShowSolution = false;
                vm.IsCorrect = null;
                vm.FeedbackMessage = "Hint: use square brackets with an index.";
                return View(vm);
            }

            if (actionType == "solution")
            {
                vm.ShowHint = false;
                vm.ShowSolution = true;
                vm.IsCorrect = null;
                vm.FeedbackMessage = "Solution shown below.";
                return View(vm);
            }

            if (actionType == "checkExplanation")
            {
                vm.IsCorrect = true;

                bool explanationCorrect =
                    vm.ExplanationAnswer.Contains("index", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("position", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("0", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("first", StringComparison.OrdinalIgnoreCase);

                explanationCorrect = explanationCorrect && vm.ExplanationAnswer.Length >= 10;

                vm.ExplanationCorrect = explanationCorrect;
                vm.ExplanationFeedback = explanationCorrect
                    ? "Good explanation. You explained how indexing helps access an item."
                    : "Mention that list items are accessed by index, and that the first index is 0.";

                return View(vm);
            }

            if (actionType == "submit")
            {
                vm.IsCorrect = true;
                vm.ExplanationCorrect = true;
                vm.FeedbackMessage = "Lesson complete!";
                return View(vm);
            }

            bool isCorrect = vm.UserAnswer.Contains("[0]");

            vm.IsCorrect = isCorrect;
            vm.ShowHint = false;
            vm.ShowSolution = false;
            vm.FeedbackMessage = isCorrect
                ? "Correct! Indexing starts at 0."
                : "Not quite. Remember the first index is 0.";

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
            vm.UserAnswer = vm.UserAnswer?.Trim() ?? "";
            vm.ExplanationAnswer = vm.ExplanationAnswer?.Trim() ?? "";

            if (actionType == "hint")
            {
                vm.ShowHint = true;
                vm.ShowSolution = false;
                vm.IsCorrect = null;
                vm.FeedbackMessage = "Hint: use a loop to go through each item.";
                return View(vm);
            }

            if (actionType == "solution")
            {
                vm.ShowHint = false;
                vm.ShowSolution = true;
                vm.IsCorrect = null;
                vm.FeedbackMessage = "Solution shown below.";
                return View(vm);
            }

            if (actionType == "checkExplanation")
            {
                vm.IsCorrect = true;

                bool explanationCorrect =
                    vm.ExplanationAnswer.Contains("each", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("every", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("item", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("repeat", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("repetition", StringComparison.OrdinalIgnoreCase);

                explanationCorrect = explanationCorrect && vm.ExplanationAnswer.Length >= 10;

                vm.ExplanationCorrect = explanationCorrect;
                vm.ExplanationFeedback = explanationCorrect
                    ? "Good explanation. Loops help process each item in a list without repetition."
                    : "Add a little more detail about how loops repeat through each item in the list.";

                return View(vm);
            }

            if (actionType == "submit")
            {
                vm.IsCorrect = true;
                vm.ExplanationCorrect = true;
                vm.FeedbackMessage = "Lesson complete!";
                return View(vm);
            }

            bool isCorrect = vm.UserAnswer.Contains("for", StringComparison.OrdinalIgnoreCase);

            vm.IsCorrect = isCorrect;
            vm.ShowHint = false;
            vm.ShowSolution = false;
            vm.FeedbackMessage = isCorrect
                ? "Correct! Loops allow you to process each item in a list."
                : "Not quite. Think about which loop works best here.";

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
            vm.UserAnswer = vm.UserAnswer?.Trim() ?? "";
            vm.ExplanationAnswer = vm.ExplanationAnswer?.Trim() ?? "";

            if (actionType == "hint")
            {
                vm.ShowHint = true;
                vm.ShowSolution = false;
                vm.IsCorrect = null;
                vm.FeedbackMessage = "Hint: check each item one by one.";
                return View(vm);
            }

            if (actionType == "solution")
            {
                vm.ShowHint = false;
                vm.ShowSolution = true;
                vm.IsCorrect = null;
                vm.FeedbackMessage = "Solution shown below.";
                return View(vm);
            }

            if (actionType == "checkExplanation")
            {
                vm.IsCorrect = true;

                bool explanationCorrect =
                    vm.ExplanationAnswer.Contains("each", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("item", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("one by one", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("find", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("search", StringComparison.OrdinalIgnoreCase);

                explanationCorrect = explanationCorrect && vm.ExplanationAnswer.Length >= 10;

                vm.ExplanationCorrect = explanationCorrect;
                vm.ExplanationFeedback = explanationCorrect
                    ? "Good explanation. Searching checks items until the target is found."
                    : "Explain a bit more clearly that searching checks items one by one until it finds the target.";

                return View(vm);
            }

            if (actionType == "submit")
            {
                vm.IsCorrect = true;
                vm.ExplanationCorrect = true;
                vm.FeedbackMessage = "Lesson complete!";
                return View(vm);
            }

            bool hasLoop = vm.UserAnswer.Contains("for", StringComparison.OrdinalIgnoreCase);
            bool hasIf = vm.UserAnswer.Contains("if", StringComparison.OrdinalIgnoreCase);
            bool isCorrect = hasLoop && hasIf;

            vm.IsCorrect = isCorrect;
            vm.ShowHint = false;
            vm.ShowSolution = false;
            vm.FeedbackMessage = isCorrect
                ? "Correct! Linear search checks each item."
                : "Not quite. You need both a loop and a condition.";

            return View(vm);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}