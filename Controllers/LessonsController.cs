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

            Lesson? currentLesson = null;

            if (lessonId.HasValue)
            {
                currentLesson = units
                    .SelectMany(u => u.Lessons)
                    .OrderBy(l => l.Unit.SortOrder)
                    .ThenBy(l => l.SortOrder)
                    .ThenBy(l => l.Id)
                    .FirstOrDefault(l => l.Id == lessonId.Value);
            }

            if (currentLesson == null)
            {
                currentLesson = units
                    .SelectMany(u => u.Lessons)
                    .OrderBy(l => l.Unit.SortOrder)
                    .ThenBy(l => l.SortOrder)
                    .ThenBy(l => l.Id)
                    .FirstOrDefault();
            }

            if (!isGuest && userId.HasValue && currentLesson != null)
            {
                await UpsertLastAccessed(userId.Value, currentLesson.Id);
            }

            var allLessonsOrdered = units
                .OrderBy(u => u.SortOrder)
                .ThenBy(u => u.Id)
                .SelectMany(u => u.Lessons.OrderBy(l => l.SortOrder).ThenBy(l => l.Id))
                .ToList();

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
                        .OrderBy(l => l.SortOrder)
                        .ThenBy(l => l.Id)
                        .Select(l => new LessonLinkViewModel
                        {
                            LessonId = l.Id,
                            Title = l.Title,
                            SortOrder = l.SortOrder,
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

            var lessonsQuery = _context.Lessons
                .Include(l => l.Unit)
                .Where(l => l.IsPublished)
                .AsQueryable();

            if (isGuest)
            {
                lessonsQuery = lessonsQuery.Where(l => l.UnitId == 1);
            }

            var firstLesson = await lessonsQuery
                .OrderBy(l => l.Unit.SortOrder)
                .ThenBy(l => l.SortOrder)
                .ThenBy(l => l.Id)
                .FirstOrDefaultAsync();

            if (firstLesson == null)
                return RedirectToAction(nameof(Index));

            return RedirectToAction(nameof(Open), new { lessonId = firstLesson.Id });
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

            return RedirectToAction(nameof(Index), new { lessonId });
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
                .OrderBy(l => l.Unit.SortOrder)
                .ThenBy(l => l.SortOrder)
                .ThenBy(l => l.Id)
                .ToListAsync();

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
                .OrderBy(l => l.Unit.SortOrder)
                .ThenBy(l => l.SortOrder)
                .ThenBy(l => l.Id)
                .ToListAsync();

            var currentIndex = lessons.FindIndex(l => l.Id == currentLessonId);
            if (currentIndex > 0)
                return lessons[currentIndex - 1].Id;

            return null;
        }

        private bool IsGuestUser()
        {
            return User.FindFirst("IsGuest")?.Value == "true";
        }

        private bool CanGuestAccessLesson(Lesson lesson)
        {
            return lesson.UnitId == 1;
        }
    }
}