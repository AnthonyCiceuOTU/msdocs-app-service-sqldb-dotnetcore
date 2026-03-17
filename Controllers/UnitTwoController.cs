using DotNetCoreSqlDb.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCoreSqlDb.Controllers
{
    public class UnitTwoController : Controller
    {
        public IActionResult Index(string? lesson = null)
        {
            var model = BuildViewModel(lesson);
            return View("UnitTwo", model);
        }

        private UnitTwoViewModel BuildViewModel(string? lessonSlug)
        {
            var lessons = new List<UnitTwoLessonViewModel>
            {
                new UnitTwoLessonViewModel
                {
                    Id = 1,
                    Slug = "variables",
                    Title = "Variables",
                    LessonType = "Lesson",
                    ExerciseCount = 1,
                    ChallengeTitle = "Practice: Assign a Variable",
                    ChallengePrompt = "Complete the pseudocode so the variable 'points' stores the value 25.",
                    StarterCode = "BEGIN\nSET points = ____\nDISPLAY points\nEND",
                    ExpectedAnswer = "25",
                    Hint = "You are assigning a number directly to the variable.",
                    Solution = "BEGIN\nSET points = 25\nDISPLAY points\nEND"
                },
                new UnitTwoLessonViewModel
                {
                    Id = 2,
                    Slug = "data-types",
                    Title = "Data Types",
                    LessonType = "Lesson",
                    ExerciseCount = 1,
                    ChallengeTitle = "Practice: Store Text",
                    ChallengePrompt = "Complete the pseudocode so the variable 'name' stores the text Alex.",
                    StarterCode = "BEGIN\nSET name = ____\nDISPLAY name\nEND",
                    ExpectedAnswer = "\"Alex\"",
                    Hint = "Text values should be written in quotation marks.",
                    Solution = "BEGIN\nSET name = \"Alex\"\nDISPLAY name\nEND"
                },
                new UnitTwoLessonViewModel
                {
                    Id = 3,
                    Slug = "arithmetic-expressions",
                    Title = "Arithmetic Expressions",
                    LessonType = "Lesson",
                    ExerciseCount = 1,
                    ChallengeTitle = "Practice: Calculate a Total",
                    ChallengePrompt = "Complete the pseudocode so the variable 'total' stores the result of 8 + 4.",
                    StarterCode = "BEGIN\nSET total = ____\nDISPLAY total\nEND",
                    ExpectedAnswer = "8 + 4",
                    Hint = "Use an arithmetic expression on the right side of the assignment.",
                    Solution = "BEGIN\nSET total = 8 + 4\nDISPLAY total\nEND"
                },
                new UnitTwoLessonViewModel
                {
                    Id = 4,
                    Slug = "input-output",
                    Title = "Input and Output",
                    LessonType = "Lesson",
                    ExerciseCount = 1,
                    ChallengeTitle = "Practice: Input a Value",
                    ChallengePrompt = "Complete the pseudocode so the program asks the user for age, stores it, and then displays it.",
                    StarterCode = "BEGIN\nINPUT ____\nDISPLAY age\nEND",
                    ExpectedAnswer = "age",
                    Hint = "The variable should be named the same thing that gets displayed.",
                    Solution = "BEGIN\nINPUT age\nDISPLAY age\nEND"
                }
            };

            var selectedLesson = lessons.FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(lessonSlug))
            {
                selectedLesson = lessons.FirstOrDefault(l => l.Slug == lessonSlug) ?? selectedLesson;
            }

            var selectedIndex = lessons.FindIndex(l => l.Slug == selectedLesson!.Slug);

            return new UnitTwoViewModel
            {
                UnitTitle = "Unit Two: Variables and Data",
                UnitDescription = "Understand how programs store and manipulate information using variables, data types, and expressions.",
                Lessons = lessons,
                SelectedLesson = selectedLesson,
                PreviousLesson = selectedIndex > 0 ? lessons[selectedIndex - 1] : null,
                NextLesson = selectedIndex < lessons.Count - 1 ? lessons[selectedIndex + 1] : null
            };
        }
    }
}