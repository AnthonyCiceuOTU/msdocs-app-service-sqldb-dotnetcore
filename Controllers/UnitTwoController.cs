using Microsoft.AspNetCore.Mvc;
using DotNetCoreSqlDb.ViewModels;
using System;

namespace DotNetCoreSqlDb.Controllers
{
    public class UnitTwoController : Controller
    {
        [HttpGet]
        public IActionResult Variables()
        {
            return View(new VariablesViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Variables(VariablesViewModel vm, string actionType)
        {
            vm.UserAnswer = vm.UserAnswer?.Trim() ?? "";
            vm.ExplanationAnswer = vm.ExplanationAnswer?.Trim() ?? "";

            if (actionType == "hint")
            {
                vm.ShowHint = true;
                vm.ShowSolution = false;
                vm.IsCorrect = null;
                vm.FeedbackMessage = "Hint: assign the number directly to the variable using the equals sign.";
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
                    vm.ExplanationAnswer.Contains("store", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("save", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("hold", StringComparison.OrdinalIgnoreCase);

                explanationCorrect = explanationCorrect &&
                    (
                        vm.ExplanationAnswer.Contains("value", StringComparison.OrdinalIgnoreCase) ||
                        vm.ExplanationAnswer.Contains("data", StringComparison.OrdinalIgnoreCase) ||
                        vm.ExplanationAnswer.Contains("information", StringComparison.OrdinalIgnoreCase)
                    );

                explanationCorrect = explanationCorrect && vm.ExplanationAnswer.Length >= 10;

                vm.ExplanationCorrect = explanationCorrect;
                vm.ExplanationFeedback = explanationCorrect
                    ? "Good explanation. Variables store values so a program can use them later."
                    : "Try mentioning that a variable stores or saves a value so the program can use it later.";

                return View(vm);
            }

            if (actionType == "submit")
            {
                vm.IsCorrect = true;
                vm.ExplanationCorrect = true;
                vm.FeedbackMessage = "Lesson complete!";
                return View(vm);
            }

            bool isCorrect =
                vm.UserAnswer.Equals("25", StringComparison.OrdinalIgnoreCase) ||
                vm.UserAnswer.Equals("points = 25", StringComparison.OrdinalIgnoreCase);

            vm.IsCorrect = isCorrect;
            vm.ShowHint = false;
            vm.ShowSolution = false;
            vm.FeedbackMessage = isCorrect
                ? "Correct! The variable points now stores the value 25."
                : "Not quite. Try assigning the value 25 to the variable.";

            return View(vm);
        }

        [HttpGet]
        public IActionResult DataTypes()
        {
            return View(new DataTypesViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DataTypes(DataTypesViewModel vm, string actionType)
        {
            vm.UserAnswer = vm.UserAnswer?.Trim() ?? "";

            if (actionType == "hint")
            {
                vm.ShowHint = true;
                vm.ShowSolution = false;
                vm.IsCorrect = null;
                vm.FeedbackMessage = "Hint: text values should be inside quotation marks.";
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
                vm.UserAnswer.Equals("\"Alex\"", StringComparison.OrdinalIgnoreCase) ||
                vm.UserAnswer.Equals("Alex", StringComparison.OrdinalIgnoreCase);

            vm.IsCorrect = isCorrect;
            vm.ShowHint = false;
            vm.ShowSolution = false;
            vm.FeedbackMessage = isCorrect
                ? "Correct! Alex is stored as text."
                : "Not quite. Remember that text values are usually written in quotation marks.";

            return View(vm);
        }

        [HttpGet]
        public IActionResult ArithmeticExpressions()
        {
            return View(new ArithmeticExpressionsViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ArithmeticExpressions(ArithmeticExpressionsViewModel vm, string actionType)
        {
            vm.UserAnswer = vm.UserAnswer?.Trim() ?? "";

            if (actionType == "hint")
            {
                vm.ShowHint = true;
                vm.ShowSolution = false;
                vm.IsCorrect = null;
                vm.FeedbackMessage = "Hint: use an arithmetic expression with 8 and 4.";
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

            bool isCorrect = vm.UserAnswer.Replace(" ", "") == "8+4";

            vm.IsCorrect = isCorrect;
            vm.ShowHint = false;
            vm.ShowSolution = false;
            vm.FeedbackMessage = isCorrect
                ? "Correct! The expression 8 + 4 calculates the total."
                : "Not quite. Try writing the expression using 8 plus 4.";

            return View(vm);
        }

        [HttpGet]
        public IActionResult InputOutput()
        {
            return View(new InputOutputViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult InputOutput(InputOutputViewModel vm, string actionType)
        {
            vm.UserAnswer = vm.UserAnswer?.Trim() ?? "";

            if (actionType == "hint")
            {
                vm.ShowHint = true;
                vm.ShowSolution = false;
                vm.IsCorrect = null;
                vm.FeedbackMessage = "Hint: the same variable should be input and then displayed.";
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

            bool isCorrect = vm.UserAnswer.Equals("age", StringComparison.OrdinalIgnoreCase);

            vm.IsCorrect = isCorrect;
            vm.ShowHint = false;
            vm.ShowSolution = false;
            vm.FeedbackMessage = isCorrect
                ? "Correct! The program inputs age and then displays it."
                : "Not quite. Try using the variable name age.";

            return View(vm);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction(nameof(Variables));
        }
    }
}