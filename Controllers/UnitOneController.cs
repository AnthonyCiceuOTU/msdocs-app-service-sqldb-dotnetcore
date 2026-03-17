using Microsoft.AspNetCore.Mvc;
using DotNetCoreSqlDb.ViewModels;

namespace DotNetCoreSqlDb.Controllers
{
    public class UnitOneController : Controller
    {
        [HttpGet]
        public IActionResult Algorithms()
        {
            return View(new AlgorithmsViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Algorithms(AlgorithmsViewModel vm, string actionType)
        {
            vm.UserAnswer1 = vm.UserAnswer1?.Trim() ?? "";
            vm.UserAnswer2 = vm.UserAnswer2?.Trim() ?? "";

            if (actionType == "hint")
            {
                vm.ShowHint = true;
                vm.ShowSolution = false;
                vm.FeedbackMessage = "Hint: use the variables that store the two prices.";
                vm.IsCorrect = null;
                return View(vm);
            }

            if (actionType == "solution")
            {
                vm.ShowHint = false;
                vm.ShowSolution = true;
                vm.FeedbackMessage = "Solution shown below.";
                vm.IsCorrect = null;
                return View(vm);
            }

            bool isCorrect =
                (vm.UserAnswer1.Equals("price1", StringComparison.OrdinalIgnoreCase) &&
                 vm.UserAnswer2.Equals("price2", StringComparison.OrdinalIgnoreCase))
                ||
                (vm.UserAnswer1.Equals("price2", StringComparison.OrdinalIgnoreCase) &&
                 vm.UserAnswer2.Equals("price1", StringComparison.OrdinalIgnoreCase));

            vm.IsCorrect = isCorrect;
            vm.ShowHint = false;
            vm.ShowSolution = false;
            vm.FeedbackMessage = isCorrect
                ? "Correct! The algorithm adds the two item prices together."
                : "Not quite. Try using the two variables defined above.";

            return View(vm);
        }

        [HttpGet]
        public IActionResult Decomposition()
        {
            return View(new DecompositionViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Decomposition(DecompositionViewModel vm, string actionType)
        {
            vm.UserAnswer1 = vm.UserAnswer1?.Trim() ?? "";
            vm.UserAnswer2 = vm.UserAnswer2?.Trim() ?? "";
            vm.UserAnswer3 = vm.UserAnswer3?.Trim() ?? "";

            if (actionType == "hint")
            {
                vm.ShowHint = true;
                vm.ShowSolution = false;
                vm.FeedbackMessage = "Hint: think of the small steps you do before leaving home for school.";
                vm.IsCorrect = null;
                return View(vm);
            }

            if (actionType == "solution")
            {
                vm.ShowHint = false;
                vm.ShowSolution = true;
                vm.FeedbackMessage = "Solution shown below.";
                vm.IsCorrect = null;
                return View(vm);
            }

            bool step1Correct = vm.UserAnswer1.Equals("wake up", StringComparison.OrdinalIgnoreCase);
            bool step2Correct = vm.UserAnswer2.Equals("get dressed", StringComparison.OrdinalIgnoreCase);
            bool step3Correct = vm.UserAnswer3.Equals("eat breakfast", StringComparison.OrdinalIgnoreCase);

            bool isCorrect = step1Correct && step2Correct && step3Correct;

            vm.IsCorrect = isCorrect;
            vm.ShowHint = false;
            vm.ShowSolution = false;
            vm.FeedbackMessage = isCorrect
                ? "Correct! You broke the morning routine into smaller steps."
                : "Not quite. Try thinking of three actions that happen before leaving for school.";

            return View(vm);
        }

        [HttpGet]
        public IActionResult WhatIsComputerScience()
        {
            return View(new ComputerScienceViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult WhatIsComputerScience(ComputerScienceViewModel vm, string actionType)
        {
            vm.UserAnswer1 = vm.UserAnswer1?.Trim() ?? "";
            vm.UserAnswer2 = vm.UserAnswer2?.Trim() ?? "";
            vm.UserAnswer3 = vm.UserAnswer3?.Trim() ?? "";

            if (actionType == "hint")
            {
                vm.ShowHint = true;
                vm.ShowSolution = false;
                vm.IsCorrect = null;
                vm.FeedbackMessage = "Hint: think about the steps you follow when brushing your teeth.";
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

            bool step1 = vm.UserAnswer1.Equals("put toothpaste on toothbrush", StringComparison.OrdinalIgnoreCase);
            bool step2 = vm.UserAnswer2.Equals("brush teeth", StringComparison.OrdinalIgnoreCase);
            bool step3 = vm.UserAnswer3.Equals("rinse mouth", StringComparison.OrdinalIgnoreCase);

            bool isCorrect = step1 && step2 && step3;

            vm.IsCorrect = isCorrect;
            vm.ShowHint = false;
            vm.ShowSolution = false;

            vm.FeedbackMessage = isCorrect
                ? "Correct! You wrote the steps of brushing your teeth as an algorithm."
                : "Not quite. Think about the main steps involved when brushing your teeth.";

            return View(vm);
        }
    }
}