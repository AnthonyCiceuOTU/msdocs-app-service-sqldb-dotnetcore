using System.Security.Claims;
using DotNetCoreSqlDb.Data;
using DotNetCoreSqlDb.Models;
using DotNetCoreSqlDb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNetCoreSqlDb.Controllers
{
    [Authorize]
    public class LessonsController : Controller
    {
        private readonly MyDatabaseContext _context;
        private static readonly Dictionary<string, int> UnitFourLessonOrder = new(StringComparer.OrdinalIgnoreCase)
        {
            ["Counters"] = 1,
            ["ForLoops"] = 2,
            ["LoopErrors"] = 3,
            ["WhileLoops"] = 4,
            ["WhyLoops"] = 5
        };

        public LessonsController(MyDatabaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? lessonId = null)
        {
            var isGuest = IsGuestUser();
            var userId = GetCurrentUserId();

            var unitsQuery = _context.Units
                .Include(u => u.Lessons.Where(l => l.IsPublished))
                .AsQueryable();

            if (isGuest)
            {
                unitsQuery = unitsQuery.Where(u => u.Id == 1);
            }

            var units = await unitsQuery
                .OrderBy(u => u.SortOrder)
                .ThenBy(u => u.Id)
                .ToListAsync();

            var progressList = new List<UserLessonProgress>();
            if (!isGuest && userId.HasValue)
            {
                progressList = await _context.UserLessonProgresses
                    .Where(p => p.UserId == userId.Value)
                    .ToListAsync();
            }

            var allLessonsOrdered = units
                .OrderBy(u => u.SortOrder)
                .ThenBy(u => u.Id)
                .SelectMany(u => u.Lessons.OrderBy(GetEffectiveLessonSortOrder).ThenBy(l => l.Id))
                .ToList();

            Lesson? currentLesson = null;

            if (lessonId.HasValue)
            {
                currentLesson = allLessonsOrdered.FirstOrDefault(l => l.Id == lessonId.Value);
            }

            if (currentLesson == null)
            {
                currentLesson = GetFirstIncompleteLesson(allLessonsOrdered, progressList, isGuest)
                    ?? allLessonsOrdered.FirstOrDefault();
            }

            if (!isGuest && userId.HasValue && currentLesson != null)
            {
                await UpsertLastAccessed(userId.Value, currentLesson.Id);
            }

            int? previousLessonId = null;
            int? nextLessonId = null;

            if (currentLesson != null)
            {
                var currentIndex = allLessonsOrdered.FindIndex(l => l.Id == currentLesson.Id);

                if (currentIndex > 0)
                    previousLessonId = allLessonsOrdered[currentIndex - 1].Id;

                if (currentIndex >= 0 && currentIndex < allLessonsOrdered.Count - 1)
                    nextLessonId = allLessonsOrdered[currentIndex + 1].Id;
            }

            var vm = new LessonsIndexViewModel
            {
                Units = units.Select(u => new UnitLessonsViewModel
                {
                    UnitId = u.Id,
                    Title = u.Title,
                    Description = u.Description,
                    SortOrder = u.SortOrder,
                    Lessons = u.Lessons
                        .OrderBy(GetEffectiveLessonSortOrder)
                        .ThenBy(l => l.Id)
                        .Select(l => new LessonLinkViewModel
                        {
                            LessonId = l.Id,
                            Title = l.Title,
                            SortOrder = GetEffectiveLessonSortOrder(l),
                            IsCompleted = !isGuest && progressList.Any(p => p.LessonId == l.Id && p.IsCompleted),
                            IsCurrent = currentLesson != null && l.Id == currentLesson.Id
                        })
                        .ToList()
                }).ToList(),
                CurrentLessonId = currentLesson?.Id,
                CurrentUnitId = currentLesson?.UnitId,
                CurrentUnitTitle = currentLesson?.Unit?.Title,
                CurrentLessonTitle = currentLesson?.Title,
                CurrentLessonDescription = currentLesson?.Description,
                CurrentLessonCompleted = !isGuest &&
                                         currentLesson != null &&
                                         progressList.Any(p => p.LessonId == currentLesson.Id && p.IsCompleted),
                CurrentLessonContent = currentLesson?.Content,
                PreviousLessonId = previousLessonId,
                NextLessonId = nextLessonId
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Start()
        {
            var isGuest = IsGuestUser();
            var userId = GetCurrentUserId();

            var lessonsQuery = _context.Lessons
                .Include(l => l.Unit)
                .Where(l => l.IsPublished)
                .AsQueryable();

            if (isGuest)
            {
                lessonsQuery = lessonsQuery.Where(l => l.UnitId == 1);
            }

            var lessons = await lessonsQuery
                .Include(l => l.Unit)
                .ToListAsync();

            lessons = lessons
                .OrderBy(l => l.Unit.SortOrder)
                .ThenBy(GetEffectiveLessonSortOrder)
                .ThenBy(l => l.Id)
                .ToList();

            if (!lessons.Any())
                return RedirectToAction(nameof(Index));

            var progressList = new List<UserLessonProgress>();
            if (!isGuest && userId.HasValue)
            {
                progressList = await _context.UserLessonProgresses
                    .Where(p => p.UserId == userId.Value)
                    .ToListAsync();
            }

            var lessonToOpen = GetFirstIncompleteLesson(lessons, progressList, isGuest)
                ?? lessons.First();

            return RedirectToAction(nameof(Open), new { lessonId = lessonToOpen.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Open(int lessonId)
        {
            var isGuest = IsGuestUser();
            var userId = GetCurrentUserId();

            var lesson = await _context.Lessons
                .Include(l => l.Unit)
                .FirstOrDefaultAsync(l => l.Id == lessonId && l.IsPublished);

            if (lesson == null)
                return RedirectToAction(nameof(Index));

            if (isGuest && !CanGuestAccessLesson(lesson))
                return RedirectToAction(nameof(Index));

            if (!isGuest)
            {
                if (!userId.HasValue)
                    return RedirectToAction("NotAuthorized", "Home");

                await UpsertLastAccessed(userId.Value, lesson.Id);
            }

            return RedirectToAction(
                actionName: lesson.ActionName,
                controllerName: lesson.ControllerName,
                routeValues: new { lessonId = lesson.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Complete(int lessonId)
        {
            var isGuest = IsGuestUser();
            if (isGuest)
                return RedirectToAction(nameof(Index), new { lessonId });

            var userId = GetCurrentUserId();
            if (userId == null)
                return RedirectToAction("NotAuthorized", "Home");

            var lesson = await _context.Lessons
                .FirstOrDefaultAsync(l => l.Id == lessonId && l.IsPublished);

            if (lesson == null)
                return RedirectToAction(nameof(Index));

            var progress = await _context.UserLessonProgresses
                .FirstOrDefaultAsync(p => p.UserId == userId.Value && p.LessonId == lessonId);

            if (progress == null)
            {
                progress = new UserLessonProgress
                {
                    UserId = userId.Value,
                    LessonId = lessonId,
                    IsCompleted = true,
                    CompletedAtUtc = DateTime.UtcNow,
                    LastAccessedAtUtc = DateTime.UtcNow
                };

                _context.UserLessonProgresses.Add(progress);
            }
            else
            {
                progress.IsCompleted = true;
                progress.CompletedAtUtc = DateTime.UtcNow;
                progress.LastAccessedAtUtc = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            var nextIncompleteLessonId = await GetFirstIncompleteLessonIdForUser(userId.Value, isGuest);
            return RedirectToAction(nameof(Index), new { lessonId = nextIncompleteLessonId ?? lessonId });
        }

        [HttpGet]
        public async Task<IActionResult> Previous(int lessonId)
        {
            var previousLessonId = await GetPreviousLessonId(lessonId);

            if (previousLessonId == null)
                return RedirectToAction(nameof(Index), new { lessonId });

            return RedirectToAction(nameof(Index), new { lessonId = previousLessonId.Value });
        }

        [HttpGet]
        public async Task<IActionResult> Next(int lessonId)
        {
            var nextLessonId = await GetNextLessonId(lessonId);

            if (nextLessonId == null)
                return RedirectToAction(nameof(Index), new { lessonId });

            return RedirectToAction(nameof(Index), new { lessonId = nextLessonId.Value });
        }

        private Guid? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(userIdClaim, out var userId))
                return userId;

            return null;
        }

        private Lesson? GetFirstIncompleteLesson(IEnumerable<Lesson> lessons, IEnumerable<UserLessonProgress> progressList, bool isGuest)
        {
            if (isGuest)
                return lessons.FirstOrDefault();

            var completedLessonIds = progressList
                .Where(p => p.IsCompleted)
                .Select(p => p.LessonId)
                .ToHashSet();

            return lessons.FirstOrDefault(l => !completedLessonIds.Contains(l.Id));
        }

        private async Task<int?> GetFirstIncompleteLessonIdForUser(Guid userId, bool isGuest)
        {
            var lessonsQuery = _context.Lessons
                .Include(l => l.Unit)
                .Where(l => l.IsPublished)
                .AsQueryable();

            if (isGuest)
            {
                lessonsQuery = lessonsQuery.Where(l => l.UnitId == 1);
            }

            var lessons = await lessonsQuery
                .ToListAsync();

            lessons = lessons
                .OrderBy(l => l.Unit.SortOrder)
                .ThenBy(GetEffectiveLessonSortOrder)
                .ThenBy(l => l.Id)
                .ToList();

            if (!lessons.Any())
                return null;

            var progressList = await _context.UserLessonProgresses
                .Where(p => p.UserId == userId)
                .ToListAsync();

            return GetFirstIncompleteLesson(lessons, progressList, isGuest)?.Id;
        }

        private async Task UpsertLastAccessed(Guid userId, int lessonId)
        {
            var progress = await _context.UserLessonProgresses
                .FirstOrDefaultAsync(p => p.UserId == userId && p.LessonId == lessonId);

            if (progress == null)
            {
                _context.UserLessonProgresses.Add(new UserLessonProgress
                {
                    UserId = userId,
                    LessonId = lessonId,
                    LastAccessedAtUtc = DateTime.UtcNow,
                    IsCompleted = false
                });
            }
            else
            {
                progress.LastAccessedAtUtc = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }

        private async Task<int?> GetNextLessonId(int currentLessonId)
        {
            var isGuest = IsGuestUser();

            var lessonsQuery = _context.Lessons
                .Include(l => l.Unit)
                .Where(l => l.IsPublished)
                .AsQueryable();

            if (isGuest)
            {
                lessonsQuery = lessonsQuery.Where(l => l.UnitId == 1);
            }

            var lessons = await lessonsQuery
                .ToListAsync();

            lessons = lessons
                .OrderBy(l => l.Unit.SortOrder)
                .ThenBy(GetEffectiveLessonSortOrder)
                .ThenBy(l => l.Id)
                .ToList();

            var currentIndex = lessons.FindIndex(l => l.Id == currentLessonId);
            if (currentIndex >= 0 && currentIndex < lessons.Count - 1)
                return lessons[currentIndex + 1].Id;

            return null;
        }

        private async Task<int?> GetPreviousLessonId(int currentLessonId)
        {
            var isGuest = IsGuestUser();

            var lessonsQuery = _context.Lessons
                .Include(l => l.Unit)
                .Where(l => l.IsPublished)
                .AsQueryable();

            if (isGuest)
            {
                lessonsQuery = lessonsQuery.Where(l => l.UnitId == 1);
            }

            var lessons = await lessonsQuery
                .ToListAsync();

            lessons = lessons
                .OrderBy(l => l.Unit.SortOrder)
                .ThenBy(GetEffectiveLessonSortOrder)
                .ThenBy(l => l.Id)
                .ToList();

            var currentIndex = lessons.FindIndex(l => l.Id == currentLessonId);
            if (currentIndex > 0)
                return lessons[currentIndex - 1].Id;

            return null;
        }

        private bool IsGuestUser()
        {
            return string.Equals(User.FindFirstValue("IsGuest"), "true", StringComparison.OrdinalIgnoreCase);
        }

        private bool CanGuestAccessLesson(Lesson lesson)
        {
            return lesson.UnitId == 1;
        }

        private static int GetEffectiveLessonSortOrder(Lesson lesson)
        {
            if (lesson.UnitId == 4 && UnitFourLessonOrder.TryGetValue(lesson.ActionName, out var order))
            {
                return order;
            }

            return lesson.SortOrder;
        }
    }
}
