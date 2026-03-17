using DotNetCoreSqlDb.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCoreSqlDb.Controllers
{
    public class PseudocodeController : Controller
    {
        public IActionResult Index(string? lesson = null)
        {
            var units = BuildUnits();

            var allLessons = units
                .SelectMany(u => u.Lessons)
                .Where(l => l.IsAvailable)
                .ToList();

            var selectedLesson = allLessons.FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(lesson))
            {
                selectedLesson = allLessons.FirstOrDefault(l => l.Slug == lesson) ?? selectedLesson;
            }

            var model = new PseudocodeCourseViewModel
            {
                CourseTitle = "CodeQuest Pseudocode",
                Units = units,
                SelectedLesson = selectedLesson!
            };

            return View(model);
        }

        private static List<PseudocodeUnit> BuildUnits()
        {
            return new List<PseudocodeUnit>
            {
                new PseudocodeUnit
                {
                    Id = 1,
                    Title = "Unit 1: Introduction to Pseudocode",
                    Description = "Learn what pseudocode is and how it is used to plan algorithms.",
                    IsExpanded = false,
                    Lessons = new List<PseudocodeLesson>
                    {
                        new PseudocodeLesson
                        {
                            Id = 101,
                            UnitId = 1,
                            Slug = "pseudocode-basics",
                            Title = "Pseudocode Basics",
                            Summary = "Placeholder lesson for the shared course structure.",
                            ChallengeTitle = "Coming Soon",
                            ChallengePrompt = "This lesson is not part of your section.",
                            StarterCode = "BEGIN\n// Coming soon\nEND",
                            ExpectedAnswer = "",
                            Hint = "Unit 1 is only shown as a placeholder.",
                            Solution = "// Coming soon",
                            IsAvailable = false
                        }
                    }
                },
                new PseudocodeUnit
                {
                    Id = 2,
                    Title = "Unit 2: Variables and Data",
                    Description = "Understand how programs store and manipulate information using variables, data types, and expressions.",
                    IsExpanded = true,
                    Lessons = new List<PseudocodeLesson>
                    {
                        new PseudocodeLesson
                        {
                            Id = 201,
                            UnitId = 2,
                            Slug = "variables",
                            Title = "Variables",
                            Summary = "Lesson • 1 exercise",
                            ChallengeTitle = "Practice: Assign a Variable",
                            ChallengePrompt = "Complete the pseudocode so the variable 'points' stores the value 25.",
                            StarterCode = "BEGIN\nSET points = ____\nDISPLAY points\nEND",
                            ExpectedAnswer = "25",
                            Hint = "You are assigning a number directly to the variable.",
                            Solution = "BEGIN\nSET points = 25\nDISPLAY points\nEND",
                            IsAvailable = true
                        },
                        new PseudocodeLesson
                        {
                            Id = 202,
                            UnitId = 2,
                            Slug = "data-types",
                            Title = "Data Types",
                            Summary = "Lesson • 1 exercise",
                            ChallengeTitle = "Practice: Store Text",
                            ChallengePrompt = "Complete the pseudocode so the variable 'name' stores the text Alex.",
                            StarterCode = "BEGIN\nSET name = ____\nDISPLAY name\nEND",
                            ExpectedAnswer = "\"Alex\"",
                            Hint = "Text values should be written in quotation marks.",
                            Solution = "BEGIN\nSET name = \"Alex\"\nDISPLAY name\nEND",
                            IsAvailable = true
                        },
                        new PseudocodeLesson
                        {
                            Id = 203,
                            UnitId = 2,
                            Slug = "arithmetic-expressions",
                            Title = "Arithmetic Expressions",
                            Summary = "Lesson • 1 exercise",
                            ChallengeTitle = "Practice: Calculate a Total",
                            ChallengePrompt = "Complete the pseudocode so the variable 'total' stores the result of 8 + 4.",
                            StarterCode = "BEGIN\nSET total = ____\nDISPLAY total\nEND",
                            ExpectedAnswer = "8 + 4",
                            Hint = "Use an arithmetic expression on the right side of the assignment.",
                            Solution = "BEGIN\nSET total = 8 + 4\nDISPLAY total\nEND",
                            IsAvailable = true
                        },
                        new PseudocodeLesson
                        {
                            Id = 204,
                            UnitId = 2,
                            Slug = "input-output",
                            Title = "Input and Output",
                            Summary = "Lesson • 1 exercise",
                            ChallengeTitle = "Practice: Input a Value",
                            ChallengePrompt = "Complete the pseudocode so the program asks the user for age, stores it, and then displays it.",
                            StarterCode = "BEGIN\nINPUT ____\nDISPLAY age\nEND",
                            ExpectedAnswer = "age",
                            Hint = "The variable should be named the same thing that gets displayed.",
                            Solution = "BEGIN\nINPUT age\nDISPLAY age\nEND",
                            IsAvailable = true
                        }
                    }
                },
                new PseudocodeUnit
                {
                    Id = 3,
                    Title = "Unit 3: Decision Making",
                    Description = "Learn how programs make decisions using conditions and logical operators.",
                    IsExpanded = false,
                    Lessons = new List<PseudocodeLesson>
                    {
                        new PseudocodeLesson
                        {
                            Id = 301,
                            UnitId = 3,
                            Slug = "boolean-logic",
                            Title = "Boolean Logic",
                            Summary = "Lesson • 0 exercises",
                            ChallengeTitle = "Coming Soon",
                            ChallengePrompt = "This lesson is not part of your section.",
                            StarterCode = "BEGIN\n// Coming soon\nEND",
                            ExpectedAnswer = "",
                            Hint = "Unit 3 is only shown as a placeholder.",
                            Solution = "// Coming soon",
                            IsAvailable = false
                        },
                        new PseudocodeLesson
                        {
                            Id = 302,
                            UnitId = 3,
                            Slug = "if-statements",
                            Title = "IF Statements",
                            Summary = "Lesson • 0 exercises",
                            ChallengeTitle = "Coming Soon",
                            ChallengePrompt = "This lesson is not part of your section.",
                            StarterCode = "BEGIN\n// Coming soon\nEND",
                            ExpectedAnswer = "",
                            Hint = "Unit 3 is only shown as a placeholder.",
                            Solution = "// Coming soon",
                            IsAvailable = false
                        },
                        new PseudocodeLesson
                        {
                            Id = 303,
                            UnitId = 3,
                            Slug = "if-else",
                            Title = "IF-ELSE",
                            Summary = "Lesson • 0 exercises",
                            ChallengeTitle = "Coming Soon",
                            ChallengePrompt = "This lesson is not part of your section.",
                            StarterCode = "BEGIN\n// Coming soon\nEND",
                            ExpectedAnswer = "",
                            Hint = "Unit 3 is only shown as a placeholder.",
                            Solution = "// Coming soon",
                            IsAvailable = false
                        }
                    }
                }
            };
        }
    }
}