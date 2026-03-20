using Microsoft.AspNetCore.Mvc;
using DotNetCoreSqlDb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using DotNetCoreSqlDb.Data;
using DotNetCoreSqlDb.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DotNetCoreSqlDb.Controllers
{
    [Authorize]
    public class UnitOneController : Controller
    {
        private readonly MyDatabaseContext _context;

        public UnitOneController(MyDatabaseContext context)
        {
            _context = context;
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

                // Save progress here the same way as Lesson 1
                // Find current user
                // Find lesson by title "Algorithms"
                // Insert/update UserLessonProgress
                // Save changes

                vm.FeedbackMessage = "Lesson complete! Your progress has been saved.";
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

                // Save progress here the same way as Lesson 1 and Lesson 2
                // Find current user
                // Find lesson by title "Decomposition"
                // Insert/update UserLessonProgress
                // Save changes

                vm.FeedbackMessage = "Lesson complete! Your progress has been saved.";
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

            var correctPb = string.Join("|", new[]
            {
                "Put bread slices on plate",
                "Spread peanut butter on the other slice",
                "Spread jam on one slice",
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

            if (actionType == "checkIdentify")
            {
                bool selectionsCorrect =
                    vm.RecipeSelected &&
                    !vm.GroceryListSelected &&
                    vm.GpsSelected &&
                    !vm.PhotoSelected;

                bool explanationsPresent =
                    vm.RecipeExplanation.Length >= 8 &&
                    vm.GroceryListExplanation.Length >= 8 &&
                    vm.GpsExplanation.Length >= 8 &&
                    vm.PhotoExplanation.Length >= 8;

                bool identifyCorrect = selectionsCorrect && explanationsPresent;

                vm.IdentifyCorrect = identifyCorrect;
                vm.CurrentStep = 3;

                if (!identifyCorrect)
                {
                    vm.IsCorrect = false;
                    vm.IdentifyFeedback = "Check both your selections and your explanations.";
                    vm.FeedbackMessage = "Finish Question 3 correctly to complete the lesson.";
                    return View(vm);
                }

                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!Guid.TryParse(userIdClaim, out var userId))
                {
                    vm.IsCorrect = false;
                    vm.IdentifyFeedback = "Your answer was correct, but your user session could not be matched.";
                    vm.FeedbackMessage = "Could not save lesson progress.";
                    return View(vm);
                }

                // Adjust this lookup if you prefer finding by slug/title instead.
                var lesson = await _context.Lessons
                    .FirstOrDefaultAsync(l => l.Title == "What Is Computer Science?");

                if (lesson == null)
                {
                    vm.IsCorrect = false;
                    vm.IdentifyFeedback = "Your answer was correct, but this lesson could not be found in the database.";
                    vm.FeedbackMessage = "Could not save lesson progress.";
                    return View(vm);
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

                vm.IsCorrect = true;
                vm.IdentifyFeedback = "Excellent. You correctly identified which examples are algorithms and explained your reasoning.";
                vm.FeedbackMessage = "Lesson complete! Your progress has been saved.";

                return View(vm);
            }

            vm.CurrentStep = 0;
            return View(vm);
        }
    }
}