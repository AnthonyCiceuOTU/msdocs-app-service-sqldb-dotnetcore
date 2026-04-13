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

        public int CurrentStreakDays { get; set; }

        public int TotalLessons { get; set; }
        public int CompletedLessons { get; set; }
        public int RemainingLessons => Math.Max(0, TotalLessons - CompletedLessons);
        public double OverallProgressPercent { get; set; }

        public int TotalUnits { get; set; }
        public int CompletedUnits { get; set; }

        public int DistinctUnitsStarted { get; set; }

        public ProfileLessonSummaryViewModel? NextRecommendedLesson { get; set; }

        public List<ProfileUnitProgressViewModel> UnitProgress { get; set; } = new();
        public List<ProfileLessonSummaryViewModel> RecentCompletedLessons { get; set; } = new();
        public List<ProfileAchievementViewModel> Achievements { get; set; } = new();

        public DateTime CalendarStart { get; set; }
        public DateTime CalendarEnd { get; set; }

        public Dictionary<DateTime, int> SignInsByDate { get; set; } = new();
        public List<CalendarMonthLabel> MonthLabels { get; set; } = new();
    }

    public class CalendarMonthLabel
    {
        public string Label { get; set; } = string.Empty;
        public int WeekIndex { get; set; }
    }

    public class ProfileUnitProgressViewModel
    {
        public int UnitId { get; set; }
        public string UnitTitle { get; set; } = string.Empty;
        public int CompletedLessons { get; set; }
        public int TotalLessons { get; set; }
        public double ProgressPercent { get; set; }
        public bool IsComplete { get; set; }
    }

    public class ProfileLessonSummaryViewModel
    {
        public int LessonId { get; set; }
        public int UnitId { get; set; }
        public string UnitTitle { get; set; } = string.Empty;
        public string LessonTitle { get; set; } = string.Empty;
        public DateTime? CompletedAtUtc { get; set; }
    }

    public class ProfileAchievementViewModel
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsUnlocked { get; set; }
        public string Icon { get; set; } = string.Empty;
    }
}