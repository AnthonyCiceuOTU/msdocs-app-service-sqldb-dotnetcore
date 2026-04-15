using Microsoft.AspNetCore.Mvc;
using DotNetCoreSqlDb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using DotNetCoreSqlDb.Data;
using DotNetCoreSqlDb.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using DotNetCoreSqlDb.Models.AI;
using DotNetCoreSqlDb.Services;
using Microsoft.Extensions.Logging;

namespace DotNetCoreSqlDb.Controllers
{
    public class UnitTwoController : Controller
    {
        private readonly MyDatabaseContext _context;
        private readonly IAiShortAnswerGrader _aiShortAnswerGrader;
        private readonly ILogger<UnitTwoController> _logger;

        public UnitTwoController(MyDatabaseContext context, IAiShortAnswerGrader aiShortAnswerGrader, ILogger<UnitTwoController> logger)
        {
            _context = context;
            _aiShortAnswerGrader = aiShortAnswerGrader;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Variables()
        {
            return View(new VariablesViewModel { CurrentStep = 0 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Variables(VariablesViewModel vm, string actionType)
        {
            vm.UserAnswer = vm.UserAnswer?.Trim() ?? "";
            vm.ExplanationAnswer = vm.ExplanationAnswer?.Trim() ?? "";
            vm.ExplanationFeedback = vm.ExplanationFeedback?.Trim() ?? "";

            if (actionType == "hint")
            {
                vm.CurrentStep = 1;
                vm.ShowHint = true;
                vm.ShowSolution = false;
                vm.IsCorrect = null;
                vm.FeedbackMessage = "Hint: assign the number directly to the variable using the equals sign.";
                return View(vm);
            }

            if (actionType == "solution")
            {
                vm.CurrentStep = 1;
                vm.ShowHint = false;
                vm.ShowSolution = true;
                vm.IsCorrect = null;
                vm.FeedbackMessage = "Solution: the missing value is 25, so the full line becomes SET points = 25.";
                return View(vm);
            }

            if (actionType == "check")
            {
                vm.CurrentStep = 1;

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

            if (actionType == "submit")
            {
                vm.CurrentStep = 2;
                vm.IsCorrect = true;

                if (vm.ExplanationCorrect != true)
                {
                    vm.ExplanationFeedback = string.IsNullOrWhiteSpace(vm.ExplanationFeedback)
                        ? "Please check your explanation with AI before submitting."
                        : vm.ExplanationFeedback;

                    return View(vm);
                }

                await SaveLessonProgressAsync("Variables");
                return RedirectToAction(nameof(DataTypes));
            }

            vm.CurrentStep = 0;
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckVariablesExplanation([FromForm] string explanationAnswer)
        {
            explanationAnswer = explanationAnswer?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(explanationAnswer))
            {
                return BadRequest(new
                {
                    isCorrect = false,
                    feedback = "Please enter an explanation first."
                });
            }

            try
            {
                var result = await _aiShortAnswerGrader.GradeAsync(new ShortAnswerEvaluationRequest
                {
                    QuestionText = "Explain what a variable is used for in a program.",
                    StudentAnswer = explanationAnswer,
                    ExpectedAnswer = "A variable is used to store a value or piece of information so the program can use it later.",
                    GradingRubric = """
                    To be correct, the answer should clearly show that:

                    1. A variable stores, saves, or holds something.
                    2. What it stores is a value, data, or information.
                    3. The program can use that stored value later.

                    Accept simple student wording such as:
                    - a variable stores a value
                    - it saves information
                    - it holds data for later
                    - the program remembers something in a variable

                    Do not require advanced vocabulary.
                    Minor spelling or grammar mistakes are okay.
                    Reject answers that are too vague or do not mention storing/saving a value or information.
                    """
                });

                return Json(new
                {
                    isCorrect = result.IsCorrect,
                    feedback = result.Feedback
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while checking Variables explanation.");
                return StatusCode(500, new
                {
                    isCorrect = false,
                    feedback = "We could not check your explanation right now. Please try again."
                });
            }
        }

        [HttpGet]
        public IActionResult DataTypes()
        {
            return View(new DataTypesViewModel { CurrentStep = 0 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DataTypes(DataTypesViewModel vm, string actionType)
        {
            vm.UserAnswer = vm.UserAnswer?.Trim() ?? "";
            vm.ExplanationAnswer = vm.ExplanationAnswer?.Trim() ?? "";

            if (actionType == "hint")
            {
                vm.CurrentStep = 1;
                vm.ShowHint = true;
                vm.ShowSolution = false;
                vm.IsCorrect = null;
                vm.FeedbackMessage = "Hint: text values use quotation marks, but numbers do not.";
                return View(vm);
            }

            if (actionType == "solution")
            {
                vm.CurrentStep = 1;
                vm.ShowHint = false;
                vm.ShowSolution = true;
                vm.IsCorrect = null;
                vm.FeedbackMessage = "Solution: the correct answer is \"Alex\" because Alex is a text value.";
                return View(vm);
            }

            if (actionType == "check")
            {
                vm.CurrentStep = 1;

                bool isCorrect =
                    vm.UserAnswer.Equals("\"Alex\"", StringComparison.OrdinalIgnoreCase) ||
                    vm.UserAnswer.Equals("'Alex'", StringComparison.OrdinalIgnoreCase);

                vm.IsCorrect = isCorrect;
                vm.ShowHint = false;
                vm.ShowSolution = false;
                vm.FeedbackMessage = isCorrect
                    ? "Correct! Alex is text, so it should be written in quotation marks."
                    : "Not quite. Text values should be written in quotation marks.";

                return View(vm);
            }

            if (actionType == "checkExplanation")
            {
                vm.CurrentStep = 2;
                vm.IsCorrect = true;

                bool explanationCorrect =
                    vm.ExplanationAnswer.Contains("type", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("kind", StringComparison.OrdinalIgnoreCase);

                explanationCorrect = explanationCorrect &&
                    (
                        vm.ExplanationAnswer.Contains("data", StringComparison.OrdinalIgnoreCase) ||
                        vm.ExplanationAnswer.Contains("value", StringComparison.OrdinalIgnoreCase) ||
                        vm.ExplanationAnswer.Contains("information", StringComparison.OrdinalIgnoreCase)
                    );

                explanationCorrect = explanationCorrect && vm.ExplanationAnswer.Length >= 10;

                vm.ExplanationCorrect = explanationCorrect;
                vm.ExplanationFeedback = explanationCorrect
                    ? "Good explanation. A data type tells the program what kind of value is being stored."
                    : "Try mentioning that a data type describes what kind of value is stored, like text or a number.";

                return View(vm);
            }

            if (actionType == "submit")
            {
                await SaveLessonProgressAsync("DataTypes");
                return RedirectToAction(nameof(ArithmeticExpressions));
            }

            vm.CurrentStep = 0;
            return View(vm);
        }

        [HttpGet]
        public IActionResult ArithmeticExpressions()
        {
            return View(new ArithmeticExpressionsViewModel { CurrentStep = 0 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ArithmeticExpressions(ArithmeticExpressionsViewModel vm, string actionType)
        {
            vm.UserAnswer = vm.UserAnswer?.Trim() ?? "";
            vm.ExplanationAnswer = vm.ExplanationAnswer?.Trim() ?? "";

            if (actionType == "hint")
            {
                vm.CurrentStep = 1;
                vm.ShowHint = true;
                vm.ShowSolution = false;
                vm.IsCorrect = null;
                vm.FeedbackMessage = "Hint: use both variables in one arithmetic expression.";
                return View(vm);
            }

            if (actionType == "solution")
            {
                vm.CurrentStep = 1;
                vm.ShowHint = false;
                vm.ShowSolution = true;
                vm.IsCorrect = null;
                vm.FeedbackMessage = "Solution: total = apples + oranges";
                return View(vm);
            }

            if (actionType == "check")
            {
                vm.CurrentStep = 1;

                var normalized = vm.UserAnswer.Replace(" ", "").ToLowerInvariant();
                bool isCorrect =
                    normalized == "apples+oranges" ||
                    normalized == "oranges+apples";

                vm.IsCorrect = isCorrect;
                vm.ShowHint = false;
                vm.ShowSolution = false;
                vm.FeedbackMessage = isCorrect
                    ? "Correct! The total is found by adding apples and oranges."
                    : "Not quite. Use both variables in one addition expression.";

                return View(vm);
            }

            if (actionType == "checkExplanation")
            {
                vm.CurrentStep = 2;
                vm.IsCorrect = true;

                bool explanationCorrect =
                    vm.ExplanationAnswer.Contains("math", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("calculate", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("combine", StringComparison.OrdinalIgnoreCase);

                explanationCorrect = explanationCorrect &&
                    (
                        vm.ExplanationAnswer.Contains("values", StringComparison.OrdinalIgnoreCase) ||
                        vm.ExplanationAnswer.Contains("variables", StringComparison.OrdinalIgnoreCase) ||
                        vm.ExplanationAnswer.Contains("numbers", StringComparison.OrdinalIgnoreCase)
                    );

                explanationCorrect = explanationCorrect && vm.ExplanationAnswer.Length >= 10;

                vm.ExplanationCorrect = explanationCorrect;
                vm.ExplanationFeedback = explanationCorrect
                    ? "Good explanation. Arithmetic expressions combine values using math operations."
                    : "Try mentioning that arithmetic expressions use math operations on values or variables.";

                return View(vm);
            }

            if (actionType == "submit")
            {
                await SaveLessonProgressAsync("ArithmeticExpressions");
                return RedirectToAction(nameof(InputOutput));
            }

            vm.CurrentStep = 0;
            return View(vm);
        }

        [HttpGet]
        public IActionResult InputOutput()
        {
            return View(new InputOutputViewModel { CurrentStep = 0 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InputOutput(InputOutputViewModel vm, string actionType)
        {
            vm.UserAnswer = vm.UserAnswer?.Trim() ?? "";
            vm.ExplanationAnswer = vm.ExplanationAnswer?.Trim() ?? "";

            if (actionType == "hint")
            {
                vm.CurrentStep = 1;
                vm.ShowHint = true;
                vm.ShowSolution = false;
                vm.IsCorrect = null;
                vm.FeedbackMessage = "Hint: the program should first get input, then display that same value.";
                return View(vm);
            }

            if (actionType == "solution")
            {
                vm.CurrentStep = 1;
                vm.ShowHint = false;
                vm.ShowSolution = true;
                vm.IsCorrect = null;
                vm.FeedbackMessage = "Solution: name";
                return View(vm);
            }

            if (actionType == "check")
            {
                vm.CurrentStep = 1;

                bool isCorrect = vm.UserAnswer.Equals("name", StringComparison.OrdinalIgnoreCase);

                vm.IsCorrect = isCorrect;
                vm.ShowHint = false;
                vm.ShowSolution = false;
                vm.FeedbackMessage = isCorrect
                    ? "Correct! The program gets the name as input and then displays it."
                    : "Not quite. Use the same variable that stores the user's name.";

                return View(vm);
            }

            if (actionType == "checkExplanation")
            {
                vm.CurrentStep = 2;
                vm.IsCorrect = true;

                bool explanationCorrect =
                    vm.ExplanationAnswer.Contains("input", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("enter", StringComparison.OrdinalIgnoreCase);

                explanationCorrect = explanationCorrect &&
                    (
                        vm.ExplanationAnswer.Contains("output", StringComparison.OrdinalIgnoreCase) ||
                        vm.ExplanationAnswer.Contains("display", StringComparison.OrdinalIgnoreCase) ||
                        vm.ExplanationAnswer.Contains("show", StringComparison.OrdinalIgnoreCase)
                    );

                explanationCorrect = explanationCorrect && vm.ExplanationAnswer.Length >= 10;

                vm.ExplanationCorrect = explanationCorrect;
                vm.ExplanationFeedback = explanationCorrect
                    ? "Good explanation. Input gets information from the user, and output shows information back."
                    : "Try mentioning that input gets information and output displays it.";

                return View(vm);
            }

            if (actionType == "submit")
            {
                await SaveLessonProgressAsync("InputOutput");
                return RedirectToAction("Index", "Lessons");
            }

            vm.CurrentStep = 0;
            return View(vm);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction(nameof(Variables));
        }

        private async Task<bool> SaveLessonProgressAsync(string actionName)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier)
                            ?? User.FindFirstValue("UserID");

            if (!Guid.TryParse(userIdValue, out var userId))
            {
                return false;
            }

            var lesson = await _context.Lessons
                .FirstOrDefaultAsync(l => l.ActionName == actionName);

            if (lesson == null)
            {
                return false;
            }

            var progress = await _context.UserLessonProgresses
                .FirstOrDefaultAsync(p => p.UserId == userId && p.LessonId == lesson.Id);

            if (progress == null)
            {
                progress = new UserLessonProgress
                {
                    UserId = userId,
                    LessonId = lesson.Id,
                    IsCompleted = true,
                    CompletedAtUtc = DateTime.UtcNow,
                    LastAccessedAtUtc = DateTime.UtcNow
                };

                _context.UserLessonProgresses.Add(progress);
            }
            else
            {
                progress.IsCompleted = true;
                progress.CompletedAtUtc ??= DateTime.UtcNow;
                progress.LastAccessedAtUtc = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}