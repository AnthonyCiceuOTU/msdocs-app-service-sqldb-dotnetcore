using Microsoft.AspNetCore.Mvc;
using Unit3DecisionMaking.ViewModels;

namespace Unit3DecisionMaking.Controllers
{
    public class UnitThreeController : Controller
    {

        // Lesson 11 — Boolean Logic

        [HttpGet]
        public IActionResult BooleanLogic()
        {
            return View(new BooleanLogicViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BooleanLogic(BooleanLogicViewModel vm, string actionType)
        {
            vm.UserAnswer = vm.UserAnswer?.Trim() ?? "";

            if (actionType == "hint")
            {
                vm.ShowHint = true;
                vm.ShowSolution = false;
                vm.IsCorrect = null;
                vm.FeedbackMessage = "Hint: compare the age to 18 using a comparison operator.";
                return View(vm);
            }

            if (actionType == "solution")
            {
                vm.ShowHint = false;
                vm.ShowSolution = true;
                vm.IsCorrect = null;
                vm.FeedbackMessage = "Solution: age >= 18";
                return View(vm);
            }

            bool isCorrect = vm.UserAnswer.Equals("age >= 18", StringComparison.OrdinalIgnoreCase);

            vm.IsCorrect = isCorrect;
            vm.ShowHint = false;
            vm.ShowSolution = false;
            vm.FeedbackMessage = isCorrect
                ? "Correct! This expression evaluates to True or False."
                : "Not quite. Try comparing age to 18.";

            return View(vm);
        }


        // Lesson 12 — IF Statements

        [HttpGet]
        public IActionResult IfStatements()
        {
            return View(new IfStatementViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult IfStatements(IfStatementViewModel vm, string actionType)
        {
            vm.UserAnswer = vm.UserAnswer?.Trim() ?? "";

            if (actionType == "hint")
            {
                vm.ShowHint = true;
                vm.ShowSolution = false;
                vm.IsCorrect = null;
                vm.FeedbackMessage = "Hint: check if temperature is less than 0.";
                return View(vm);
            }

            if (actionType == "solution")
            {
                vm.ShowHint = false;
                vm.ShowSolution = true;
                vm.IsCorrect = null;
                vm.FeedbackMessage = "Solution: IF temperature < 0 THEN PRINT \"Freezing\"";
                return View(vm);
            }

            bool isCorrect = vm.UserAnswer.Contains("< 0") &&
                             vm.UserAnswer.ToLower().Contains("freezing");

            vm.IsCorrect = isCorrect;
            vm.ShowHint = false;
            vm.ShowSolution = false;
            vm.FeedbackMessage = isCorrect
                ? "Correct! The IF statement runs when the condition is true."
                : "Not quite. Check the condition and output.";

            return View(vm);
        }


        // Lesson 13 — IF–ELSE

        [HttpGet]
        public IActionResult IfElse()
        {
            return View(new IfElseViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult IfElse(IfElseViewModel vm, string actionType)
        {
            vm.UserAnswer = vm.UserAnswer?.Trim() ?? "";

            if (actionType == "hint")
            {
                vm.ShowHint = true;
                vm.ShowSolution = false;
                vm.IsCorrect = null;
                vm.FeedbackMessage = "Hint: you need two outcomes: Pass and Fail.";
                return View(vm);
            }

            if (actionType == "solution")
            {
                vm.ShowHint = false;
                vm.ShowSolution = true;
                vm.IsCorrect = null;
                vm.FeedbackMessage = "Solution: IF grade >= 50 THEN Pass ELSE Fail";
                return View(vm);
            }

            bool isCorrect = vm.UserAnswer.Contains(">= 50") &&
                             vm.UserAnswer.ToLower().Contains("pass") &&
                             vm.UserAnswer.ToLower().Contains("fail");

            vm.IsCorrect = isCorrect;
            vm.ShowHint = false;
            vm.ShowSolution = false;
            vm.FeedbackMessage = isCorrect
                ? "Correct! IF–ELSE allows two possible outcomes."
                : "Not quite. Make sure both Pass and Fail are included.";

            return View(vm);
        }


        // Lesson 14 — Nested Conditions

        [HttpGet]
        public IActionResult NestedConditions()
        {
            return View(new NestedConditionsViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NestedConditions(NestedConditionsViewModel vm, string actionType)
        {
            vm.UserAnswer = vm.UserAnswer?.Trim() ?? "";

            if (actionType == "hint")
            {
                vm.ShowHint = true;
                vm.ShowSolution = false;
                vm.IsCorrect = null;
                vm.FeedbackMessage = "Hint: think of multiple grade ranges like 90+, 75+, etc.";
                return View(vm);
            }

            if (actionType == "solution")
            {
                vm.ShowHint = false;
                vm.ShowSolution = true;
                vm.IsCorrect = null;
                vm.FeedbackMessage = "Solution: Use multiple IF/ELSE IF conditions for grade ranges.";
                return View(vm);
            }

            bool isCorrect = vm.UserAnswer.Contains("90") &&
                             vm.UserAnswer.Contains("75") &&
                             vm.UserAnswer.Contains("50");

            vm.IsCorrect = isCorrect;
            vm.ShowHint = false;
            vm.ShowSolution = false;
            vm.FeedbackMessage = isCorrect
                ? "Correct! Nested or chained conditions handle multiple cases."
                : "Not quite. Include multiple grade thresholds.";

            return View(vm);
        }


        // Lesson 15 — Logical Operators

        [HttpGet]
        public IActionResult LogicalOperators()
        {
            return View(new LogicalOperatorsViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LogicalOperators(LogicalOperatorsViewModel vm, string actionType)
        {
            vm.UserAnswer = vm.UserAnswer?.Trim() ?? "";

            if (actionType == "hint")
            {
                vm.ShowHint = true;
                vm.ShowSolution = false;
                vm.IsCorrect = null;
                vm.FeedbackMessage = "Hint: combine two conditions using AND.";
                return View(vm);
            }

            if (actionType == "solution")
            {
                vm.ShowHint = false;
                vm.ShowSolution = true;
                vm.IsCorrect = null;
                vm.FeedbackMessage = "Solution: age >= 18 AND citizen = true";
                return View(vm);
            }

            bool isCorrect = vm.UserAnswer.ToLower().Contains("age") &&
                             vm.UserAnswer.Contains("18") &&
                             vm.UserAnswer.ToLower().Contains("and");

            vm.IsCorrect = isCorrect;
            vm.ShowHint = false;
            vm.ShowSolution = false;
            vm.FeedbackMessage = isCorrect
                ? "Correct! Logical operators combine conditions."
                : "Not quite. Try using AND to combine conditions.";

            return View(vm);
        }
    }
}
