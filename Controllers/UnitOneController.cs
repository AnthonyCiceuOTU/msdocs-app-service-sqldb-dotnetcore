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
    [Authorize]
    public class UnitOneController : Controller
    {
        private readonly MyDatabaseContext _context;
        private readonly IAiShortAnswerGrader _aiShortAnswerGrader;
        private readonly ILogger<UnitOneController> _logger;

        public UnitOneController(
            MyDatabaseContext context,
            IAiShortAnswerGrader aiShortAnswerGrader,
            ILogger<UnitOneController> logger)
        {
            _context = context;
            _aiShortAnswerGrader = aiShortAnswerGrader;
            _logger = logger;
}

        [HttpGet]
        public IActionResult Algorithms()
        {
            return View(new AlgorithmsViewModel
            {
                CurrentStep = 0
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Algorithms(AlgorithmsViewModel vm, string actionType)
        {
            vm.UserAnswer1 = vm.UserAnswer1?.Trim() ?? "";
            vm.UserAnswer2 = vm.UserAnswer2?.Trim() ?? "";
            vm.ExplanationAnswer = vm.ExplanationAnswer?.Trim() ?? "";

            bool codeCorrect =
                (vm.UserAnswer1.Equals("price1", StringComparison.OrdinalIgnoreCase) &&
                 vm.UserAnswer2.Equals("price2", StringComparison.OrdinalIgnoreCase))
                ||
                (vm.UserAnswer1.Equals("price2", StringComparison.OrdinalIgnoreCase) &&
                 vm.UserAnswer2.Equals("price1", StringComparison.OrdinalIgnoreCase));

            if (actionType == "check")
            {
                vm.IsCorrect = codeCorrect;
                vm.CurrentStep = 1;
                vm.FeedbackMessage = codeCorrect
                    ? "Correct! The algorithm adds the two item prices together."
                    : "Not quite. Try using the two variables already defined above.";

                return View(vm);
            }

            if (actionType == "checkExplanation")
            {
                vm.IsCorrect = true;
                vm.CurrentStep = 2;

                bool explanationCorrect =
                    vm.ExplanationAnswer.Contains("price1", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("price2", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("variable", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("price", StringComparison.OrdinalIgnoreCase) ||
                    vm.ExplanationAnswer.Contains("total", StringComparison.OrdinalIgnoreCase);

                explanationCorrect = explanationCorrect && vm.ExplanationAnswer.Length >= 12;

                vm.ExplanationCorrect = explanationCorrect;
                vm.ExplanationFeedback = explanationCorrect
                    ? "Good explanation. You showed why the algorithm works."
                    : "Add a little more detail about why using the stored price variables makes the algorithm correct.";

                return View(vm);
            }

            if (actionType == "submit")
            {
                vm.IsCorrect = true;
                vm.ExplanationCorrect = true;
                vm.CurrentStep = 2;

                var saved = await SaveLessonProgressAsync("Algorithms");
                vm.FeedbackMessage = saved
                    ? "Lesson complete! Your progress has been saved."
                    : "Your answers were submitted, but progress could not be saved.";

                return View(vm);
            }

            vm.CurrentStep = 0;
            return View(vm);
        }

        [HttpGet]
        public IActionResult Decomposition()
        {
            return View(new DecompositionViewModel
            {
                CurrentStep = 0
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Decomposition(DecompositionViewModel vm, string actionType)
        {
            vm.UserAnswer1 = vm.UserAnswer1?.Trim() ?? "";
            vm.UserAnswer2 = vm.UserAnswer2?.Trim() ?? "";
            vm.UserAnswer3 = vm.UserAnswer3?.Trim() ?? "";
            vm.ExplanationAnswer = vm.ExplanationAnswer?.Trim() ?? "";

            bool firstTaskCorrect = vm.TaskOrder == "Wake up|Get dressed|Eat breakfast";

            if (actionType == "check")
            {
                vm.IsCorrect = firstTaskCorrect;
                vm.CurrentStep = 1;
                vm.FeedbackMessage = firstTaskCorrect
                    ? "Correct! You broke the morning routine into smaller steps."
                    : "Not quite. Think about what usually happens before leaving for school.";

                return View(vm);
            }

            if (actionType == "checkExplanation")
            {
                vm.IsCorrect = true;
                vm.CurrentStep = 2;

                bool explanationCorrect =
                    vm.ExplanationAnswer.Length >= 12 &&
                    (
                        vm.ExplanationAnswer.Contains("smaller", StringComparison.OrdinalIgnoreCase) ||
                        vm.ExplanationAnswer.Contains("steps", StringComparison.OrdinalIgnoreCase) ||
                        vm.ExplanationAnswer.Contains("easier", StringComparison.OrdinalIgnoreCase) ||
                        vm.ExplanationAnswer.Contains("manage", StringComparison.OrdinalIgnoreCase) ||
                        vm.ExplanationAnswer.Contains("break", StringComparison.OrdinalIgnoreCase)
                    );

                vm.ExplanationCorrect = explanationCorrect;
                vm.ExplanationFeedback = explanationCorrect
                    ? "Good explanation. You showed how decomposition makes a problem easier to manage."
                    : "Add a little more detail about how smaller steps help solve a larger task.";

                return View(vm);
            }

            if (actionType == "submit")
            {
                vm.IsCorrect = true;
                vm.ExplanationCorrect = true;
                vm.CurrentStep = 2;

                var saved = await SaveLessonProgressAsync("Decomposition");
                vm.FeedbackMessage = saved
                    ? "Lesson complete! Your progress has been saved."
                    : "Your answers were submitted, but progress could not be saved.";

                return View(vm);
            }

            vm.CurrentStep = 0;
            return View(vm);
        }

        [HttpGet]
        public IActionResult WhatIsComputerScience()
        {
            var vm = new ComputerScienceViewModel
            {
                PbSandwichOrder = "Put bread slices on plate|Spread jam on one slice|Spread peanut butter on the other slice|Press the slices together",
                CardSortOrder = "Shuffle the deck|Look at the first two cards|Compare their values|Swap them if needed|Repeat until the deck is in order|Stop when no swaps are needed",
                CurrentStep = 0
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> WhatIsComputerScience(ComputerScienceViewModel vm, string actionType)
        {
            vm.PbSandwichOrder = vm.PbSandwichOrder?.Trim() ?? "";
            vm.CardSortOrder = vm.CardSortOrder?.Trim() ?? "";

            vm.RecipeExplanation = vm.RecipeExplanation?.Trim() ?? "";
            vm.GroceryListExplanation = vm.GroceryListExplanation?.Trim() ?? "";
            vm.GpsExplanation = vm.GpsExplanation?.Trim() ?? "";
            vm.PhotoExplanation = vm.PhotoExplanation?.Trim() ?? "";
            vm.ExplanationFeedback = vm.ExplanationFeedback?.Trim() ?? "";

            var correctPb = string.Join("|", new[]
            {
                "Put bread slices on plate",
                "Spread jam on one slice",
                "Spread peanut butter on the other slice",
                "Press the slices together"
            });

            var correctCards = string.Join("|", new[]
            {
                "Shuffle the deck",
                "Look at the first two cards",
                "Compare their values",
                "Swap them if needed",
                "Repeat until the deck is in order",
                "Stop when no swaps are needed"
            });

            if (actionType == "checkPb")
            {
                bool pbCorrect = vm.PbSandwichOrder.Equals(correctPb, StringComparison.Ordinal);

                vm.PbCorrect = pbCorrect;
                vm.PbFeedback = pbCorrect
                    ? "Correct. Great job ordering the sandwich steps."
                    : "Not quite. Review the order carefully and try again.";

                vm.CurrentStep = 1;
                vm.IsCorrect = null;
                return View(vm);
            }

            if (actionType == "goStep2")
            {
                vm.PbCorrect = true;
                vm.CurrentStep = 2;
                return View(vm);
            }

            if (actionType == "checkCards")
            {
                bool cardCorrect = vm.CardSortOrder.Equals(correctCards, StringComparison.Ordinal);

                vm.CardCorrect = cardCorrect;
                vm.CardFeedback = cardCorrect
                    ? "Correct. You arranged the sorting steps in a logical order."
                    : "Not quite. Focus on the compare-and-swap pattern and try again.";

                vm.CurrentStep = 2;
                vm.IsCorrect = null;
                return View(vm);
            }

            if (actionType == "goStep3")
            {
                vm.PbCorrect = true;
                vm.CardCorrect = true;
                vm.CurrentStep = 3;
                return View(vm);
            }

            if (actionType == "submit")
            {
                vm.CurrentStep = 3;
                vm.PbCorrect = true;
                vm.CardCorrect = true;

                bool selectionsCorrect =
                    vm.RecipeSelected &&
                    !vm.GroceryListSelected &&
                    vm.GpsSelected &&
                    !vm.PhotoSelected;

                vm.IdentifyCorrect = selectionsCorrect;

                if (!selectionsCorrect)
                {
                    vm.IsCorrect = false;
                    vm.IdentifyFeedback = "Your selections are not quite right. A recipe and GPS directions are algorithms, but a grocery list and a photo are not.";
                    vm.FeedbackMessage = "Fix Question 3 before submitting the lesson.";
                    return View(vm);
                }

                if (vm.ExplanationCorrect != true)
                {
                    vm.IsCorrect = false;
                    vm.IdentifyFeedback = "Please check your explanations with AI before submitting.";
                    vm.FeedbackMessage = "Complete the AI explanation check before finishing the lesson.";
                    return View(vm);
                }

                var saved = await SaveLessonProgressAsync("WhatIsComputerScience");

                vm.IsCorrect = saved;
                /*vm.IdentifyFeedback = saved
                    ? "Excellent. You correctly identified which examples are algorithms and explained your reasoning."
                    : "Your answers were correct, but the lesson progress could not be saved.";*/
                vm.FeedbackMessage = saved
                    ? "Lesson complete! Your progress has been saved."
                    : "Could not save lesson progress.";

                return View(vm);
            }

            vm.CurrentStep = 0;
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckWhatIsComputerScienceExplanation(
            [FromForm] bool recipeSelected,
            [FromForm] bool groceryListSelected,
            [FromForm] bool gpsSelected,
            [FromForm] bool photoSelected,
            [FromForm] string recipeExplanation,
            [FromForm] string groceryListExplanation,
            [FromForm] string gpsExplanation,
            [FromForm] string photoExplanation)
        {
            recipeExplanation = recipeExplanation?.Trim() ?? "";
            groceryListExplanation = groceryListExplanation?.Trim() ?? "";
            gpsExplanation = gpsExplanation?.Trim() ?? "";
            photoExplanation = photoExplanation?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(recipeExplanation) ||
                string.IsNullOrWhiteSpace(groceryListExplanation) ||
                string.IsNullOrWhiteSpace(gpsExplanation) ||
                string.IsNullOrWhiteSpace(photoExplanation))
            {
                return BadRequest(new
                {
                    isCorrect = false,
                    feedback = "Please answer all four explanation boxes first."
                });
            }

            try
            {
                var studentAnswer = $"""
                Recipe selected: {recipeSelected}
                Recipe explanation: {recipeExplanation}

                Grocery list selected: {groceryListSelected}
                Grocery list explanation: {groceryListExplanation}

                GPS selected: {gpsSelected}
                GPS explanation: {gpsExplanation}

                Photo selected: {photoSelected}
                Photo explanation: {photoExplanation}
                """;

                var result = await _aiShortAnswerGrader.GradeAsync(new ShortAnswerEvaluationRequest
                {
                    QuestionText = "Identify which examples are algorithms and explain why or why not.",
                    StudentAnswer = studentAnswer,
                    ExpectedAnswer = """
                    Correct understanding:
                    - A recipe for pancakes is an algorithm because it gives step-by-step instructions.
                    - A grocery list is not an algorithm because it is only a collection of items, not ordered instructions for solving a task.
                    - GPS directions are an algorithm because they provide ordered steps or directions to reach a destination.
                    - A photo is not an algorithm because it is just an image and does not provide instructions or a process.
                    """,
                    GradingRubric = """
                    To be correct, the student should show these ideas clearly:

                    1. Recipe:
                    - Should be identified as an algorithm.
                    - Reason should mention step-by-step instructions or ordered steps.

                    2. Grocery list:
                    - Should be identified as NOT an algorithm.
                    - Reason should mention that it is only a list of items, not instructions or a process.

                    3. GPS directions:
                    - Should be identified as an algorithm.
                    - Reason should mention directions, ordered steps, or instructions to reach a destination.

                    4. Photo:
                    - Should be identified as NOT an algorithm.
                    - Reason should mention that it is only an image/picture and not a set of steps.

                    Accept simple student wording.
                    Do not require advanced vocabulary.
                    Minor spelling/grammar issues are fine.
                    Reject answers that get the algorithm/non-algorithm choices wrong or give explanations that are too vague.
                    """
                });

                _logger.LogInformation(
                    "CheckWhatIsComputerScienceExplanation result. IsCorrect: {IsCorrect}, Score: {Score}, Feedback: {Feedback}",
                    result.IsCorrect,
                    result.Score,
                    result.Feedback);

                return Json(new
                {
                    isCorrect = result.IsCorrect,
                    feedback = result.Feedback
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while checking WhatIsComputerScience explanations.");
                return StatusCode(500, new
                {
                    isCorrect = false,
                    feedback = "We could not check your explanations right now. Please try again."
                });
            }
        }

        private async Task<bool> SaveLessonProgressAsync(string actionName)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return false;
            }

            var lesson = await _context.Lessons
                .FirstOrDefaultAsync(l => l.ControllerName == "UnitOne" && l.ActionName == actionName && l.IsPublished);

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