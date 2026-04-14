using System;
using System.Security.Claims;
using System.Threading.Tasks;
using DotNetCoreSqlDb.Data;
using DotNetCoreSqlDb.Models;
using DotNetCoreSqlDb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNetCoreSqlDb.Controllers
{
    public class UnitTwoController : Controller
    {
        private readonly MyDatabaseContext _context;

        public UnitTwoController(MyDatabaseContext context)
        {
            _context = context;
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

            if (actionType == "checkExplanation")
            {
                vm.CurrentStep = 2;
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
                await SaveLessonProgressAsync("Variables");
                return RedirectToAction(nameof(DataTypes));
            }

            vm.CurrentStep = 0;
            return View(vm);
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
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return false;
            }

            var lesson = await _context.Lessons
                .FirstOrDefaultAsync(l => l.ControllerName == "UnitTwo" && l.ActionName == actionName && l.IsPublished);

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