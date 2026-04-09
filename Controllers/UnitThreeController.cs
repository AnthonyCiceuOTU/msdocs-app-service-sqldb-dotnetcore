using DotNetCoreSqlDb.Data;
using DotNetCoreSqlDb.Models;
using DotNetCoreSqlDb.ViewModels;
using DotNetCoreSqlDb.Services;
using DotNetCoreSqlDb.Models.AI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace DotNetCoreSqlDb.Controllers
{
    [Authorize]
    public class UnitThreeController : Controller
    {
        private readonly MyDatabaseContext _context;
        private readonly IAiShortAnswerGrader _aiShortAnswerGrader;
        private readonly ILogger<UnitThreeController> _logger;     

        public UnitThreeController(
            MyDatabaseContext context,
            IAiShortAnswerGrader aiShortAnswerGrader,
            ILogger<UnitThreeController> logger)
        {
            _context = context;
            _aiShortAnswerGrader = aiShortAnswerGrader;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult BooleanLogic()
        {
            return View(new BooleanLogicViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BooleanLogic(BooleanLogicViewModel vm, string actionType)
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

            if (actionType == "checkExplanation")
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

            if (actionType == "submit")
            {
                var saved = await SaveLessonProgressAsync("BooleanLogic");
                vm.ExplanationCorrect = true;
                vm.ExplanationFeedback = saved
                    ? "Lesson complete! Your progress has been saved."
                    : "Your answers were submitted, but progress could not be saved.";
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

        [HttpGet]
        public IActionResult IfStatements()
        {
            return View(new IfStatementViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IfStatements(IfStatementViewModel vm, string actionType)
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

            if (actionType == "checkExplanation")
            {
                bool explanationCorrect =
                    (vm.ExplanationAnswer.Contains("condition", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("check", StringComparison.OrdinalIgnoreCase)) &&
                    vm.ExplanationAnswer.Contains("true", StringComparison.OrdinalIgnoreCase) &&
                    (vm.ExplanationAnswer.Contains("run", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("runs", StringComparison.OrdinalIgnoreCase)) &&
                    vm.ExplanationAnswer.Contains("false", StringComparison.OrdinalIgnoreCase) &&
                    (vm.ExplanationAnswer.Contains("skip", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("skipped", StringComparison.OrdinalIgnoreCase));

                vm.ExplanationCorrect = explanationCorrect;
                vm.ExplanationFeedback = explanationCorrect
                    ? "Correct! IF statements run code only when a condition is true."
                    : "Try mentioning a condition, true running code, and false skipping code.";

                return View(vm);
            }

            if (actionType == "submit")
            {
                var saved = await SaveLessonProgressAsync("IfStatements");
                vm.ExplanationCorrect = true;
                vm.ExplanationFeedback = saved
                    ? "Lesson complete! Your progress has been saved."
                    : "Your answers were submitted, but progress could not be saved.";
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

        [HttpGet]
        public IActionResult IfElse()
        {
            return View(new IfElseViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IfElse(IfElseViewModel vm, string actionType)
        {
            try
            {
                var testResult = await _aiShortAnswerGrader.GradeAsync(new ShortAnswerEvaluationRequest
                {
                    QuestionText = "What does an IF-ELSE statement do?",
                    StudentAnswer = "It lets a program do one thing if a condition is true and another thing if it is false.",
                    ExpectedAnswer = "An IF-ELSE statement lets a program choose between two different actions depending on whether a condition is true or false.",
                    GradingRubric = """
                    Mark correct if the answer explains that:
                    1. A condition is checked.
                    2. One action happens if true.
                    3. Another action happens if false.
                    """
                });

                _logger.LogInformation(
                    "Gemini hardcoded test result. IsCorrect: {IsCorrect}, Score: {Score}, Feedback: {Feedback}",
                    testResult.IsCorrect,
                    testResult.Score,
                    testResult.Feedback);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gemini hardcoded test failed in IfElse.");
            }

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

            if (actionType == "checkExplanation")
            {
                _logger.LogInformation("IfElse checkExplanation triggered.");

                if (string.IsNullOrWhiteSpace(vm.ExplanationAnswer))
                {
                    _logger.LogWarning("IfElse explanation was empty.");
                    vm.ExplanationCorrect = false;
                    vm.ExplanationFeedback = "Please enter an explanation first.";
                    ViewBag.ForceStep = 2;
                    return View(vm);
                }

                _logger.LogInformation("IfElse explanation text: {Explanation}", vm.ExplanationAnswer);

                try
                {
                    _logger.LogInformation("About to call _aiShortAnswerGrader.GradeAsync for IfElse.");

                    var result = await _aiShortAnswerGrader.GradeAsync(new ShortAnswerEvaluationRequest
                    {
                        QuestionText = "Explain why IF-ELSE statements are useful in programming.",
                        StudentAnswer = vm.ExplanationAnswer,
                        ExpectedAnswer = "IF-ELSE statements are useful because they let a program choose between two different actions or outcomes based on whether a condition is true or false.",
                        GradingRubric = """
                        To be correct, the answer should clearly show that:
                        1. IF-ELSE helps a program make a decision.
                        2. One path or action happens when the condition is true.
                        3. A different path or action happens when the condition is false.

                        Accept simple student wording such as:
                        - choose between two outcomes
                        - do one thing if true and another if false
                        - make decisions based on a condition

                        Do not require advanced vocabulary.
                        Reject answers that are too vague or do not mention both true and false outcomes.
                        """
                    });

                    _logger.LogInformation(
                        "Gemini grading returned. IsCorrect: {IsCorrect}, Score: {Score}, Feedback: {Feedback}",
                        result.IsCorrect,
                        result.Score,
                        result.Feedback);

                    vm.ExplanationCorrect = result.IsCorrect;
                    vm.ExplanationFeedback = result.Feedback;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while calling Gemini for IfElse explanation check.");
                    vm.ExplanationCorrect = false;
                    vm.ExplanationFeedback = "We could not check your explanation right now. Please try again.";
                }

                ViewBag.ForceStep = 2;
                return View(vm);
            }

            if (actionType == "submit")
            {
                var saved = await SaveLessonProgressAsync("IfElse");
                vm.ExplanationCorrect = true;
                vm.ExplanationFeedback = saved
                    ? "Lesson complete! Your progress has been saved."
                    : "Your answers were submitted, but progress could not be saved.";
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

        [HttpGet]
        public IActionResult NestedConditions()
        {
            return View(new NestedConditionsViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NestedConditions(NestedConditionsViewModel vm, string actionType)
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

            if (actionType == "checkExplanation")
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

            if (actionType == "submit")
            {
                var saved = await SaveLessonProgressAsync("NestedConditions");
                vm.ExplanationCorrect = true;
                vm.ExplanationFeedback = saved
                    ? "Lesson complete! Your progress has been saved."
                    : "Your answers were submitted, but progress could not be saved.";
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

        [HttpGet]
        public IActionResult LogicalOperators()
        {
            return View(new LogicalOperatorsViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogicalOperators(LogicalOperatorsViewModel vm, string actionType)
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

            if (actionType == "checkExplanation")
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

            if (actionType == "submit")
            {
                var saved = await SaveLessonProgressAsync("LogicalOperators");
                vm.ExplanationCorrect = true;
                vm.ExplanationFeedback = saved
                    ? "Lesson complete! Your progress has been saved."
                    : "Your answers were submitted, but progress could not be saved.";
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

        private async Task<bool> SaveLessonProgressAsync(string actionName)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return false;
            }

            var lesson = await _context.Lessons
                .FirstOrDefaultAsync(l => l.ControllerName == "UnitThree" && l.ActionName == actionName && l.IsPublished);

            if (lesson == null)
            {
                return false;
            }

            var progress = await _context.UserLessonProgresses
                .FirstOrDefaultAsync(p => p.UserId == userId && p.LessonId == lesson.Id);

            var now = DateTime.UtcNow;

            if (progress == null)
            {
                progress = new UserLessonProgress
                {
                    UserId = userId,
                    LessonId = lesson.Id,
                    IsCompleted = true,
                    CompletedAtUtc = now,
                    LastAccessedAtUtc = now
                };

                _context.UserLessonProgresses.Add(progress);
            }
            else
            {
                progress.IsCompleted = true;
                progress.CompletedAtUtc = now;
                progress.LastAccessedAtUtc = now;
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}