using Microsoft.AspNetCore.Mvc;
using DotNetCoreSqlDb.ViewModels;
using System;

namespace DotNetCoreSqlDb.Controllers
{
    public class UnitFourController : Controller
    {
        // -----------------------------
        // Lesson 16 — Why Loops
        // -----------------------------
        [HttpGet]
        public IActionResult WhyLoops()
        {
            return View(new WhyLoopsViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult WhyLoops(WhyLoopsViewModel vm, string actionType)
        {
            vm.UserAnswer = vm.UserAnswer?.Trim() ?? "";

            if (actionType == "hint")
            {
                vm.ShowHint = true;
                vm.ShowSolution = false;
                vm.IsCorrect = null;
                vm.FeedbackMessage = "Hint: loops help repeat actions without rewriting code many times.";
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

            bool isCorrect = vm.UserAnswer.Contains("repeat", StringComparison.OrdinalIgnoreCase);

            vm.IsCorrect = isCorrect;
            vm.ShowHint = false;
            vm.ShowSolution = false;

            vm.FeedbackMessage = isCorrect
                ? "Correct! Loops are used to repeat tasks efficiently."
                : "Not quite. Think about why repeating code manually is inefficient.";

            return View(vm);
        }

        // -----------------------------
        // Lesson 17 — WHILE Loops
        // -----------------------------
        [HttpGet]
        public IActionResult WhileLoops()
        {
            return View(new WhileLoopsViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult WhileLoops(WhileLoopsViewModel vm, string actionType)
        {
            vm.UserAnswer = vm.UserAnswer?.Trim() ?? "";

            if (actionType == "hint")
            {
                vm.ShowHint = true;
                vm.ShowSolution = false;
                vm.IsCorrect = null;
                vm.FeedbackMessage = "Hint: the loop continues while a condition is true.";
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

            bool isCorrect =
                vm.UserAnswer.Contains("while", StringComparison.OrdinalIgnoreCase) &&
                vm.UserAnswer.Contains("<", StringComparison.OrdinalIgnoreCase);

            vm.IsCorrect = isCorrect;
            vm.ShowHint = false;
            vm.ShowSolution = false;

            vm.FeedbackMessage = isCorrect
                ? "Correct! A WHILE loop runs as long as the condition is true."
                : "Not quite. Make sure your answer includes the loop condition.";

            return View(vm);
        }

        // -----------------------------
        // Lesson 18 — FOR Loops
        // -----------------------------
        [HttpGet]
        public IActionResult ForLoops()
        {
            return View(new ForLoopsViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ForLoops(ForLoopsViewModel vm, string actionType)
        {
            vm.UserAnswer = vm.UserAnswer?.Trim() ?? "";

            if (actionType == "hint")
            {
                vm.ShowHint = true;
                vm.ShowSolution = false;
                vm.IsCorrect = null;
                vm.FeedbackMessage = "Hint: FOR loops are used when the number of repetitions is known.";
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

            bool isCorrect =
                vm.UserAnswer.Contains("for", StringComparison.OrdinalIgnoreCase) &&
                vm.UserAnswer.Contains("1") &&
                vm.UserAnswer.Contains("10");

            vm.IsCorrect = isCorrect;
            vm.ShowHint = false;
            vm.ShowSolution = false;

            vm.FeedbackMessage = isCorrect
                ? "Correct! FOR loops iterate a set number of times."
                : "Not quite. Think about looping from 1 to 10.";

            return View(vm);
        }

        // -----------------------------
        // Lesson 19 — Counters & Accumulators
        // -----------------------------
        [HttpGet]
        public IActionResult Counters()
        {
            return View(new CountersViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Counters(CountersViewModel vm, string actionType)
        {
            vm.UserAnswer = vm.UserAnswer?.Trim() ?? "";

            if (actionType == "hint")
            {
                vm.ShowHint = true;
                vm.ShowSolution = false;
                vm.IsCorrect = null;
                vm.FeedbackMessage = "Hint: an accumulator adds values over time.";
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

            bool isCorrect = vm.UserAnswer.Contains("sum", StringComparison.OrdinalIgnoreCase);

            vm.IsCorrect = isCorrect;
            vm.ShowHint = false;
            vm.ShowSolution = false;

            vm.FeedbackMessage = isCorrect
                ? "Correct! An accumulator like 'sum' stores a running total."
                : "Not quite. Think about a variable that keeps adding values.";

            return View(vm);
        }

        // -----------------------------
        // Lesson 20 — Loop Errors
        // -----------------------------
        [HttpGet]
        public IActionResult LoopErrors()
        {
            return View(new LoopErrorsViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LoopErrors(LoopErrorsViewModel vm, string actionType)
        {
            vm.UserAnswer = vm.UserAnswer?.Trim() ?? "";

            if (actionType == "hint")
            {
                vm.ShowHint = true;
                vm.ShowSolution = false;
                vm.IsCorrect = null;
                vm.FeedbackMessage = "Hint: think about loops that never stop or go one step too far.";
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

            bool mentionsInfinite = vm.UserAnswer.Contains("infinite", StringComparison.OrdinalIgnoreCase);
            bool mentionsOffByOne = vm.UserAnswer.Contains("off", StringComparison.OrdinalIgnoreCase);

            bool isCorrect = mentionsInfinite && mentionsOffByOne;

            vm.IsCorrect = isCorrect;
            vm.ShowHint = false;
            vm.ShowSolution = false;

            vm.FeedbackMessage = isCorrect
                ? "Correct! Infinite loops and off-by-one errors are common loop mistakes."
                : "Not quite. Try naming two common loop errors.";

            return View(vm);
        }
    }
}