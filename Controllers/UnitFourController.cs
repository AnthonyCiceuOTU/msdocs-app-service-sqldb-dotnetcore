using Microsoft.AspNetCore.Mvc;
using DotNetCoreSqlDb.ViewModels;
using System;

namespace DotNetCoreSqlDb.Controllers
{
    public class UnitFourController : Controller
    {
        [HttpGet]
        public IActionResult WhyLoops()
        {
            return View(new WhyLoopsViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult WhyLoops(WhyLoopsViewModel vm, string actionType)
        {
            TrimAll(vm);

            if (actionType == "hint")
            {
                vm.ShowHint = true;
                vm.ShowSolution = false;
                ViewBag.ForceStep = 1;
                return View(vm);
            }

            if (actionType == "solution")
            {
                vm.ShowHint = false;
                vm.ShowSolution = true;
                ViewBag.ForceStep = 1;
                return View(vm);
            }

            if (actionType == "checkExplanation")
            {
                vm.ExplanationCorrect =
                    vm.ExplanationAnswer.Contains("repeat", StringComparison.OrdinalIgnoreCase) &&
                    (
                        vm.ExplanationAnswer.Contains("same code", StringComparison.OrdinalIgnoreCase) ||
                        vm.ExplanationAnswer.Contains("again", StringComparison.OrdinalIgnoreCase) ||
                        vm.ExplanationAnswer.Contains("multiple times", StringComparison.OrdinalIgnoreCase)
                    );

                vm.ExplanationFeedback = vm.ExplanationCorrect == true
                    ? "Correct! Loops are useful because they repeat code without rewriting it."
                    : "Try mentioning repeating code or doing the same task multiple times.";

                ViewBag.ForceStep = 2;
                return View(vm);
            }

            if (actionType == "submit")
            {
                if (vm.ExplanationCorrect != true)
                {
                    vm.ExplanationFeedback = string.IsNullOrWhiteSpace(vm.ExplanationFeedback)
                        ? "Please check your explanation before submitting."
                        : vm.ExplanationFeedback;

                    ViewBag.ForceStep = 2;
                    return View(vm);
                }

                vm.ExplanationFeedback = "Lesson complete!";
                ViewBag.ForceStep = 2;
                return View(vm);
            }

            vm.ShowHint = false;
            vm.ShowSolution = false;

            vm.IsQ1Correct = vm.UserAnswer1.Equals("repeat code", StringComparison.OrdinalIgnoreCase)
                             || vm.UserAnswer1.Contains("repeat", StringComparison.OrdinalIgnoreCase);
            vm.Feedback1 = vm.IsQ1Correct == true
                ? "Correct!"
                : "Think about what loops help us do again and again.";

            vm.IsQ2Correct = vm.UserAnswer2.Equals("less code", StringComparison.OrdinalIgnoreCase)
                             || vm.UserAnswer2.Contains("less", StringComparison.OrdinalIgnoreCase)
                             || vm.UserAnswer2.Contains("shorter", StringComparison.OrdinalIgnoreCase);
            vm.Feedback2 = vm.IsQ2Correct == true
                ? "Correct!"
                : "Loops help reduce repeated code.";

            vm.IsQ3Correct = vm.UserAnswer3.Equals("loop", StringComparison.OrdinalIgnoreCase);
            vm.Feedback3 = vm.IsQ3Correct == true
                ? "Correct!"
                : "The missing word is loop.";

            vm.IsQ4Correct = vm.UserAnswer4.Equals("repeat a task", StringComparison.OrdinalIgnoreCase)
                             || vm.UserAnswer4.Contains("repeat", StringComparison.OrdinalIgnoreCase);
            vm.Feedback4 = vm.IsQ4Correct == true
                ? "Correct!"
                : "Think about what a loop does.";

            ViewBag.ForceStep = AllCorrect(vm) ? 2 : 1;
            return View(vm);
        }

        [HttpGet]
        public IActionResult WhileLoops()
        {
            return View(new WhileLoopsViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult WhileLoops(WhileLoopsViewModel vm, string actionType)
        {
            TrimAll(vm);

            if (actionType == "hint")
            {
                vm.ShowHint = true;
                vm.ShowSolution = false;
                ViewBag.ForceStep = 1;
                return View(vm);
            }

            if (actionType == "solution")
            {
                vm.ShowHint = false;
                vm.ShowSolution = true;
                ViewBag.ForceStep = 1;
                return View(vm);
            }

            if (actionType == "checkExplanation")
            {
                vm.ExplanationCorrect =
                    vm.ExplanationAnswer.Contains("condition", StringComparison.OrdinalIgnoreCase) &&
                    vm.ExplanationAnswer.Contains("true", StringComparison.OrdinalIgnoreCase);

                vm.ExplanationFeedback = vm.ExplanationCorrect == true
                    ? "Correct! A WHILE loop repeats while a condition is true."
                    : "Try mentioning that it repeats while a condition stays true.";

                ViewBag.ForceStep = 2;
                return View(vm);
            }

            if (actionType == "submit")
            {
                if (vm.ExplanationCorrect != true)
                {
                    vm.ExplanationFeedback = string.IsNullOrWhiteSpace(vm.ExplanationFeedback)
                        ? "Please check your explanation before submitting."
                        : vm.ExplanationFeedback;

                    ViewBag.ForceStep = 2;
                    return View(vm);
                }

                vm.ExplanationFeedback = "Lesson complete!";
                ViewBag.ForceStep = 2;
                return View(vm);
            }

            vm.ShowHint = false;
            vm.ShowSolution = false;

            vm.IsQ1Correct = vm.UserAnswer1.Contains("score < 100", StringComparison.OrdinalIgnoreCase)
                             || (vm.UserAnswer1.Contains("<") && vm.UserAnswer1.Contains("100"));
            vm.Feedback1 = vm.IsQ1Correct == true
                ? "Correct!"
                : "Check the condition in the example.";

            vm.IsQ2Correct = vm.UserAnswer2.Equals("while the condition is true", StringComparison.OrdinalIgnoreCase)
                             || (vm.UserAnswer2.Contains("condition", StringComparison.OrdinalIgnoreCase)
                                 && vm.UserAnswer2.Contains("true", StringComparison.OrdinalIgnoreCase));
            vm.Feedback2 = vm.IsQ2Correct == true
                ? "Correct!"
                : "A WHILE loop continues as long as its condition is true.";

            vm.IsQ3Correct = vm.UserAnswer3.Equals("WHILE", StringComparison.OrdinalIgnoreCase);
            vm.Feedback3 = vm.IsQ3Correct == true
                ? "Correct!"
                : "The missing keyword is WHILE.";

            vm.IsQ4Correct = vm.UserAnswer4.Equals("condition becomes false", StringComparison.OrdinalIgnoreCase)
                             || vm.UserAnswer4.Contains("false", StringComparison.OrdinalIgnoreCase);
            vm.Feedback4 = vm.IsQ4Correct == true
                ? "Correct!"
                : "A WHILE loop stops when the condition becomes false.";

            ViewBag.ForceStep = AllCorrect(vm) ? 2 : 1;
            return View(vm);
        }

        [HttpGet]
        public IActionResult ForLoops()
        {
            return View(new ForLoopsViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ForLoops(ForLoopsViewModel vm, string actionType)
        {
            TrimAll(vm);

            if (actionType == "hint")
            {
                vm.ShowHint = true;
                vm.ShowSolution = false;
                ViewBag.ForceStep = 1;
                return View(vm);
            }

            if (actionType == "solution")
            {
                vm.ShowHint = false;
                vm.ShowSolution = true;
                ViewBag.ForceStep = 1;
                return View(vm);
            }

            if (actionType == "checkExplanation")
            {
                vm.ExplanationCorrect =
                    (
                        vm.ExplanationAnswer.Contains("known", StringComparison.OrdinalIgnoreCase) ||
                        vm.ExplanationAnswer.Contains("fixed", StringComparison.OrdinalIgnoreCase) ||
                        vm.ExplanationAnswer.Contains("set number", StringComparison.OrdinalIgnoreCase)
                    ) &&
                    vm.ExplanationAnswer.Contains("times", StringComparison.OrdinalIgnoreCase);

                vm.ExplanationFeedback = vm.ExplanationCorrect == true
                    ? "Correct! FOR loops are useful when you know how many times to repeat."
                    : "Try mentioning a known or fixed number of repetitions.";

                ViewBag.ForceStep = 2;
                return View(vm);
            }

            if (actionType == "submit")
            {
                if (vm.ExplanationCorrect != true)
                {
                    vm.ExplanationFeedback = string.IsNullOrWhiteSpace(vm.ExplanationFeedback)
                        ? "Please check your explanation before submitting."
                        : vm.ExplanationFeedback;

                    ViewBag.ForceStep = 2;
                    return View(vm);
                }

                vm.ExplanationFeedback = "Lesson complete!";
                ViewBag.ForceStep = 2;
                return View(vm);
            }

            vm.ShowHint = false;
            vm.ShowSolution = false;

            vm.IsQ1Correct = vm.UserAnswer1.Contains("1 to 10", StringComparison.OrdinalIgnoreCase)
                             || (vm.UserAnswer1.Contains("1") && vm.UserAnswer1.Contains("10"));
            vm.Feedback1 = vm.IsQ1Correct == true
                ? "Correct!"
                : "Look at the range in the FOR loop.";

            vm.IsQ2Correct = vm.UserAnswer2.Equals("when the number of repetitions is known", StringComparison.OrdinalIgnoreCase)
                             || (vm.UserAnswer2.Contains("known", StringComparison.OrdinalIgnoreCase)
                                 && vm.UserAnswer2.Contains("times", StringComparison.OrdinalIgnoreCase));
            vm.Feedback2 = vm.IsQ2Correct == true
                ? "Correct!"
                : "FOR loops are used when you know how many times to repeat.";

            vm.IsQ3Correct = vm.UserAnswer3.Equals("FOR", StringComparison.OrdinalIgnoreCase);
            vm.Feedback3 = vm.IsQ3Correct == true
                ? "Correct!"
                : "The missing keyword is FOR.";

            vm.IsQ4Correct = vm.UserAnswer4.Equals("5", StringComparison.OrdinalIgnoreCase)
                             || vm.UserAnswer4.Contains("5");
            vm.Feedback4 = vm.IsQ4Correct == true
                ? "Correct!"
                : "FOR i = 1 TO 5 repeats 5 times.";

            ViewBag.ForceStep = AllCorrect(vm) ? 2 : 1;
            return View(vm);
        }

        [HttpGet]
        public IActionResult Counters()
        {
            return View(new CountersViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Counters(CountersViewModel vm, string actionType)
        {
            TrimAll(vm);

            if (actionType == "hint")
            {
                vm.ShowHint = true;
                vm.ShowSolution = false;
                ViewBag.ForceStep = 1;
                return View(vm);
            }

            if (actionType == "solution")
            {
                vm.ShowHint = false;
                vm.ShowSolution = true;
                ViewBag.ForceStep = 1;
                return View(vm);
            }

            if (actionType == "checkExplanation")
            {
                vm.ExplanationCorrect =
                    (
                        vm.ExplanationAnswer.Contains("running total", StringComparison.OrdinalIgnoreCase) ||
                        vm.ExplanationAnswer.Contains("keeps adding", StringComparison.OrdinalIgnoreCase) ||
                        vm.ExplanationAnswer.Contains("adds values", StringComparison.OrdinalIgnoreCase)
                    );

                vm.ExplanationFeedback = vm.ExplanationCorrect == true
                    ? "Correct! An accumulator stores a running total."
                    : "Try mentioning that it keeps adding values or stores a running total.";

                ViewBag.ForceStep = 2;
                return View(vm);
            }

            if (actionType == "submit")
            {
                if (vm.ExplanationCorrect != true)
                {
                    vm.ExplanationFeedback = string.IsNullOrWhiteSpace(vm.ExplanationFeedback)
                        ? "Please check your explanation before submitting."
                        : vm.ExplanationFeedback;

                    ViewBag.ForceStep = 2;
                    return View(vm);
                }

                vm.ExplanationFeedback = "Lesson complete!";
                ViewBag.ForceStep = 2;
                return View(vm);
            }

            vm.ShowHint = false;
            vm.ShowSolution = false;

            vm.IsQ1Correct = vm.UserAnswer1.Equals("sum", StringComparison.OrdinalIgnoreCase);
            vm.Feedback1 = vm.IsQ1Correct == true
                ? "Correct!"
                : "Which variable is storing the total?";

            vm.IsQ2Correct = vm.UserAnswer2.Equals("running total", StringComparison.OrdinalIgnoreCase)
                             || vm.UserAnswer2.Contains("total", StringComparison.OrdinalIgnoreCase);
            vm.Feedback2 = vm.IsQ2Correct == true
                ? "Correct!"
                : "An accumulator keeps a running total.";

            vm.IsQ3Correct = vm.UserAnswer3.Equals("sum = sum + i", StringComparison.OrdinalIgnoreCase)
                             || vm.UserAnswer3.Contains("sum", StringComparison.OrdinalIgnoreCase);
            vm.Feedback3 = vm.IsQ3Correct == true
                ? "Correct!"
                : "Look at the line that updates the total.";

            vm.IsQ4Correct = vm.UserAnswer4.Equals("accumulator", StringComparison.OrdinalIgnoreCase);
            vm.Feedback4 = vm.IsQ4Correct == true
                ? "Correct!"
                : "A variable that stores a running total is called an accumulator.";

            ViewBag.ForceStep = AllCorrect(vm) ? 2 : 1;
            return View(vm);
        }

        [HttpGet]
        public IActionResult LoopErrors()
        {
            return View(new LoopErrorsViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LoopErrors(LoopErrorsViewModel vm, string actionType)
        {
            TrimAll(vm);

            if (actionType == "hint")
            {
                vm.ShowHint = true;
                vm.ShowSolution = false;
                ViewBag.ForceStep = 1;
                return View(vm);
            }

            if (actionType == "solution")
            {
                vm.ShowHint = false;
                vm.ShowSolution = true;
                ViewBag.ForceStep = 1;
                return View(vm);
            }

            if (actionType == "checkExplanation")
            {
                vm.ExplanationCorrect =
                    (
                        vm.ExplanationAnswer.Contains("infinite", StringComparison.OrdinalIgnoreCase) ||
                        vm.ExplanationAnswer.Contains("never stops", StringComparison.OrdinalIgnoreCase)
                    ) &&
                    (
                        vm.ExplanationAnswer.Contains("off-by-one", StringComparison.OrdinalIgnoreCase) ||
                        vm.ExplanationAnswer.Contains("one too many", StringComparison.OrdinalIgnoreCase) ||
                        vm.ExplanationAnswer.Contains("one too few", StringComparison.OrdinalIgnoreCase)
                    );

                vm.ExplanationFeedback = vm.ExplanationCorrect == true
                    ? "Correct! Infinite loops and off-by-one errors are common loop mistakes."
                    : "Try mentioning both infinite loops and off-by-one errors.";

                ViewBag.ForceStep = 2;
                return View(vm);
            }

            if (actionType == "submit")
            {
                if (vm.ExplanationCorrect != true)
                {
                    vm.ExplanationFeedback = string.IsNullOrWhiteSpace(vm.ExplanationFeedback)
                        ? "Please check your explanation before submitting."
                        : vm.ExplanationFeedback;

                    ViewBag.ForceStep = 2;
                    return View(vm);
                }

                vm.ExplanationFeedback = "Lesson complete!";
                ViewBag.ForceStep = 2;
                return View(vm);
            }

            vm.ShowHint = false;
            vm.ShowSolution = false;

            vm.IsQ1Correct = vm.UserAnswer1.Contains("infinite", StringComparison.OrdinalIgnoreCase);
            vm.Feedback1 = vm.IsQ1Correct == true
                ? "Correct!"
                : "One common error is a loop that never stops.";

            vm.IsQ2Correct = vm.UserAnswer2.Contains("off-by-one", StringComparison.OrdinalIgnoreCase)
                             || vm.UserAnswer2.Contains("off by one", StringComparison.OrdinalIgnoreCase);
            vm.Feedback2 = vm.IsQ2Correct == true
                ? "Correct!"
                : "Another common error is off-by-one.";

            vm.IsQ3Correct = vm.UserAnswer3.Contains("never stops", StringComparison.OrdinalIgnoreCase)
                             || vm.UserAnswer3.Contains("runs forever", StringComparison.OrdinalIgnoreCase);
            vm.Feedback3 = vm.IsQ3Correct == true
                ? "Correct!"
                : "What happens in an infinite loop?";

            vm.IsQ4Correct = vm.UserAnswer4.Contains("one too many", StringComparison.OrdinalIgnoreCase)
                             || vm.UserAnswer4.Contains("one too few", StringComparison.OrdinalIgnoreCase)
                             || vm.UserAnswer4.Contains("incorrect number", StringComparison.OrdinalIgnoreCase);
            vm.Feedback4 = vm.IsQ4Correct == true
                ? "Correct!"
                : "Off-by-one means the loop counts one too many or one too few times.";

            ViewBag.ForceStep = AllCorrect(vm) ? 2 : 1;
            return View(vm);
        }

        private static void TrimAll(UnitFourLessonViewModel vm)
        {
            vm.UserAnswer1 = vm.UserAnswer1?.Trim() ?? "";
            vm.UserAnswer2 = vm.UserAnswer2?.Trim() ?? "";
            vm.UserAnswer3 = vm.UserAnswer3?.Trim() ?? "";
            vm.UserAnswer4 = vm.UserAnswer4?.Trim() ?? "";
            vm.ExplanationAnswer = vm.ExplanationAnswer?.Trim() ?? "";
            vm.ExplanationFeedback = vm.ExplanationFeedback?.Trim() ?? "";
        }

        private static bool AllCorrect(UnitFourLessonViewModel vm)
        {
            return vm.IsQ1Correct == true &&
                   vm.IsQ2Correct == true &&
                   vm.IsQ3Correct == true &&
                   vm.IsQ4Correct == true;
        }
    }
}