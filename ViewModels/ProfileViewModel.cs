using System;
using System.Collections.Generic;

namespace DotNetCoreSqlDb.Models.ViewModels
{
    public class ProfileViewModel
    {
        public Guid? UserId { get; set; }
        public string Username { get; set; } = string.Empty;

        public int TotalSignIns { get; set; }
        public DateTime? FirstSignIn { get; set; }
        public DateTime? LastSignIn { get; set; }

        public DateTime CalendarStart { get; set; }
        public DateTime CalendarEnd { get; set; }

        // Key = date only (midnight), Value = number of sign-ins that day
        public Dictionary<DateTime, int> SignInsByDate { get; set; } = new();

        public List<CalendarMonthLabel> MonthLabels { get; set; } = new();
    }

    public class CalendarMonthLabel
    {
        public string Label { get; set; } = string.Empty;
        public int WeekIndex { get; set; }
    }
}