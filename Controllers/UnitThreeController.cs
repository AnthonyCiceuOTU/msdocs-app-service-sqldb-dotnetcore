using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DotNetCoreSqlDb.ViewModels;

namespace DotNetCoreSqlDb.Controllers
{
    [Authorize]
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
            vm.ExplanationAnswer = vm.ExplanationAnswer?.Trim() ?? "";

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

            if (actionType == "checkExplanation" || actionType == "submit")
            {
                bool explanationCorrect =
                    vm.ExplanationAnswer.Contains("true", StringComparison.OrdinalIgnoreCase) &&
                    vm.ExplanationAnswer.Contains("false", StringComparison.OrdinalIgnoreCase) &&
                    (vm.ExplanationAnswer.Contains("compare", StringComparison.OrdinalIgnoreCase)
                     || vm.ExplanationAnswer.Contains("comparison", StringComparison.OrdinalIgnoreCase)) &&
                    (vm.ExplanationAnswer.Contains("decision", StringComparison.OrdinalIgnoreCase)
                     || vm.ExplanationAnswer.Contains("decide", StringComparison.OrdinalIgnoreCase));

                vm.ExplanationCorrect = explanationCorrect;
                vm.ExplanationFeedback = explanationCorrect
                    ? "Correct! Boolean logic helps programs compare values and make decisions."
                    : "Try mentioning true/false, comparing values, and decision making.";

                return View(vm);
            }

            vm.ShowHint = false;
            vm.ShowSolution = false;

            vm.IsQ1Correct = vm.UserAnswer1.Equals("age >= 18", StringComparison.OrdinalIgnoreCase);
            vm.Feedback1 = vm.IsQ1Correct == true
                ? "Correct!"
                : "Remember to use >= for 'at least 18'.";

            vm.IsQ2Correct = vm.UserAnswer2 == "True";
            vm.Feedback2 = vm.IsQ2Correct == true
                ? "Correct! 75 is greater than 50."
                : "75 is greater than 50, so this is True.";

            vm.IsQ3Correct = vm.UserAnswer3 == "!=";
            vm.Feedback3 = vm.IsQ3Correct == true
                ? "Correct!"
                : "The 'not equal to' operator is !=";

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
            vm.UserAnswer1 = vm.UserAnswer1?.Trim() ?? "";
            vm.UserAnswer2 = vm.UserAnswer2?.Trim() ?? "";
            vm.UserAnswer3 = vm.UserAnswer3?.Trim() ?? "";
            vm.UserAnswer4 = vm.UserAnswer4?.Trim() ?? "";
            vm.ExplanationAnswer = vm.ExplanationAnswer?.Trim() ?? "";

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

            if (actionType == "checkExplanation" || actionType == "submit")
            {
                bool explanationCorrect =
                    (vm.ExplanationAnswer.Contains("condition", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("check", StringComparison.OrdinalIgnoreCase)) &&
                    (vm.ExplanationAnswer.Contains("true", StringComparison.OrdinalIgnoreCase)) &&
                    (vm.ExplanationAnswer.Contains("run", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("runs", StringComparison.OrdinalIgnoreCase)) &&
                    (vm.ExplanationAnswer.Contains("false", StringComparison.OrdinalIgnoreCase)) &&
                    (vm.ExplanationAnswer.Contains("skip", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("skipped", StringComparison.OrdinalIgnoreCase));

                vm.ExplanationCorrect = explanationCorrect;
                vm.ExplanationFeedback = explanationCorrect
                    ? "Correct! IF statements run code only when a condition is true."
                    : "Try mentioning a condition, true running code, and false skipping code.";

                return View(vm);
            }

            vm.ShowHint = false;
            vm.ShowSolution = false;

            vm.IsQ1Correct = vm.UserAnswer1.Contains("< 0");
            vm.Feedback1 = vm.IsQ1Correct == true
                ? "Correct!"
                : "You need to check if temperature is less than 0.";

            vm.IsQ2Correct = vm.UserAnswer2 == "Code is skipped";
            vm.Feedback2 = vm.IsQ2Correct == true
                ? "Correct! IF only runs when the condition is true."
                : "If the condition is false, the code inside the IF does not run.";

            vm.IsQ3Correct = vm.UserAnswer3.Contains(">=");
            vm.Feedback3 = vm.IsQ3Correct == true
                ? "Correct!"
                : "To pass at 50 or higher, use >=.";

            vm.IsQ4Correct = vm.UserAnswer4 == "Freezing prints";
            vm.Feedback4 = vm.IsQ4Correct == true
                ? "Correct! -5 is less than 0."
                : "Since -5 < 0, the IF condition is true.";

            return View(vm);
        }

        // Lesson 13 — IF-ELSE

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
            vm.ExplanationAnswer = vm.ExplanationAnswer?.Trim() ?? "";

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

            if (actionType == "checkExplanation" || actionType == "submit")
            {
                bool explanationCorrect =
                    vm.ExplanationAnswer.Contains("true", StringComparison.OrdinalIgnoreCase) &&
                    vm.ExplanationAnswer.Contains("false", StringComparison.OrdinalIgnoreCase) &&
                    (
                        vm.ExplanationAnswer.Contains("path", StringComparison.OrdinalIgnoreCase) ||
                        vm.ExplanationAnswer.Contains("outcome", StringComparison.OrdinalIgnoreCase) ||
                        vm.ExplanationAnswer.Contains("block", StringComparison.OrdinalIgnoreCase)
                    ) &&
                    (
                        vm.ExplanationAnswer.Contains("other", StringComparison.OrdinalIgnoreCase) ||
                        vm.ExplanationAnswer.Contains("else", StringComparison.OrdinalIgnoreCase) ||
                        vm.ExplanationAnswer.Contains("different", StringComparison.OrdinalIgnoreCase)
                    );

                vm.ExplanationCorrect = explanationCorrect;
                vm.ExplanationFeedback = explanationCorrect
                    ? "Correct! IF-ELSE lets a program choose one path when true and another when false."
                    : "Try mentioning true, false, and choosing between two different paths.";

                return View(vm);
            }

            vm.ShowHint = false;
            vm.ShowSolution = false;

            vm.IsQ1Correct = vm.UserAnswer1.Equals("grade >= 50", StringComparison.OrdinalIgnoreCase);
            vm.Feedback1 = vm.IsQ1Correct == true
                ? "Correct!"
                : "Use >= for 'at least 50'.";

            vm.IsQ2Correct = vm.UserAnswer2 == "ELSE runs";
            vm.Feedback2 = vm.IsQ2Correct == true
                ? "Correct!"
                : "When IF is false, ELSE runs.";

            vm.IsQ3Correct = vm.UserAnswer3.Equals("ELSE", StringComparison.OrdinalIgnoreCase);
            vm.Feedback3 = vm.IsQ3Correct == true
                ? "Correct!"
                : "The missing keyword is ELSE.";

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
            vm.ExplanationAnswer = vm.ExplanationAnswer?.Trim() ?? "";

            if (actionType == "hint")
            {
                vm.ShowHint = true;
                vm.ShowSolution = false;
                vm.IsCorrect = null;
                vm.FeedbackMessage = "Hint: think of multiple grade ranges like 90+, 75+, and 50+.";
                return View(vm);
            }

            if (actionType == "solution")
            {
                vm.ShowHint = false;
                vm.ShowSolution = true;
                vm.IsCorrect = null;
                vm.FeedbackMessage = "Solution: Use multiple IF or ELSE IF checks for grade ranges like 90, 75, and 50.";
                return View(vm);
            }

            if (actionType == "checkExplanation" || actionType == "submit")
            {
                bool explanationCorrect =
                    (
                        vm.ExplanationAnswer.Contains("multiple", StringComparison.OrdinalIgnoreCase) ||
                        vm.ExplanationAnswer.Contains("many", StringComparison.OrdinalIgnoreCase) ||
                        vm.ExplanationAnswer.Contains("several", StringComparison.OrdinalIgnoreCase)
                    ) &&
                    (
                        vm.ExplanationAnswer.Contains("condition", StringComparison.OrdinalIgnoreCase) ||
                        vm.ExplanationAnswer.Contains("check", StringComparison.OrdinalIgnoreCase)
                    ) &&
                    (
                        vm.ExplanationAnswer.Contains("different", StringComparison.OrdinalIgnoreCase) ||
                        vm.ExplanationAnswer.Contains("cases", StringComparison.OrdinalIgnoreCase) ||
                        vm.ExplanationAnswer.Contains("outcomes", StringComparison.OrdinalIgnoreCase)
                    );

                vm.ExplanationCorrect = explanationCorrect;
                vm.ExplanationFeedback = explanationCorrect
                    ? "Correct! Nested conditions help programs check multiple cases in order."
                    : "Try mentioning multiple conditions, ordered checks, and different cases.";

                return View(vm);
            }

            bool isCorrect =
                vm.UserAnswer.Contains("90") &&
                vm.UserAnswer.Contains("75") &&
                vm.UserAnswer.Contains("50");

            vm.IsCorrect = isCorrect;
            vm.ShowHint = false;
            vm.ShowSolution = false;
            vm.FeedbackMessage = isCorrect
                ? "Correct! Nested or chained conditions handle multiple cases."
                : "Not quite. Include multiple grade thresholds such as 90, 75, and 50.";

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
            vm.ExplanationAnswer = vm.ExplanationAnswer?.Trim() ?? "";

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

            if (actionType == "checkExplanation" || actionType == "submit")
            {
                bool explanationCorrect =
                    (
                        vm.ExplanationAnswer.Contains("combine", StringComparison.OrdinalIgnoreCase) ||
                        vm.ExplanationAnswer.Contains("combines", StringComparison.OrdinalIgnoreCase)
                    ) &&
                    (
                        vm.ExplanationAnswer.Contains("condition", StringComparison.OrdinalIgnoreCase) ||
                        vm.ExplanationAnswer.Contains("conditions", StringComparison.OrdinalIgnoreCase)
                    ) &&
                    (
                        vm.ExplanationAnswer.Contains("decision", StringComparison.OrdinalIgnoreCase) ||
                        vm.ExplanationAnswer.Contains("decisions", StringComparison.OrdinalIgnoreCase) ||
                        vm.ExplanationAnswer.Contains("precise", StringComparison.OrdinalIgnoreCase)
                    );

                vm.ExplanationCorrect = explanationCorrect;
                vm.ExplanationFeedback = explanationCorrect
                    ? "Correct! Logical operators combine conditions to make more precise decisions."
                    : "Try mentioning combining conditions and making better or more precise decisions.";

                return View(vm);
            }

            bool isCorrect =
                vm.UserAnswer.Contains("age", StringComparison.OrdinalIgnoreCase) &&
                vm.UserAnswer.Contains("18") &&
                vm.UserAnswer.Contains("and", StringComparison.OrdinalIgnoreCase) &&
                vm.UserAnswer.Contains("citizen", StringComparison.OrdinalIgnoreCase);

            vm.IsCorrect = isCorrect;
            vm.ShowHint = false;
            vm.ShowSolution = false;
            vm.FeedbackMessage = isCorrect
                ? "Correct! Logical operators combine conditions."
                : "Not quite. Try using AND to combine age >= 18 with citizen = true.";

            return View(vm);
        }
    }
}