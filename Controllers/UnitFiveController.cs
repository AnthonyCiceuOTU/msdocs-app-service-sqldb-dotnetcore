using System.Security.Claims;
using DotNetCoreSqlDb.Data;
using DotNetCoreSqlDb.Models;
using DotNetCoreSqlDb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNetCoreSqlDb.Controllers
{
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
            Normalize(vm);

            await HandleCommonActionsAsync(
                vm,
                actionType,
                "Lists",
                "Hint: think about what a list stores.",
                "Solution: Question 1 = B) Storing multiple related values in one variable. Question 2 = A) A list can hold many items together.",
                IsListsQ1Correct,
                IsListsQ2Correct,
                "Correct! A list stores multiple related values in one variable.",
                "Not quite yet. A list is used to keep several related values together.",
                "Lesson complete!");

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
            Normalize(vm);

            await HandleCommonActionsAsync(
                vm,
                actionType,
                "Accessing",
                "Hint: the first item in a list uses index 0.",
                "Solution: Question 1 = C) scores[0]. Question 2 = B) Because list indexes start at 0.",
                IsAccessingQ1Correct,
                IsAccessingQ2Correct,
                "Correct! The first item in a list is usually accessed with index 0.",
                "Not quite yet. Remember that most lists start indexing at 0.",
                "Lesson complete!");

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
            Normalize(vm);

            await HandleCommonActionsAsync(
                vm,
                actionType,
                "Looping",
                "Hint: think about what helps you repeat the same action for every item in a list.",
                "Solution: Question 1 = A) A loop. Question 2 = B) They let you process each item without repeating the same code.",
                IsLoopingQ1Correct,
                IsLoopingQ2Correct,
                "Correct! Loops are useful because they let you go through list items one by one.",
                "Not quite yet. Think about what repeats code for each item in a list.",
                "Lesson complete!");

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
            Normalize(vm);

            await HandleCommonActionsAsync(
                vm,
                actionType,
                "Searching",
                "Hint: searching means checking items until you find the one you want.",
                "Solution: Question 1 = C) Checking items to find a target value. Question 2 = A) Use a loop and compare each item with the target.",
                IsSearchingQ1Correct,
                IsSearchingQ2Correct,
                "Correct! Searching a list means checking items to find a target value.",
                "Not quite yet. Think about checking each item one by one until there is a match.",
                "Lesson complete!");

            return View(vm);
        }

        public IActionResult Index()
        {
            return View();
        }

        private static void Normalize(dynamic vm)
        {
            vm.UserAnswer = vm.UserAnswer?.Trim() ?? string.Empty;
            vm.ExplanationAnswer = vm.ExplanationAnswer?.Trim() ?? string.Empty;
        }

        private async Task HandleCommonActionsAsync(
            dynamic vm,
            string actionType,
            string actionName,
            string hintMessage,
            string solutionMessage,
            Func<string, bool> firstQuestionChecker,
            Func<string, bool> secondQuestionChecker,
            string successMessage,
            string retryMessage,
            string completionMessage)
        {
            if (actionType == "hint")
            {
                vm.ShowHint = true;
                vm.ShowSolution = false;
                vm.FeedbackMessage = hintMessage;
                return;
            }

            if (actionType == "solution")
            {
                vm.ShowHint = false;
                vm.ShowSolution = true;
                vm.FeedbackMessage = solutionMessage;
                return;
            }

            bool firstCorrect = firstQuestionChecker(vm.UserAnswer);
            bool secondCorrect = secondQuestionChecker(vm.ExplanationAnswer);
            bool allCorrect = firstCorrect && secondCorrect;

            vm.IsCorrect = allCorrect;
            vm.ExplanationCorrect = secondCorrect;
            vm.ShowHint = false;
            vm.ShowSolution = false;
            vm.ExplanationFeedback = secondCorrect ? "Correct!" : "Try the second question again.";

            if (actionType == "submit")
            {
                if (allCorrect)
                {
                    await SaveLessonProgressAsync(actionName);
                    vm.FeedbackMessage = completionMessage;
                }
                else
                {
                    vm.FeedbackMessage = "Please answer both multiple-choice questions correctly before marking the lesson complete.";
                }

                return;
            }

            vm.FeedbackMessage = allCorrect ? successMessage : retryMessage;
        }

        private async Task<bool> SaveLessonProgressAsync(string actionName)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserID");

            if (!Guid.TryParse(userIdValue, out var userId))
            {
                return false;
            }

            var lesson = await _context.Lessons
                .FirstOrDefaultAsync(l => l.ControllerName == "UnitFive" && l.ActionName == actionName && l.IsPublished);

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

        private static bool IsListsQ1Correct(string answer) => answer == "B";
        private static bool IsListsQ2Correct(string answer) => answer == "A";

        private static bool IsAccessingQ1Correct(string answer) => answer == "C";
        private static bool IsAccessingQ2Correct(string answer) => answer == "B";

        private static bool IsLoopingQ1Correct(string answer) => answer == "A";
        private static bool IsLoopingQ2Correct(string answer) => answer == "B";

        private static bool IsSearchingQ1Correct(string answer) => answer == "C";
        private static bool IsSearchingQ2Correct(string answer) => answer == "A";
    }
}