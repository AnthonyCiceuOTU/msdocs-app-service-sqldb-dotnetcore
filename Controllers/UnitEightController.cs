using System.Security.Claims;
using DotNetCoreSqlDb.Data;
using DotNetCoreSqlDb.Models;
using DotNetCoreSqlDb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNetCoreSqlDb.Controllers
{
    public class UnitEightController : Controller
    {
        private readonly MyDatabaseContext _context;

        public UnitEightController(MyDatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult DataProcessing()
        {
            return View(new DataProcessingViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DataProcessing(DataProcessingViewModel vm, string actionType)
        {
            Normalize(vm);

            await HandleLessonAsync(
                vm,
                actionType,
                "DataProcessing",
                "Hint: data processing usually means taking raw data, working on it, and turning it into useful information.",
                "Solution: 1) process  2) divide  3) B) useful information",
                q1 => q1.Equals("process", StringComparison.OrdinalIgnoreCase) ||
                      q1.Equals("processing", StringComparison.OrdinalIgnoreCase),
                q2 => q2.Equals("divide", StringComparison.OrdinalIgnoreCase) ||
                      q2.Equals("division", StringComparison.OrdinalIgnoreCase),
                q3 => q3 == "B",
                "Correct! Data processing turns raw data into useful information.",
                "Not quite yet. Review what happens to raw data and how averages are calculated.",
                "Lesson complete!");

            return View(vm);
        }

        [HttpGet]
        public IActionResult Simulation()
        {
            return View(new SimulationViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Simulation(SimulationViewModel vm, string actionType)
        {
            Normalize(vm);

            await HandleLessonAsync(
                vm,
                actionType,
                "Simulation",
                "Hint: a simulation models something from real life, and random(1,6) acts like a dice roll.",
                "Solution: 1) model  2) dice  3) A) To test or predict real-world behaviour",
                q1 => q1.Equals("model", StringComparison.OrdinalIgnoreCase) ||
                      q1.Equals("models", StringComparison.OrdinalIgnoreCase),
                q2 => q2.Equals("dice", StringComparison.OrdinalIgnoreCase) ||
                      q2.Equals("die", StringComparison.OrdinalIgnoreCase),
                q3 => q3 == "A",
                "Correct! Simulations model real-world systems and can help with testing or prediction.",
                "Not quite yet. Think about modeling real-life situations and using random outcomes.",
                "Lesson complete!");

            return View(vm);
        }

        [HttpGet]
        public IActionResult DesigningProgram()
        {
            return View(new DesigningProgramViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DesigningProgram(DesigningProgramViewModel vm, string actionType)
        {
            Normalize(vm);

            await HandleLessonAsync(
                vm,
                actionType,
                "DesigningProgram",
                "Hint: good program design starts with planning, and many programs follow input-process-output.",
                "Solution: 1) plan  2) input  3) C) They make programs easier to manage and reuse",
                q1 => q1.Equals("plan", StringComparison.OrdinalIgnoreCase) ||
                      q1.Equals("planning", StringComparison.OrdinalIgnoreCase),
                q2 => q2.Equals("input", StringComparison.OrdinalIgnoreCase),
                q3 => q3 == "C",
                "Correct! Designing a program means planning it before coding and organizing its parts well.",
                "Not quite yet. Think about planning first and using clear program structure.",
                "Lesson complete!");

            return View(vm);
        }

        public IActionResult Index()
        {
            return View();
        }

        private static void Normalize(dynamic vm)
        {
            vm.Q1Answer = vm.Q1Answer?.Trim() ?? string.Empty;
            vm.Q2Answer = vm.Q2Answer?.Trim() ?? string.Empty;
            vm.Q3Answer = vm.Q3Answer?.Trim() ?? string.Empty;
            vm.ExplanationAnswer = vm.ExplanationAnswer?.Trim() ?? string.Empty;
        }

        private async Task HandleLessonAsync(
            dynamic vm,
            string actionType,
            string actionName,
            string hintMessage,
            string solutionMessage,
            Func<string, bool> q1Checker,
            Func<string, bool> q2Checker,
            Func<string, bool> q3Checker,
            string successMessage,
            string retryMessage,
            string completionMessage)
        {
            if (actionType == "hint")
            {
                vm.ShowHint = true;
                vm.ShowSolution = false;
                vm.Feedback1 = hintMessage;
                vm.Feedback2 = null;
                vm.Feedback3 = null;
                return;
            }

            if (actionType == "solution")
            {
                vm.ShowHint = false;
                vm.ShowSolution = true;
                vm.Feedback1 = solutionMessage;
                vm.Feedback2 = null;
                vm.Feedback3 = null;
                return;
            }

            bool q1Correct = q1Checker(vm.Q1Answer);
            bool q2Correct = q2Checker(vm.Q2Answer);
            bool q3Correct = q3Checker(vm.Q3Answer);
            bool allCorrect = q1Correct && q2Correct && q3Correct;

            vm.IsQ1Correct = q1Correct;
            vm.IsQ2Correct = q2Correct;
            vm.IsQ3Correct = q3Correct;
            vm.ShowHint = false;
            vm.ShowSolution = false;

            vm.Feedback1 = q1Correct ? "Correct!" : "Try again.";
            vm.Feedback2 = q2Correct ? "Correct!" : "Try again.";
            vm.Feedback3 = q3Correct ? "Correct!" : "Try again.";

            if (actionType == "submit")
            {
                if (allCorrect)
                {
                    await SaveLessonProgressAsync(actionName);
                    vm.Feedback1 = completionMessage;
                    vm.Feedback2 = null;
                    vm.Feedback3 = null;
                }
                else
                {
                    vm.Feedback1 = "Please answer all questions correctly before marking the lesson complete.";
                    vm.Feedback2 = null;
                    vm.Feedback3 = null;
                }

                return;
            }

            if (actionType == "check")
            {
                vm.Feedback1 = allCorrect ? successMessage : retryMessage;
                vm.Feedback2 = null;
                vm.Feedback3 = null;
            }
        }

        private async Task<bool> SaveLessonProgressAsync(string actionName)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserID");

            if (!Guid.TryParse(userIdValue, out var userId))
            {
                return false;
            }

            var lesson = await _context.Lessons
                .FirstOrDefaultAsync(l => l.ControllerName == "UnitEight" && l.ActionName == actionName && l.IsPublished);

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
                progress.CompletedAtUtc ??= now;
                progress.LastAccessedAtUtc = now;
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}