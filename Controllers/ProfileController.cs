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
            // Try to get the signed-in user's username from claims
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

            // Pull the user record
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

            // Build a GitHub-style 1-year calendar
            var today = DateTime.Today;
            var endDate = today;

            // Start roughly 1 year back, then align to Sunday so grid looks clean
            var rawStart = today.AddDays(-364);
            var startDate = rawStart.AddDays(-(int)rawStart.DayOfWeek);

            var logsQuery = _context.SignInLog
                .Where(x => x.UserName == user.Username &&
                            x.DateTime.Date >= startDate.Date &&
                            x.DateTime.Date <= endDate.Date);

            var logs = await logsQuery
                .OrderBy(x => x.DateTime)
                .ToListAsync();

            var grouped = logs
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

            var totalAllTimeSignIns = await _context.SignInLog
                .Where(x => x.UserName == user.Username)
                .CountAsync();

            var firstSignIn = await _context.SignInLog
                .Where(x => x.UserName == user.Username)
                .OrderBy(x => x.DateTime)
                .Select(x => (DateTime?)x.DateTime)
                .FirstOrDefaultAsync();

            var lastSignIn = await _context.SignInLog
                .Where(x => x.UserName == user.Username)
                .OrderByDescending(x => x.DateTime)
                .Select(x => (DateTime?)x.DateTime)
                .FirstOrDefaultAsync();

            var vm = new ProfileViewModel
            {
                UserId = user.ID,
                Username = user.Username,
                TotalSignIns = totalAllTimeSignIns,
                FirstSignIn = firstSignIn,
                LastSignIn = lastSignIn,
                CalendarStart = startDate.Date,
                CalendarEnd = endDate.Date,
                SignInsByDate = grouped,
                MonthLabels = monthLabels
            };

            return View(vm);
        }
    }
}