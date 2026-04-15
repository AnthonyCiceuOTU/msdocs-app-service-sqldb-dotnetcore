using Microsoft.AspNetCore.Mvc;
using DotNetCoreSqlDb.ViewModels;
using DotNetCoreSqlDb.Data;
using DotNetCoreSqlDb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace DotNetCoreSqlDb.Controllers
{
    [Authorize]
    public class UnitFiveController : Controller
    {
        private readonly MyDatabaseContext _context;

        public UnitFiveController(MyDatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Lists()
        {
            return View(new ListsViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Lists(ListsViewModel vm, string actionType)
        {
            vm.UserAnswer = vm.UserAnswer?.Trim() ?? "";
            vm.ExplanationAnswer = vm.ExplanationAnswer?.Trim() ?? "";

            if (actionType == "hint")
            {
                vm.ShowHint = true;
                vm.ShowSolution = false;
                vm.IsCorrect = null;
                vm.FeedbackMessage = "Hint: a list lets you keep several related values together in one place.";
                return View(vm);
            }

            if (actionType == "solution")
            {
                vm.ShowHint = false;
                vm.ShowSolution = true;
                vm.IsCorrect = null;
                vm.FeedbackMessage = "Solution: a list is used to store multiple values in one variable.";
                return View(vm);
            }

            if (actionType == "checkExplanation")
            {
                vm.IsCorrect = true;

                bool explanationCorrect =
                    vm.ExplanationAnswer.Contains("multiple", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("many", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("items", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("values", StringComparison.OrdinalIgnoreCase);

                explanationCorrect = explanationCorrect && vm.ExplanationAnswer.Length >= 10;

                vm.ExplanationCorrect = explanationCorrect;
                vm.ExplanationFeedback = explanationCorrect
                    ? "Good explanation. Lists are useful because they store multiple values together."
                    : "Add a bit more detail about how lists help store multiple values in one place.";

                return View(vm);
            }

            if (actionType == "submit")
            {
                vm.IsCorrect = true;

                bool explanationCorrect =
                    vm.ExplanationAnswer.Contains("multiple", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("many", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("items", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("values", StringComparison.OrdinalIgnoreCase);

                explanationCorrect = explanationCorrect && vm.ExplanationAnswer.Length >= 10;

                vm.ExplanationCorrect = explanationCorrect;
                vm.ExplanationFeedback = explanationCorrect
                    ? "Good explanation. Lists are useful because they store multiple values together."
                    : "Add a bit more detail about how lists help store multiple values in one place.";

                if (vm.ExplanationCorrect != true)
                {
                    return View(vm);
                }

                await SaveLessonProgressAsync("Lists");
                return await RedirectToNextIncompleteUnitFiveLessonOrLessonsAsync();
            }

            bool isCorrect =
                vm.UserAnswer.Contains("multiple", StringComparison.OrdinalIgnoreCase) ||
                vm.UserAnswer.Contains("many", StringComparison.OrdinalIgnoreCase) ||
                vm.UserAnswer.Contains("several", StringComparison.OrdinalIgnoreCase);

            vm.IsCorrect = isCorrect;
            vm.ShowHint = false;
            vm.ShowSolution = false;
            vm.FeedbackMessage = isCorrect
                ? "Correct! Lists store multiple values."
                : "Not quite. Think about what lists allow you to store.";

            return View(vm);
        }

        [HttpGet]
        public IActionResult Accessing()
        {
            return View(new AccessingViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Accessing(AccessingViewModel vm, string actionType)
        {
            vm.UserAnswer = vm.UserAnswer?.Trim() ?? "";
            vm.ExplanationAnswer = vm.ExplanationAnswer?.Trim() ?? "";

            if (actionType == "hint")
            {
                vm.ShowHint = true;
                vm.ShowSolution = false;
                vm.IsCorrect = null;
                vm.FeedbackMessage = "Hint: use square brackets with the first index, which starts at 0.";
                return View(vm);
            }

            if (actionType == "solution")
            {
                vm.ShowHint = false;
                vm.ShowSolution = true;
                vm.IsCorrect = null;
                vm.FeedbackMessage = "Solution: use scores[0] to access the first item.";
                return View(vm);
            }

            if (actionType == "checkExplanation")
            {
                vm.IsCorrect = true;

                bool explanationCorrect =
                    vm.ExplanationAnswer.Contains("index", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("position", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("0", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("first", StringComparison.OrdinalIgnoreCase);

                explanationCorrect = explanationCorrect && vm.ExplanationAnswer.Length >= 10;

                vm.ExplanationCorrect = explanationCorrect;
                vm.ExplanationFeedback = explanationCorrect
                    ? "Good explanation. You explained how indexing helps access an item."
                    : "Mention that list items are accessed by index, and that the first index is 0.";

                return View(vm);
            }

            if (actionType == "submit")
            {
                vm.IsCorrect = true;

                bool explanationCorrect =
                    vm.ExplanationAnswer.Contains("index", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("position", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("0", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("first", StringComparison.OrdinalIgnoreCase);

                explanationCorrect = explanationCorrect && vm.ExplanationAnswer.Length >= 10;

                vm.ExplanationCorrect = explanationCorrect;
                vm.ExplanationFeedback = explanationCorrect
                    ? "Good explanation. You explained how indexing helps access an item."
                    : "Mention that list items are accessed by index, and that the first index is 0.";

                if (vm.ExplanationCorrect != true)
                {
                    return View(vm);
                }

                await SaveLessonProgressAsync("Accessing");
                return await RedirectToNextIncompleteUnitFiveLessonOrLessonsAsync();
            }

            bool isCorrect =
                vm.UserAnswer.Contains("[0]") ||
                vm.UserAnswer.Equals("scores[0]", StringComparison.OrdinalIgnoreCase);

            vm.IsCorrect = isCorrect;
            vm.ShowHint = false;
            vm.ShowSolution = false;
            vm.FeedbackMessage = isCorrect
                ? "Correct! Indexing starts at 0."
                : "Not quite. Remember the first index is 0.";

            return View(vm);
        }

        [HttpGet]
        public IActionResult Looping()
        {
            return View(new LoopingViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Looping(LoopingViewModel vm, string actionType)
        {
            vm.UserAnswer = vm.UserAnswer?.Trim() ?? "";
            vm.ExplanationAnswer = vm.ExplanationAnswer?.Trim() ?? "";

            if (actionType == "hint")
            {
                vm.ShowHint = true;
                vm.ShowSolution = false;
                vm.IsCorrect = null;
                vm.FeedbackMessage = "Hint: use a loop to go through each item one at a time.";
                return View(vm);
            }

            if (actionType == "solution")
            {
                vm.ShowHint = false;
                vm.ShowSolution = true;
                vm.IsCorrect = null;
                vm.FeedbackMessage = "Solution: use a FOR loop to go through each item in the list.";
                return View(vm);
            }

            if (actionType == "checkExplanation")
            {
                vm.IsCorrect = true;

                bool explanationCorrect =
                    vm.ExplanationAnswer.Contains("each", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("every", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("item", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("repeat", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("repetition", StringComparison.OrdinalIgnoreCase);

                explanationCorrect = explanationCorrect && vm.ExplanationAnswer.Length >= 10;

                vm.ExplanationCorrect = explanationCorrect;
                vm.ExplanationFeedback = explanationCorrect
                    ? "Good explanation. Loops help process each item in a list without repetition."
                    : "Add a little more detail about how loops repeat through each item in the list.";

                return View(vm);
            }

            if (actionType == "submit")
            {
                vm.IsCorrect = true;

                bool explanationCorrect =
                    vm.ExplanationAnswer.Contains("each", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("every", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("item", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("repeat", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("repetition", StringComparison.OrdinalIgnoreCase);

                explanationCorrect = explanationCorrect && vm.ExplanationAnswer.Length >= 10;

                vm.ExplanationCorrect = explanationCorrect;
                vm.ExplanationFeedback = explanationCorrect
                    ? "Good explanation. Loops help process each item in a list without repetition."
                    : "Add a little more detail about how loops repeat through each item in the list.";

                if (vm.ExplanationCorrect != true)
                {
                    return View(vm);
                }

                await SaveLessonProgressAsync("Looping");
                return await RedirectToNextIncompleteUnitFiveLessonOrLessonsAsync();
            }

            bool isCorrect =
                vm.UserAnswer.Contains("for", StringComparison.OrdinalIgnoreCase) ||
                vm.UserAnswer.Contains("loop", StringComparison.OrdinalIgnoreCase);

            vm.IsCorrect = isCorrect;
            vm.ShowHint = false;
            vm.ShowSolution = false;
            vm.FeedbackMessage = isCorrect
                ? "Correct! Loops allow you to process each item in a list."
                : "Not quite. Think about which loop works best here.";

            return View(vm);
        }

        [HttpGet]
        public IActionResult Searching()
        {
            return View(new SearchingViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Searching(SearchingViewModel vm, string actionType)
        {
            vm.UserAnswer = vm.UserAnswer?.Trim() ?? "";
            vm.ExplanationAnswer = vm.ExplanationAnswer?.Trim() ?? "";

            if (actionType == "hint")
            {
                vm.ShowHint = true;
                vm.ShowSolution = false;
                vm.IsCorrect = null;
                vm.FeedbackMessage = "Hint: go through each item and compare it to the value you want.";
                return View(vm);
            }

            if (actionType == "solution")
            {
                vm.ShowHint = false;
                vm.ShowSolution = true;
                vm.IsCorrect = null;
                vm.FeedbackMessage = "Solution: use a loop with an IF statement to compare each item.";
                return View(vm);
            }

            if (actionType == "checkExplanation")
            {
                vm.IsCorrect = true;

                bool explanationCorrect =
                    vm.ExplanationAnswer.Contains("each", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("item", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("one by one", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("find", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("search", StringComparison.OrdinalIgnoreCase);

                explanationCorrect = explanationCorrect && vm.ExplanationAnswer.Length >= 10;

                vm.ExplanationCorrect = explanationCorrect;
                vm.ExplanationFeedback = explanationCorrect
                    ? "Good explanation. Searching checks items until the target is found."
                    : "Explain a bit more clearly that searching checks items one by one until it finds the target.";

                return View(vm);
            }

            if (actionType == "submit")
            {
                vm.IsCorrect = true;

                bool explanationCorrect =
                    vm.ExplanationAnswer.Contains("each", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("item", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("one by one", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("find", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("search", StringComparison.OrdinalIgnoreCase);

                explanationCorrect = explanationCorrect && vm.ExplanationAnswer.Length >= 10;

                vm.ExplanationCorrect = explanationCorrect;
                vm.ExplanationFeedback = explanationCorrect
                    ? "Good explanation. Searching checks items until the target is found."
                    : "Explain a bit more clearly that searching checks items one by one until it finds the target.";

                if (vm.ExplanationCorrect != true)
                {
                    return View(vm);
                }

                await SaveLessonProgressAsync("Searching");
                return await RedirectToNextIncompleteUnitFiveLessonOrLessonsAsync();
            }

            bool hasLoop = vm.UserAnswer.Contains("for", StringComparison.OrdinalIgnoreCase) || vm.UserAnswer.Contains("loop", StringComparison.OrdinalIgnoreCase);
            bool hasIf = vm.UserAnswer.Contains("if", StringComparison.OrdinalIgnoreCase) || vm.UserAnswer.Contains("compare", StringComparison.OrdinalIgnoreCase);
            bool isCorrect = hasLoop && hasIf;

            vm.IsCorrect = isCorrect;
            vm.ShowHint = false;
            vm.ShowSolution = false;
            vm.FeedbackMessage = isCorrect
                ? "Correct! Linear search checks each item."
                : "Not quite. You need both a loop and a condition.";

            return View(vm);
        }

        public IActionResult Index()
        {
            return View();
        }

        private async Task<bool> SaveLessonProgressAsync(string actionName)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserID");
            if (!Guid.TryParse(userIdValue, out var userId)) return false;

            var lesson = await _context.Lessons.FirstOrDefaultAsync(l => l.ActionName == actionName);
            if (lesson == null) return false;

            var progress = await _context.UserLessonProgresses.FirstOrDefaultAsync(p => p.UserId == userId && p.LessonId == lesson.Id);
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

        private async Task<IActionResult> RedirectToNextIncompleteUnitFiveLessonOrLessonsAsync()
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserID");
            if (!Guid.TryParse(userIdValue, out var userId))
            {
                return RedirectToAction("Index", "Lessons");
            }

            var unitFiveLessons = await _context.Lessons
                .Where(l => l.IsPublished && l.UnitId == 5)
                .OrderBy(l => l.SortOrder)
                .ThenBy(l => l.Id)
                .ToListAsync();

            if (!unitFiveLessons.Any())
            {
                return RedirectToAction("Index", "Lessons");
            }

            var completedLessonIds = await _context.UserLessonProgresses
                .Where(p => p.UserId == userId && p.IsCompleted)
                .Select(p => p.LessonId)
                .ToListAsync();

            var nextIncomplete = unitFiveLessons.FirstOrDefault(l => !completedLessonIds.Contains(l.Id));

            if (nextIncomplete == null)
            {
                return RedirectToAction("Index", "Lessons");
            }

            return RedirectToAction(nextIncomplete.ActionName, nextIncomplete.ControllerName);
        }
    }
}