using DotNetCoreSqlDb.Data;
using DotNetCoreSqlDb.Models;
using DotNetCoreSqlDb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DotNetCoreSqlDb.Controllers
{
    [Authorize]
    public class UnitSixController : Controller
    {
        private readonly MyDatabaseContext _context;

        public UnitSixController(MyDatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Functions()
        {
            return View(new FunctionsViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Functions(FunctionsViewModel vm, string actionType)
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
                    (vm.ExplanationAnswer.Contains("reuse", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("reusable", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("reusing", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("repeat", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("duplicate", StringComparison.OrdinalIgnoreCase)) &&
                    (vm.ExplanationAnswer.Contains("code", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("method", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("program", StringComparison.OrdinalIgnoreCase));

                vm.ExplanationCorrect = explanationCorrect;
                vm.ExplanationFeedback = explanationCorrect
                    ? "Correct! Methods let you reuse code instead of writing the same thing repeatedly."
                    : "Try mentioning reusing code, avoiding repetition, or keeping your program organized.";

                return View(vm);
            }

            if (actionType == "submit")
            {
                var saved = await SaveLessonProgressAsync("Functions");
                vm.ExplanationCorrect = true;
                vm.ExplanationFeedback = saved
                    ? "Lesson complete! Your progress has been saved."
                    : "Your answers were submitted, but progress could not be saved.";
                return View(vm);
            }

            vm.ShowHint = false;
            vm.ShowSolution = false;

            vm.IsQ1Correct = vm.UserAnswer1.Contains("Greet", StringComparison.OrdinalIgnoreCase);
            vm.Feedback1 = vm.IsQ1Correct == true
                ? "Correct!"
                : "Look for the name written after `void` in the method signature.";

            vm.IsQ2Correct = vm.UserAnswer2 == "Returns nothing";
            vm.Feedback2 = vm.IsQ2Correct == true
                ? "Correct! void means the method does not return any value."
                : "void means the method does not return a value.";

            vm.IsQ3Correct = vm.UserAnswer3.Equals("function", StringComparison.OrdinalIgnoreCase);
            vm.Feedback3 = vm.IsQ3Correct == true
                ? "Correct!"
                : "The keyword is `function`. It lets you call the method without creating an object.";

            vm.IsQ4Correct = vm.UserAnswer4 == "True";
            vm.Feedback4 = vm.IsQ4Correct == true
                ? "Correct! Methods can be called as many times as needed."
                : "Methods can be called more than once — that is the point of reusable code.";

            return View(vm);
        }

        [HttpGet]
        public IActionResult CallingFunctions()
        {
            return View(new CallingFunctionsViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CallingFunctions(CallingFunctionsViewModel vm, string actionType)
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
                    (vm.ExplanationAnswer.Contains("run", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("execute", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("executes", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("runs", StringComparison.OrdinalIgnoreCase)) &&
                    (vm.ExplanationAnswer.Contains("method", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("code", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("body", StringComparison.OrdinalIgnoreCase)) &&
                    (vm.ExplanationAnswer.Contains("call", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("calling", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("called", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("invoke", StringComparison.OrdinalIgnoreCase));

                vm.ExplanationCorrect = explanationCorrect;
                vm.ExplanationFeedback = explanationCorrect
                    ? "Correct! Calling a method runs its code body wherever you need it."
                    : "Try mentioning that calling a method runs or executes its code.";

                return View(vm);
            }

            if (actionType == "submit")
            {
                var saved = await SaveLessonProgressAsync("CallingFunctions");
                vm.ExplanationCorrect = true;
                vm.ExplanationFeedback = saved
                    ? "Lesson complete! Your progress has been saved."
                    : "Your answers were submitted, but progress could not be saved.";
                return View(vm);
            }

            vm.ShowHint = false;
            vm.ShowSolution = false;

            vm.IsQ1Correct =
                vm.UserAnswer1.Contains("PrintScore", StringComparison.OrdinalIgnoreCase) &&
                vm.UserAnswer1.Contains("95");
            vm.Feedback1 = vm.IsQ1Correct == true
                ? "Correct!"
                : "Write the method name followed by the value in parentheses: PrintScore(95)";

            vm.IsQ2Correct = vm.UserAnswer2 == "()";
            vm.Feedback2 = vm.IsQ2Correct == true
                ? "Correct! Parentheses are required to call a method."
                : "You must use parentheses () after the method name to call it.";

            vm.IsQ3Correct = vm.UserAnswer3 == "The method runs";
            vm.Feedback3 = vm.IsQ3Correct == true
                ? "Correct! The program jumps into the method body and executes it."
                : "When you call a method, its body runs immediately.";

            vm.IsQ4Correct = vm.UserAnswer4.Contains('3');
            vm.Feedback4 = vm.IsQ4Correct == true
                ? "Correct! Each call prints one line, so 3 calls print 3 lines."
                : "Each call to PrintScore prints one line. Three calls print 3 lines.";

            return View(vm);
        }

        [HttpGet]
        public IActionResult Parameters()
        {
            return View(new ParametersViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Parameters(ParametersViewModel vm, string actionType)
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
                    (vm.ExplanationAnswer.Contains("different", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("various", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("flexible", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("any", StringComparison.OrdinalIgnoreCase)) &&
                    (vm.ExplanationAnswer.Contains("value", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("values", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("input", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("data", StringComparison.OrdinalIgnoreCase));

                vm.ExplanationCorrect = explanationCorrect;
                vm.ExplanationFeedback = explanationCorrect
                    ? "Correct! Parameters allow methods to accept different input values each time they are called."
                    : "Try mentioning passing different values or making the method flexible with input.";

                return View(vm);
            }

            if (actionType == "submit")
            {
                var saved = await SaveLessonProgressAsync("Parameters");
                vm.ExplanationCorrect = true;
                vm.ExplanationFeedback = saved
                    ? "Lesson complete! Your progress has been saved."
                    : "Your answers were submitted, but progress could not be saved.";
                return View(vm);
            }

            vm.ShowHint = false;
            vm.ShowSolution = false;

            vm.IsQ1Correct = vm.UserAnswer1.Contains("name", StringComparison.OrdinalIgnoreCase);
            vm.Feedback1 = vm.IsQ1Correct == true
                ? "Correct!"
                : "The parameter is declared as `string name`, so its name is `name`.";

            vm.IsQ2Correct = vm.UserAnswer2 == "Hello, Alice!";
            vm.Feedback2 = vm.IsQ2Correct == true
                ? "Correct! The argument Alice replaces the parameter when the method runs."
                : "The argument replaces the parameter, so name becomes Alice and it prints: Hello, Alice!";

            vm.IsQ3Correct = vm.UserAnswer3.Equals("string", StringComparison.OrdinalIgnoreCase);
            vm.Feedback3 = vm.IsQ3Correct == true
                ? "Correct!"
                : "The parameter type is `string`, written before the parameter name.";

            vm.IsQ4Correct = vm.UserAnswer4 == "True";
            vm.Feedback4 = vm.IsQ4Correct == true
                ? "Correct! Parameters make methods flexible — you can pass different values each call."
                : "Parameters exist specifically so you can pass different values each time.";

            return View(vm);
        }

        [HttpGet]
        public IActionResult ReturnValues()
        {
            return View(new ReturnValuesViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReturnValues(ReturnValuesViewModel vm, string actionType)
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
                    (vm.ExplanationAnswer.Contains("return", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("returns", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("send back", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("gives back", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("give back", StringComparison.OrdinalIgnoreCase)) &&
                    (vm.ExplanationAnswer.Contains("value", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("result", StringComparison.OrdinalIgnoreCase)) &&
                    (vm.ExplanationAnswer.Contains("use", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("store", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("using", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("used", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("stored", StringComparison.OrdinalIgnoreCase));

                vm.ExplanationCorrect = explanationCorrect;
                vm.ExplanationFeedback = explanationCorrect
                    ? "Correct! Return values let the caller receive and use the result of a method."
                    : "Try mentioning sending back a value and using or storing the result.";

                return View(vm);
            }

            if (actionType == "submit")
            {
                var saved = await SaveLessonProgressAsync("ReturnValues");
                vm.ExplanationCorrect = true;
                vm.ExplanationFeedback = saved
                    ? "Lesson complete! Your progress has been saved."
                    : "Your answers were submitted, but progress could not be saved.";
                return View(vm);
            }

            vm.ShowHint = false;
            vm.ShowSolution = false;

            vm.IsQ1Correct = vm.UserAnswer1.Equals("int", StringComparison.OrdinalIgnoreCase);
            vm.Feedback1 = vm.IsQ1Correct == true
                ? "Correct!"
                : "The return type is `int`, written before the method name in the signature.";

            vm.IsQ2Correct = vm.UserAnswer2 == "8";
            vm.Feedback2 = vm.IsQ2Correct == true
                ? "Correct! 3 + 5 = 8."
                : "Add adds the two numbers together: 3 + 5 = 8.";

            vm.IsQ3Correct = vm.UserAnswer3.Equals("return", StringComparison.OrdinalIgnoreCase);
            vm.Feedback3 = vm.IsQ3Correct == true
                ? "Correct!"
                : "The `return` keyword exits the method and sends a value back to the caller.";

            vm.IsQ4Correct = vm.UserAnswer4 == "Returns nothing";
            vm.Feedback4 = vm.IsQ4Correct == true
                ? "Correct! void methods do not return any value."
                : "void means no value is returned — it is different from returning zero.";

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
                .FirstOrDefaultAsync(l => l.ControllerName == "UnitSix" && l.ActionName == actionName && l.IsPublished);

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
