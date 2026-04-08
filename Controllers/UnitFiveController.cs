using Microsoft.AspNetCore.Mvc;
using DotNetCoreSqlDb.ViewModels;
using System;

namespace DotNetCoreSqlDb.Controllers
{
    public class UnitFiveController : Controller
    {
        // -----------------------------
        // Lesson 21 — Lists / Arrays
        // -----------------------------
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

            bool isCorrect = vm.UserAnswer.Contains("multiple", StringComparison.OrdinalIgnoreCase);

            vm.IsCorrect = isCorrect;
            vm.ShowHint = false;
            vm.ShowSolution = false;
            vm.FeedbackMessage = isCorrect
                ? "Correct! Lists store multiple values."
                : "Not quite. Think about what lists allow you to store.";

            return View(vm);
        }

        // -----------------------------
        // Lesson 22 — Accessing Elements
        // -----------------------------
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

            bool isCorrect = vm.UserAnswer.Contains("[0]");

            vm.IsCorrect = isCorrect;
            vm.ShowHint = false;
            vm.ShowSolution = false;
            vm.FeedbackMessage = isCorrect
                ? "Correct! Indexing starts at 0."
                : "Not quite. Remember the first index is 0.";

            return View(vm);
        }

        // -----------------------------
        // Lesson 23 — Looping Lists
        // -----------------------------
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

            bool isCorrect = vm.UserAnswer.Contains("for", StringComparison.OrdinalIgnoreCase);

            vm.IsCorrect = isCorrect;
            vm.ShowHint = false;
            vm.ShowSolution = false;
            vm.FeedbackMessage = isCorrect
                ? "Correct! Loops allow you to process each item in a list."
                : "Not quite. Think about which loop works best here.";

            return View(vm);
        }

        // -----------------------------
        // Lesson 24 — Searching Lists
        // -----------------------------
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

        // Navigation page
        public IActionResult Index()
        {
            return View();
        }
    }
}