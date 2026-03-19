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
    vm.UserAnswer1 = vm.UserAnswer1?.Trim() ?? "";
    vm.UserAnswer2 = vm.UserAnswer2?.Trim() ?? "";
    vm.UserAnswer3 = vm.UserAnswer3?.Trim() ?? "";
    vm.UserAnswer4 = vm.UserAnswer4?.Trim() ?? "";

    if (actionType == "hint")
    {
        vm.ShowHint = true;
        vm.ShowSolution = false;
        return View(vm);
    }

    if (actionType == "solution")
    {
        vm.ShowHint = false;
        vm.ShowSolution = true;
        return View(vm);
    }

    // Question 1
    vm.IsQ1Correct = vm.UserAnswer1.Equals("age >= 18", StringComparison.OrdinalIgnoreCase);
    vm.Feedback1 = vm.IsQ1Correct == true
        ? "Correct!"
        : "Remember to use >= for 'at least 18'.";

    // Question 2
    vm.IsQ2Correct = vm.UserAnswer2 == "True";
    vm.Feedback2 = vm.IsQ2Correct == true
        ? "Correct! 75 is greater than 50."
        : "75 is greater than 50, so this is True.";

    // Question 3
    vm.IsQ3Correct = vm.UserAnswer3.Trim() == "!=";
    vm.Feedback3 = vm.IsQ3Correct == true
        ? "Correct!"
        : "The 'not equal to' operator is !=";

    // Question 4
    vm.IsQ4Correct = vm.UserAnswer4 == "False";
    vm.Feedback4 = vm.IsQ4Correct == true
        ? "Correct! 10 is not less than 5."
        : "10 is greater than 5, so this is False.";

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
    vm.UserAnswer1 = vm.UserAnswer1?.Trim() ?? "";
    vm.UserAnswer2 = vm.UserAnswer2?.Trim() ?? "";
    vm.UserAnswer3 = vm.UserAnswer3?.Trim() ?? "";
    vm.UserAnswer4 = vm.UserAnswer4?.Trim() ?? "";

    if (actionType == "hint")
    {
        vm.ShowHint = true;
        vm.ShowSolution = false;
        return View(vm);
    }

    if (actionType == "solution")
    {
        vm.ShowHint = false;
        vm.ShowSolution = true;
        return View(vm);
    }

    // Q1
    vm.IsQ1Correct = vm.UserAnswer1.Equals("grade >= 50", StringComparison.OrdinalIgnoreCase);
    vm.Feedback1 = vm.IsQ1Correct == true
        ? "Correct!"
        : "Use >= for 'at least 50'.";

    // Q2
    vm.IsQ2Correct = vm.UserAnswer2 == "ELSE runs";
    vm.Feedback2 = vm.IsQ2Correct == true
        ? "Correct!"
        : "When IF is false, ELSE runs.";

    // Q3
    vm.IsQ3Correct = vm.UserAnswer3.Equals("ELSE", StringComparison.OrdinalIgnoreCase);
    vm.Feedback3 = vm.IsQ3Correct == true
        ? "Correct!"
        : "The missing keyword is ELSE.";

    // Q4
    vm.IsQ4Correct = vm.UserAnswer4 == "Fail";
    vm.Feedback4 = vm.IsQ4Correct == true
        ? "Correct!"
        : "40 is less than 50, so it prints Fail.";

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
