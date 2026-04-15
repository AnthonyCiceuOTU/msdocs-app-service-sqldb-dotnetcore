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
            ViewBag.ActiveStep = 0;
            return View(new ListsViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Lists(ListsViewModel vm, string actionType, int activeStep = 1)
        {
            Normalize(vm);

            await HandleTwoStepLessonAsync(
                vm,
                actionType,
                activeStep,
                "Lists",
                "list",
                "Hint: a list stores several values together in one variable.",
                "Solution: Fill in the blank = list",
                "B",
                "Hint: choose the option that says a list can store many related values together.",
                "Solution: Question 2 = B) Storing multiple related values in one variable.",
                "Correct! A list stores multiple values together.",
                "Not quite yet. Think about what a list is used for.",
                "Correct! A list is used for storing multiple related values in one variable.",
                "Not quite yet. Pick the answer that describes storing many values together.",
                "Lesson complete!");

            ViewBag.ActiveStep = activeStep;
            return View(vm);
        }

        [HttpGet]
        public IActionResult Accessing()
        {
            ViewBag.ActiveStep = 0;
            return View(new AccessingViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Accessing(AccessingViewModel vm, string actionType, int activeStep = 1)
        {
            Normalize(vm);

            await HandleTwoStepLessonAsync(
                vm,
                actionType,
                activeStep,
                "Accessing",
                "index",
                "Hint: the position number of an item in a list is called an index.",
                "Solution: Fill in the blank = index",
                "C",
                "Hint: most programming languages start counting positions at 0.",
                "Solution: Question 2 = C) scores[0]",
                "Correct! The position of an item in a list is called an index.",
                "Not quite yet. Think about the word used for a list position.",
                "Correct! The first item is usually accessed with index 0.",
                "Not quite yet. Remember that the first position is usually 0.",
                "Lesson complete!");

            ViewBag.ActiveStep = activeStep;
            return View(vm);
        }

        [HttpGet]
        public IActionResult Looping()
        {
            ViewBag.ActiveStep = 0;
            return View(new LoopingViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Looping(LoopingViewModel vm, string actionType, int activeStep = 1)
        {
            Normalize(vm);

            await HandleTwoStepLessonAsync(
                vm,
                actionType,
                activeStep,
                "Looping",
                "loop",
                "Hint: the structure used to repeat code for every item is called a loop.",
                "Solution: Fill in the blank = loop",
                "A",
                "Hint: choose the answer about repeating code for each item in the list.",
                "Solution: Question 2 = A) They repeat an action for each item in the list.",
                "Correct! A loop repeats code for each item.",
                "Not quite yet. Think about the word for repeated code.",
                "Correct! Loops help you repeat an action for each item in a list.",
                "Not quite yet. Choose the answer about repeating actions for each item.",
                "Lesson complete!");

            ViewBag.ActiveStep = activeStep;
            return View(vm);
        }

        [HttpGet]
        public IActionResult Searching()
        {
            ViewBag.ActiveStep = 0;
            return View(new SearchingViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Searching(SearchingViewModel vm, string actionType, int activeStep = 1)
        {
            Normalize(vm);

            await HandleTwoStepLessonAsync(
                vm,
                actionType,
                activeStep,
                "Searching",
                "target",
                "Hint: when searching, you are trying to find a target value.",
                "Solution: Fill in the blank = target",
                "A",
                "Hint: searching usually means checking each item until you find a match.",
                "Solution: Question 2 = A) Check each item until the target is found.",
                "Correct! The value you are trying to find is called the target.",
                "Not quite yet. Think about the word for the value you want to find.",
                "Correct! Searching means checking items until the target is found.",
                "Not quite yet. Choose the answer about checking each item one by one.",
                "Lesson complete!");

            ViewBag.ActiveStep = activeStep;
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

        private async Task HandleTwoStepLessonAsync(
            dynamic vm,
            string actionType,
            int activeStep,
            string actionName,
            string fillBlankAnswer,
            string step1Hint,
            string step1Solution,
            string step2CorrectOption,
            string step2Hint,
            string step2Solution,
            string step1Success,
            string step1Retry,
            string step2Success,
            string step2Retry,
            string completionMessage)
        {
            bool step1CorrectNow = string.Equals(vm.UserAnswer, fillBlankAnswer, StringComparison.OrdinalIgnoreCase);
            bool step2CorrectNow = string.Equals(vm.ExplanationAnswer, step2CorrectOption, StringComparison.OrdinalIgnoreCase);

            if (actionType == "hint")
            {
                vm.ShowHint = true;
                vm.ShowSolution = false;

                if (activeStep == 2)
                {
                    vm.ExplanationFeedback = step2Hint;
                }
                else
                {
                    vm.FeedbackMessage = step1Hint;
                }

                return;
            }

            if (actionType == "solution")
            {
                vm.ShowHint = false;
                vm.ShowSolution = true;

                if (activeStep == 2)
                {
                    vm.ExplanationFeedback = step2Solution;
                }
                else
                {
                    vm.FeedbackMessage = step1Solution;
                }

                return;
            }

            vm.ShowHint = false;
            vm.ShowSolution = false;

            if (actionType == "checkStep1")
            {
                vm.IsCorrect = step1CorrectNow;
                vm.FeedbackMessage = step1CorrectNow ? step1Success : step1Retry;
                return;
            }

            if (actionType == "checkStep2")
            {
                vm.IsCorrect = step1CorrectNow || vm.IsCorrect == true;
                vm.ExplanationCorrect = step2CorrectNow;
                vm.ExplanationFeedback = step2CorrectNow ? step2Success : step2Retry;
                return;
            }

            if (actionType == "submit")
            {
                bool finalStep1 = step1CorrectNow || vm.IsCorrect == true;
                bool finalStep2 = step2CorrectNow || vm.ExplanationCorrect == true;

                vm.IsCorrect = finalStep1;
                vm.ExplanationCorrect = finalStep2;

                if (finalStep1 && finalStep2)
                {
                    await SaveLessonProgressAsync(actionName);
                    vm.ExplanationFeedback = completionMessage;
                }
                else
                {
                    vm.ExplanationFeedback = "Please complete both steps correctly before marking the lesson complete.";
                }
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
    }
}
