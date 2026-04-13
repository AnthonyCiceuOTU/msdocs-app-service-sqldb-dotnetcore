using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DotNetCoreSqlDb.Data;
using DotNetCoreSqlDb.Models;
using DotNetCoreSqlDb.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNetCoreSqlDb.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly MyDatabaseContext _context;

        public ProfileController(MyDatabaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(username))
            {
                return RedirectToAction("NotAuthorized", "Login");
            }

            Guid? currentUserId = null;
            if (Guid.TryParse(userIdClaim, out var parsedId))
            {
                currentUserId = parsedId;
            }

            User? user = null;

            if (currentUserId.HasValue)
            {
                user = await _context.User
                    .FirstOrDefaultAsync(u => u.ID == currentUserId.Value);
            }

            if (user == null)
            {
                user = await _context.User
                    .FirstOrDefaultAsync(u => u.Username == username);
            }

            if (user == null)
            {
                return RedirectToAction("NotAuthorized", "Login");
            }

            var today = DateTime.Today;
            var endDate = today;
            var rawStart = today.AddDays(-364);
            var startDate = rawStart.AddDays(-(int)rawStart.DayOfWeek);

            var signInLogs = await _context.SignInLog
                .Where(x => x.UserName == user.Username)
                .OrderBy(x => x.DateTime)
                .ToListAsync();

            var calendarLogs = signInLogs
                .Where(x => x.DateTime.Date >= startDate.Date && x.DateTime.Date <= endDate.Date)
                .ToList();

            var grouped = calendarLogs
                .GroupBy(x => x.DateTime.Date)
                .ToDictionary(g => g.Key, g => g.Count());

            var monthLabels = new List<CalendarMonthLabel>();
            DateTime cursor = startDate.Date;
            string? lastMonth = null;
            int weekIndex = 0;

            while (cursor <= endDate.Date)
            {
                var monthName = cursor.ToString("MMM", CultureInfo.InvariantCulture);

                if (lastMonth != monthName && cursor.Day <= 7)
                {
                    monthLabels.Add(new CalendarMonthLabel
                    {
                        Label = monthName,
                        WeekIndex = weekIndex
                    });

                    lastMonth = monthName;
                }

                cursor = cursor.AddDays(7);
                weekIndex++;
            }

            var publishedUnits = await _context.Units
                .Include(u => u.Lessons.Where(l => l.IsPublished))
                .OrderBy(u => u.SortOrder)
                .ThenBy(u => u.Id)
                .ToListAsync();

            var totalLessons = publishedUnits.Sum(u => u.Lessons.Count);

            var progressList = await _context.UserLessonProgresses
                .Include(p => p.Lesson)
                    .ThenInclude(l => l.Unit)
                .Where(p => p.UserId == user.ID && p.Lesson.IsPublished)
                .ToListAsync();

            var completedProgress = progressList
                .Where(p => p.IsCompleted)
                .ToList();

            var completedLessonIds = completedProgress
                .Select(p => p.LessonId)
                .Distinct()
                .ToHashSet();

            var completedLessons = completedLessonIds.Count;
            var overallProgressPercent = totalLessons == 0
                ? 0
                : Math.Round((double)completedLessons / totalLessons * 100, 1);

            var allLessonsOrdered = publishedUnits
                .SelectMany(u => u.Lessons.OrderBy(l => l.SortOrder).ThenBy(l => l.Id))
                .ToList();

            var nextLesson = allLessonsOrdered
                .FirstOrDefault(l => !completedLessonIds.Contains(l.Id));

            var unitProgress = publishedUnits
                .Select(u =>
                {
                    var unitTotal = u.Lessons.Count;
                    var unitCompleted = u.Lessons.Count(l => completedLessonIds.Contains(l.Id));
                    var percent = unitTotal == 0 ? 0 : Math.Round((double)unitCompleted / unitTotal * 100, 1);

                    return new ProfileUnitProgressViewModel
                    {
                        UnitId = u.Id,
                        UnitTitle = u.Title,
                        TotalLessons = unitTotal,
                        CompletedLessons = unitCompleted,
                        ProgressPercent = percent,
                        IsComplete = unitTotal > 0 && unitCompleted == unitTotal
                    };
                })
                .ToList();

            var completedUnits = unitProgress.Count(u => u.IsComplete);
            var distinctUnitsStarted = progressList
                .Select(p => p.Lesson.UnitId)
                .Distinct()
                .Count();

            var recentCompletedLessons = completedProgress
                .Where(p => p.CompletedAtUtc.HasValue)
                .OrderByDescending(p => p.CompletedAtUtc)
                .Take(5)
                .Select(p => new ProfileLessonSummaryViewModel
                {
                    LessonId = p.LessonId,
                    UnitId = p.Lesson.UnitId,
                    UnitTitle = p.Lesson.Unit.Title,
                    LessonTitle = p.Lesson.Title,
                    CompletedAtUtc = p.CompletedAtUtc
                })
                .ToList();

            var nextRecommendedLesson = nextLesson == null
                ? null
                : new ProfileLessonSummaryViewModel
                {
                    LessonId = nextLesson.Id,
                    UnitId = nextLesson.UnitId,
                    UnitTitle = nextLesson.Unit.Title,
                    LessonTitle = nextLesson.Title
                };

            var distinctSignInDates = signInLogs
                .Select(x => x.DateTime.Date)
                .Distinct()
                .OrderByDescending(d => d)
                .ToList();

            int currentStreakDays = CalculateStreak(distinctSignInDates, today);

            bool unitOneComplete = unitProgress.Any(u => u.UnitId == 1 && u.IsComplete);
            bool halfwayDone = overallProgressPercent >= 50;
            bool explorer = distinctUnitsStarted >= 2;
            bool consistentLearner = distinctSignInDates.Count >= 3;
            bool dedicationWeek = currentStreakDays >= 7;

            var achievements = new List<ProfileAchievementViewModel>
            {
                new ProfileAchievementViewModel
                {
                    Title = "First Step",
                    Description = "Complete your first lesson.",
                    IsUnlocked = completedLessons >= 1,
                    Icon = "★"
                },
                new ProfileAchievementViewModel
                {
                    Title = "Getting Started",
                    Description = "Complete 3 lessons.",
                    IsUnlocked = completedLessons >= 3,
                    Icon = "✓"
                },
                new ProfileAchievementViewModel
                {
                    Title = "Momentum Builder",
                    Description = "Complete 5 lessons.",
                    IsUnlocked = completedLessons >= 5,
                    Icon = "⬈"
                },
                new ProfileAchievementViewModel
                {
                    Title = "Unit One Complete",
                    Description = "Finish all lessons in Unit 1.",
                    IsUnlocked = unitOneComplete,
                    Icon = "🏁"
                },
                new ProfileAchievementViewModel
                {
                    Title = "Halfway There",
                    Description = "Reach 50% overall course progress.",
                    IsUnlocked = halfwayDone,
                    Icon = "◐"
                },
                new ProfileAchievementViewModel
                {
                    Title = "Course Explorer",
                    Description = "Start lessons in at least 2 different units.",
                    IsUnlocked = explorer,
                    Icon = "🧭"
                },
                new ProfileAchievementViewModel
                {
                    Title = "Consistent Learner",
                    Description = "Sign in on 3 different days.",
                    IsUnlocked = consistentLearner,
                    Icon = "📅"
                },
                new ProfileAchievementViewModel
                {
                    Title = "Dedication Week",
                    Description = "Build a 7-day sign-in streak.",
                    IsUnlocked = dedicationWeek,
                    Icon = "🔥"
                }
            };

            var vm = new ProfileViewModel
            {
                UserId = user.ID,
                Username = user.Username,
                TotalSignIns = signInLogs.Count,
                FirstSignIn = signInLogs.FirstOrDefault()?.DateTime,
                LastSignIn = signInLogs.LastOrDefault()?.DateTime,
                CurrentStreakDays = currentStreakDays,
                TotalLessons = totalLessons,
                CompletedLessons = completedLessons,
                OverallProgressPercent = overallProgressPercent,
                TotalUnits = publishedUnits.Count,
                CompletedUnits = completedUnits,
                DistinctUnitsStarted = distinctUnitsStarted,
                UnitProgress = unitProgress,
                RecentCompletedLessons = recentCompletedLessons,
                NextRecommendedLesson = nextRecommendedLesson,
                Achievements = achievements,
                CalendarStart = startDate.Date,
                CalendarEnd = endDate.Date,
                SignInsByDate = grouped,
                MonthLabels = monthLabels
            };

            return View(vm);
        }

        private static int CalculateStreak(List<DateTime> distinctDatesDescending, DateTime today)
        {
            if (!distinctDatesDescending.Any())
                return 0;

            var normalizedDates = distinctDatesDescending
                .Select(d => d.Date)
                .Distinct()
                .OrderByDescending(d => d)
                .ToList();

            var firstDate = normalizedDates[0];
            if (firstDate != today.Date && firstDate != today.Date.AddDays(-1))
                return 0;

            int streak = 0;
            var expectedDate = firstDate;

            foreach (var date in normalizedDates)
            {
                if (date == expectedDate)
                {
                    streak++;
                    expectedDate = expectedDate.AddDays(-1);
                }
                else if (date < expectedDate)
                {
                    break;
                }
            }

            return streak;
        }
    }
}