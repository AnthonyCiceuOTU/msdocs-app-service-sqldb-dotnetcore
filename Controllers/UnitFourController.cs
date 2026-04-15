using DotNetCoreSqlDb.Data;
using DotNetCoreSqlDb.Models;
using DotNetCoreSqlDb.Models.AI;
using DotNetCoreSqlDb.Services;
using DotNetCoreSqlDb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DotNetCoreSqlDb.Controllers
{
    [Authorize]
    public class UnitFourController : Controller
    {
        private readonly MyDatabaseContext _context;
        private readonly IAiShortAnswerGrader _aiShortAnswerGrader;
        private readonly ILogger<UnitFourController> _logger;
        private static readonly Dictionary<string, int> UnitFourLessonOrder = new(StringComparer.OrdinalIgnoreCase)
        {
            ["Counters"] = 1,
            ["ForLoops"] = 2,
            ["LoopErrors"] = 3,
            ["WhileLoops"] = 4,
            ["WhyLoops"] = 5
        };

        public UnitFourController(
            MyDatabaseContext context,
            IAiShortAnswerGrader aiShortAnswerGrader,
            ILogger<UnitFourController> logger)
        {
            _context = context;
            _aiShortAnswerGrader = aiShortAnswerGrader;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult WhyLoops()
        {
            return View(new WhyLoopsViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> WhyLoops(WhyLoopsViewModel vm, string actionType)
        {
            TrimAll(vm);

            if (actionType == "hint")
            {
                vm.ShowHint = true;
                vm.ShowSolution = false;
                ViewBag.ForceStep = 1;
                return View(vm);
            }

            if (actionType == "solution")
            {
                vm.ShowHint = false;
                vm.ShowSolution = true;
                ViewBag.ForceStep = 1;
                return View(vm);
            }

            if (actionType == "checkExplanation")
            {
                EvaluateWhyLoopsExplanation(vm);
                ViewBag.ForceStep = 2;
                return View(vm);
            }

            if (actionType == "submit")
            {
                EvaluateWhyLoopsExplanation(vm);

                if (vm.ExplanationCorrect != true)
                {
                    vm.ExplanationFeedback = string.IsNullOrWhiteSpace(vm.ExplanationFeedback)
                        ? "Please check your explanation before submitting."
                        : vm.ExplanationFeedback;

                    ViewBag.ForceStep = 2;
                    return View(vm);
                }

                await SaveLessonProgressAsync("WhyLoops");
                return await RedirectToNextIncompleteUnitFourLessonOrLessonsAsync();
            }

            vm.ShowHint = false;
            vm.ShowSolution = false;

            vm.IsQ1Correct = vm.UserAnswer1.Equals("repeat code", StringComparison.OrdinalIgnoreCase)
                             || vm.UserAnswer1.Contains("repeat", StringComparison.OrdinalIgnoreCase);
            vm.Feedback1 = vm.IsQ1Correct == true
                ? "Correct!"
                : "Think about what loops help us do again and again.";

            vm.IsQ2Correct = vm.UserAnswer2.Equals("less code", StringComparison.OrdinalIgnoreCase)
                             || vm.UserAnswer2.Contains("less", StringComparison.OrdinalIgnoreCase)
                             || vm.UserAnswer2.Contains("shorter", StringComparison.OrdinalIgnoreCase);
            vm.Feedback2 = vm.IsQ2Correct == true
                ? "Correct!"
                : "Loops help reduce repeated code.";

            vm.IsQ3Correct = vm.UserAnswer3.Equals("loop", StringComparison.OrdinalIgnoreCase);
            vm.Feedback3 = vm.IsQ3Correct == true
                ? "Correct!"
                : "The missing word is loop.";

            vm.IsQ4Correct = vm.UserAnswer4.Equals("repeat a task", StringComparison.OrdinalIgnoreCase)
                             || vm.UserAnswer4.Contains("repeat", StringComparison.OrdinalIgnoreCase);
            vm.Feedback4 = vm.IsQ4Correct == true
                ? "Correct!"
                : "Think about what a loop does.";

            ViewBag.ForceStep = AllCorrect(vm) ? 2 : 1;
            return View(vm);
        }

        [HttpGet]
        public IActionResult WhileLoops()
        {
            return View(new WhileLoopsViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> WhileLoops(WhileLoopsViewModel vm, string actionType)
        {
            TrimAll(vm);

            if (actionType == "check")
            {
                EvaluateWhileLoopsStepOne(vm);
                ViewBag.ForceStep = 1;
                return View(vm);
            }

            if (actionType == "hintStep2")
            {
                vm.ShowHint = true;
                vm.ShowSolution = false;
                ViewBag.ForceStep = 2;
                return View(vm);
            }

            if (actionType == "solutionStep2")
            {
                vm.ShowHint = false;
                vm.ShowSolution = true;
                ViewBag.ForceStep = 2;
                return View(vm);
            }

            if (actionType == "hint")
            {
                vm.ShowHint = true;
                vm.ShowSolution = false;
                ViewBag.ForceStep = 1;
                return View(vm);
            }

            if (actionType == "solution")
            {
                vm.ShowHint = false;
                vm.ShowSolution = true;
                ViewBag.ForceStep = 1;
                return View(vm);
            }

            if (actionType == "checkStep2")
            {
                EvaluateWhileLoopsStepTwo(vm);
                ViewBag.ForceStep = 2;
                return View(vm);
            }

            if (actionType == "submit")
            {
                EvaluateWhileLoopsStepTwo(vm);

                if (vm.IsQ2Correct != true || vm.IsQ3Correct != true || vm.IsQ4Correct != true)
                {
                    ViewBag.ForceStep = 2;
                    return View(vm);
                }

                await SaveLessonProgressAsync("WhileLoops");
                return await RedirectToNextIncompleteUnitFourLessonOrLessonsAsync();
            }

            EvaluateWhileLoopsStepOne(vm);
            ViewBag.ForceStep = vm.IsQ1Correct == true ? 2 : 1;
            return View(vm);
        }

        [HttpGet]
        public IActionResult ForLoops()
        {
            return View(new ForLoopsViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForLoops(ForLoopsViewModel vm, string actionType)
        {
            TrimAll(vm);

            if (actionType == "check")
            {
                EvaluateForLoopsStepOne(vm);
                ViewBag.ForceStep = 1;
                return View(vm);
            }

            if (actionType == "hint")
            {
                vm.ShowHint = true;
                vm.ShowSolution = false;
                ViewBag.ForceStep = 1;
                return View(vm);
            }

            if (actionType == "solution")
            {
                vm.ShowHint = false;
                vm.ShowSolution = true;
                ViewBag.ForceStep = 1;
                return View(vm);
            }

            if (actionType == "checkStep2")
            {
                EvaluateForLoopsStepTwo(vm);
                ViewBag.ForceStep = 2;
                return View(vm);
            }

            if (actionType == "submit")
            {
                EvaluateForLoopsStepTwo(vm);

                if (vm.IsQ2Correct != true || vm.IsQ3Correct != true || vm.IsQ4Correct != true)
                {
                    ViewBag.ForceStep = 2;
                    return View(vm);
                }

                await SaveLessonProgressAsync("ForLoops");
                return await RedirectToNextIncompleteUnitFourLessonOrLessonsAsync();
            }

            EvaluateForLoopsStepOne(vm);
            ViewBag.ForceStep = vm.IsQ1Correct == true ? 2 : 1;
            return View(vm);
        }

        [HttpGet]
        public IActionResult Counters()
        {
            return View(new CountersViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Counters(CountersViewModel vm, string actionType)
        {
            vm.UserAnswer1 = vm.UserAnswer1?.Trim() ?? "";
            vm.UserAnswer2 = vm.UserAnswer2?.Trim() ?? "";
            vm.UserAnswer3 = vm.UserAnswer3?.Trim() ?? "";
            vm.UserAnswer4 = vm.UserAnswer4?.Trim() ?? "";
            vm.ExplanationAnswer = vm.ExplanationAnswer?.Trim() ?? "";
            vm.ExplanationFeedback = vm.ExplanationFeedback?.Trim() ?? "";

            if (actionType == "hint")
            {
                vm.ShowHint = true;
                vm.ShowSolution = false;
                ViewBag.ForceStep = 1;
                return View(vm);
            }

            if (actionType == "solution")
            {
                vm.ShowHint = false;
                vm.ShowSolution = true;
                ViewBag.ForceStep = 1;
                return View(vm);
            }

            if (actionType == "submit")
            {
                if (vm.ExplanationCorrect != true)
                {
                    vm.ExplanationFeedback = string.IsNullOrWhiteSpace(vm.ExplanationFeedback)
                        ? "Please check your explanation with AI before submitting."
                        : vm.ExplanationFeedback;

                    ViewBag.ForceStep = 2;
                    return View(vm);
                }

                await SaveLessonProgressAsync("Counters");
                return await RedirectToNextIncompleteUnitFourLessonOrLessonsAsync();
            }

            vm.ShowHint = false;
            vm.ShowSolution = false;

            vm.IsQ1Correct = vm.UserAnswer1.Equals("sum", StringComparison.OrdinalIgnoreCase);
            vm.Feedback1 = vm.IsQ1Correct == true ? "Correct!" : "Look for the variable storing the total.";

            vm.IsQ2Correct = vm.UserAnswer2.Equals("running total", StringComparison.OrdinalIgnoreCase) || vm.UserAnswer2.Contains("total", StringComparison.OrdinalIgnoreCase);
            vm.Feedback2 = vm.IsQ2Correct == true ? "Correct!" : "An accumulator keeps a running total.";

            vm.IsQ3Correct = vm.UserAnswer3.Equals("sum = sum + i", StringComparison.OrdinalIgnoreCase)
                             || vm.UserAnswer3.Equals("sum ← sum + i", StringComparison.OrdinalIgnoreCase)
                             || (vm.UserAnswer3.Contains("sum", StringComparison.OrdinalIgnoreCase) && vm.UserAnswer3.Contains("+", StringComparison.OrdinalIgnoreCase));
            vm.Feedback3 = vm.IsQ3Correct == true ? "Correct!" : "Look at the line that updates the total each loop.";

            vm.IsQ4Correct =
    vm.UserAnswer4.Equals("accumulator", StringComparison.OrdinalIgnoreCase)
    || vm.UserAnswer4.Contains("accumulator", StringComparison.OrdinalIgnoreCase);

vm.Feedback4 = vm.IsQ4Correct == true
    ? "Correct!"
    : "A variable that stores a running total is called an accumulator.";

            ViewBag.ForceStep = vm.IsQ1Correct == true && vm.IsQ2Correct == true && vm.IsQ3Correct == true && vm.IsQ4Correct == true ? 2 : 1;
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckCountersExplanation([FromForm] string explanationAnswer)
        {
            explanationAnswer = explanationAnswer?.Trim() ?? "";

            _logger.LogInformation("CheckCountersExplanation called. Explanation: {Explanation}", explanationAnswer);

            if (string.IsNullOrWhiteSpace(explanationAnswer))
            {
                return BadRequest(new { isCorrect = false, feedback = "Please enter an explanation first." });
            }

            try
            {
                var result = await _aiShortAnswerGrader.GradeAsync(new ShortAnswerEvaluationRequest
                {
                    QuestionText = "Explain why counters or accumulators are useful in programming.",
                    StudentAnswer = explanationAnswer,
                    ExpectedAnswer = "Counters and accumulators are useful because they help a program keep track of values in a loop, such as counting how many times something happens or keeping a running total.",
                    GradingRubric = """
                    To be correct, the answer should clearly show that:
                    1. A counter or accumulator keeps track of something while code repeats.
                    2. It may count occurrences or store a running total.
                    3. It is useful in loops or repeated steps.

                    Accept simple student wording such as:
                    - keeps track of a total
                    - counts how many times something happens
                    - updates a value in a loop
                    - stores a running total

                    Do not require advanced vocabulary.
                    Reject answers that are too vague or do not mention tracking/counting/totaling.
                    """
                });

                _logger.LogInformation("CheckCountersExplanation result. IsCorrect: {IsCorrect}, Score: {Score}, Feedback: {Feedback}", result.IsCorrect, result.Score, result.Feedback);
                return Json(new { isCorrect = result.IsCorrect, feedback = result.Feedback });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while checking Counters explanation.");
                return StatusCode(500, new { isCorrect = false, feedback = "We could not check your explanation right now. Please try again." });
            }
        }

        [HttpGet]
        public IActionResult LoopErrors()
        {
            return View(new LoopErrorsViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoopErrors(LoopErrorsViewModel vm, string actionType)
        {
            TrimAll(vm);

            if (actionType == "check")
            {
                EvaluateLoopErrorsStepOne(vm);
                ViewBag.ForceStep = 1;
                return View(vm);
            }

            if (actionType == "hint")
            {
                vm.ShowHint = true;
                vm.ShowSolution = false;
                ViewBag.ForceStep = 1;
                return View(vm);
            }

            if (actionType == "solution")
            {
                vm.ShowHint = false;
                vm.ShowSolution = true;
                ViewBag.ForceStep = 1;
                return View(vm);
            }

            if (actionType == "checkStep2")
            {
                EvaluateLoopErrorsStepTwo(vm);
                ViewBag.ForceStep = 2;
                return View(vm);
            }

            if (actionType == "submit")
            {
                EvaluateLoopErrorsStepTwo(vm);

                if (vm.IsQ2Correct != true || vm.IsQ3Correct != true || vm.IsQ4Correct != true)
                {
                    ViewBag.ForceStep = 2;
                    return View(vm);
                }

                await SaveLessonProgressAsync("LoopErrors");
                return await RedirectToNextIncompleteUnitFourLessonOrLessonsAsync();
            }

            EvaluateLoopErrorsStepOne(vm);
            ViewBag.ForceStep = vm.IsQ1Correct == true ? 2 : 1;
            return View(vm);
        }

        private static void EvaluateWhyLoopsExplanation(WhyLoopsViewModel vm)
        {
            vm.ExplanationCorrect =
                (
                    vm.ExplanationAnswer.Contains("repeat", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("again", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("multiple times", StringComparison.OrdinalIgnoreCase)
                ) &&
                (
                    vm.ExplanationAnswer.Contains("same code", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("less code", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("faster", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("efficient", StringComparison.OrdinalIgnoreCase)
                );

            vm.ExplanationFeedback = vm.ExplanationCorrect == true
                ? "Correct! Loops are useful because they repeat code without rewriting it."
                : "Try mentioning that loops repeat tasks and reduce repeated code.";
        }

        private static void EvaluateWhileLoopsStepOne(WhileLoopsViewModel vm)
        {
            vm.ShowHint = false;
            vm.ShowSolution = false;

            vm.IsQ1Correct =
                (
                    vm.UserAnswer1.Contains("condition", StringComparison.OrdinalIgnoreCase) &&
                    vm.UserAnswer1.Contains("true", StringComparison.OrdinalIgnoreCase)
                )
                || vm.UserAnswer1.Contains("repeats while its condition is true", StringComparison.OrdinalIgnoreCase)
                || vm.UserAnswer1.Contains("while the condition is true", StringComparison.OrdinalIgnoreCase);

            vm.Feedback1 = vm.IsQ1Correct == true
                ? "Correct!"
                : "Try mentioning that a WHILE loop repeats while its condition is true.";
        }

        private static void EvaluateWhileLoopsStepTwo(WhileLoopsViewModel vm)
        {
            vm.ShowHint = false;
            vm.ShowSolution = false;

            vm.IsQ2Correct = vm.UserAnswer2.Equals("condition", StringComparison.OrdinalIgnoreCase);
            vm.Feedback2 = vm.IsQ2Correct == true
                ? "Correct!"
                : "A WHILE loop is best when repetition depends on a condition.";

            vm.IsQ3Correct = vm.UserAnswer3.Equals("WHILE", StringComparison.OrdinalIgnoreCase);
            vm.Feedback3 = vm.IsQ3Correct == true
                ? "Correct!"
                : "The missing keyword is WHILE.";

            vm.IsQ4Correct = vm.UserAnswer4.Equals("false", StringComparison.OrdinalIgnoreCase);
            vm.Feedback4 = vm.IsQ4Correct == true
                ? "Correct!"
                : "A WHILE loop stops when its condition becomes false.";
        }

        private static void EvaluateForLoopsStepOne(ForLoopsViewModel vm)
        {
            vm.ShowHint = false;
            vm.ShowSolution = false;

            vm.IsQ1Correct = vm.UserAnswer1.Contains("1 to 10", StringComparison.OrdinalIgnoreCase)
                             || vm.UserAnswer1.Contains("1 through 10", StringComparison.OrdinalIgnoreCase)
                             || vm.UserAnswer1.Contains("prints numbers from 1 to 10", StringComparison.OrdinalIgnoreCase)
                             || vm.UserAnswer1.Contains("print numbers from 1 to 10", StringComparison.OrdinalIgnoreCase)
                             || (vm.UserAnswer1.Contains("1") && vm.UserAnswer1.Contains("10"));
            vm.Feedback1 = vm.IsQ1Correct == true
                ? "Correct!"
                : "Look at the range in the FOR loop.";
        }

        private static void EvaluateForLoopsStepTwo(ForLoopsViewModel vm)
        {
            vm.IsQ2Correct = vm.UserAnswer2.Equals("known", StringComparison.OrdinalIgnoreCase);
            vm.Feedback2 = vm.IsQ2Correct == true
                ? "Correct!"
                : "FOR loops are used when you know how many times to repeat.";

            vm.IsQ3Correct = vm.UserAnswer3.Equals("FOR", StringComparison.OrdinalIgnoreCase);
            vm.Feedback3 = vm.IsQ3Correct == true
                ? "Correct!"
                : "The missing keyword is FOR.";

            vm.IsQ4Correct = vm.UserAnswer4.Equals("5", StringComparison.OrdinalIgnoreCase);
            vm.Feedback4 = vm.IsQ4Correct == true
                ? "Correct!"
                : "FOR i = 1 TO 5 repeats 5 times.";
        }

        private static void EvaluateLoopErrorsStepOne(LoopErrorsViewModel vm)
        {
            vm.ShowHint = false;
            vm.ShowSolution = false;

            vm.IsQ1Correct = vm.UserAnswer1.Contains("infinite", StringComparison.OrdinalIgnoreCase);
            vm.Feedback1 = vm.IsQ1Correct == true ? "Correct!" : "One common error is a loop that never stops.";
        }

        private static void EvaluateLoopErrorsStepTwo(LoopErrorsViewModel vm)
        {
            vm.IsQ2Correct = vm.UserAnswer2.Equals("off-by-one", StringComparison.OrdinalIgnoreCase)
                             || vm.UserAnswer2.Equals("off by one", StringComparison.OrdinalIgnoreCase);
            vm.Feedback2 = vm.IsQ2Correct == true ? "Correct!" : "Another common loop error is off-by-one.";

            vm.IsQ3Correct = vm.UserAnswer3.Equals("runs forever", StringComparison.OrdinalIgnoreCase);
            vm.Feedback3 = vm.IsQ3Correct == true ? "Correct!" : "An infinite loop keeps running forever.";

            vm.IsQ4Correct = vm.UserAnswer4.Equals("one too many or one too few", StringComparison.OrdinalIgnoreCase);
            vm.Feedback4 = vm.IsQ4Correct == true ? "Correct!" : "Off-by-one means the loop runs one too many or one too few times.";
        }

        private static void TrimAll(UnitFourLessonViewModel vm)
        {
            vm.UserAnswer1 = vm.UserAnswer1?.Trim() ?? "";
            vm.UserAnswer2 = vm.UserAnswer2?.Trim() ?? "";
            vm.UserAnswer3 = vm.UserAnswer3?.Trim() ?? "";
            vm.UserAnswer4 = vm.UserAnswer4?.Trim() ?? "";
            vm.ExplanationAnswer = vm.ExplanationAnswer?.Trim() ?? "";
            vm.ExplanationFeedback = vm.ExplanationFeedback?.Trim() ?? "";
        }

        private static bool AllCorrect(UnitFourLessonViewModel vm)
        {
            return vm.IsQ1Correct == true && vm.IsQ2Correct == true && vm.IsQ3Correct == true && vm.IsQ4Correct == true;
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

        private async Task<IActionResult> RedirectToNextIncompleteUnitFourLessonOrLessonsAsync()
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("UserID");
            if (!Guid.TryParse(userIdValue, out var userId))
            {
                return RedirectToAction("Index", "Lessons");
            }

            var unitFourLessons = await _context.Lessons
                .Where(l => l.IsPublished && l.UnitId == 4)
                .ToListAsync();

            unitFourLessons = unitFourLessons
                .OrderBy(GetEffectiveLessonSortOrder)
                .ThenBy(l => l.Id)
                .ToList();

            if (!unitFourLessons.Any())
            {
                return RedirectToAction("Index", "Lessons");
            }

            var completedLessonIds = await _context.UserLessonProgresses
                .Where(p => p.UserId == userId && p.IsCompleted)
                .Select(p => p.LessonId)
                .ToListAsync();

            var nextIncomplete = unitFourLessons.FirstOrDefault(l => !completedLessonIds.Contains(l.Id));

            if (nextIncomplete == null)
            {
                return RedirectToAction("Index", "Lessons");
            }

            return RedirectToAction(nextIncomplete.ActionName, nextIncomplete.ControllerName);
        }

        private static int GetEffectiveLessonSortOrder(Lesson lesson)
        {
            if (UnitFourLessonOrder.TryGetValue(lesson.ActionName, out var order))
            {
                return order;
            }

            return lesson.SortOrder;
        }
    }
}
