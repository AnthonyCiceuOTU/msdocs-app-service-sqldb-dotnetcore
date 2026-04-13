using DotNetCoreSqlDb.Data;
using DotNetCoreSqlDb.Models;
using DotNetCoreSqlDb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DotNetCoreSqlDb.Controllers
{
    [Authorize]
    public class UnitSevenController : Controller
    {
        private readonly MyDatabaseContext _context;

        public UnitSevenController(MyDatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Classes()
        {
            return View(new ClassesViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Classes(ClassesViewModel vm, string actionType)
        {
            vm.UserAnswer1 = vm.UserAnswer1?.Trim() ?? "";
            vm.UserAnswer2 = vm.UserAnswer2?.Trim() ?? "";
            vm.UserAnswer3 = vm.UserAnswer3?.Trim() ?? "";
            vm.UserAnswer4 = vm.UserAnswer4?.Trim() ?? "";
            vm.ExplanationAnswer = vm.ExplanationAnswer?.Trim() ?? "";

            if (actionType == "hint")
            {
                vm.ShowHint = true;
                vm.ShowSolution = false;
                return View(vm);
            }

            if (actionType == "solution")
            {
                vm.ShowHint = false;
                vm.ShowSolution = true;
                return View(vm);
            }

            if (actionType == "checkExplanation")
            {
                bool explanationCorrect =
                    (vm.ExplanationAnswer.Contains("blueprint", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("template", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("define", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("defines", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("structure", StringComparison.OrdinalIgnoreCase)) &&
                    (vm.ExplanationAnswer.Contains("object", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("objects", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("instance", StringComparison.OrdinalIgnoreCase));

                vm.ExplanationCorrect = explanationCorrect;
                vm.ExplanationFeedback = explanationCorrect
                    ? "Correct! A class is a blueprint that defines the structure and behavior of objects."
                    : "Try mentioning that a class is a blueprint or template used to create objects.";

                return View(vm);
            }

            if (actionType == "submit")
            {
                var saved = await SaveLessonProgressAsync("Classes");
                vm.ExplanationCorrect = true;
                vm.ExplanationFeedback = saved
                    ? "Lesson complete! Your progress has been saved."
                    : "Your answers were submitted, but progress could not be saved.";
                return View(vm);
            }

            vm.ShowHint = false;
            vm.ShowSolution = false;

            vm.IsQ1Correct = vm.UserAnswer1.Contains("Dog", StringComparison.OrdinalIgnoreCase);
            vm.Feedback1 = vm.IsQ1Correct == true
                ? "Correct!"
                : "Look for the name written after the `class` keyword.";

            vm.IsQ2Correct = vm.UserAnswer2 == "A blueprint for objects";
            vm.Feedback2 = vm.IsQ2Correct == true
                ? "Correct! A class is a blueprint that describes what objects of that type look like."
                : "A class acts as a blueprint — it describes the structure for creating objects.";

            vm.IsQ3Correct = vm.UserAnswer3.Equals("class", StringComparison.OrdinalIgnoreCase);
            vm.Feedback3 = vm.IsQ3Correct == true
                ? "Correct!"
                : "The keyword to declare a class is `class`.";

            vm.IsQ4Correct = vm.UserAnswer4 == "True";
            vm.Feedback4 = vm.IsQ4Correct == true
                ? "Correct! A program can contain as many classes as needed."
                : "You can define as many classes as your program requires.";

            return View(vm);
        }

        [HttpGet]
        public IActionResult ClassProperties()
        {
            return View(new ClassPropertiesViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClassProperties(ClassPropertiesViewModel vm, string actionType)
        {
            vm.UserAnswer1 = vm.UserAnswer1?.Trim() ?? "";
            vm.UserAnswer2 = vm.UserAnswer2?.Trim() ?? "";
            vm.UserAnswer3 = vm.UserAnswer3?.Trim() ?? "";
            vm.UserAnswer4 = vm.UserAnswer4?.Trim() ?? "";
            vm.ExplanationAnswer = vm.ExplanationAnswer?.Trim() ?? "";

            if (actionType == "hint")
            {
                vm.ShowHint = true;
                vm.ShowSolution = false;
                return View(vm);
            }

            if (actionType == "solution")
            {
                vm.ShowHint = false;
                vm.ShowSolution = true;
                return View(vm);
            }

            if (actionType == "checkExplanation")
            {
                bool explanationCorrect =
                    (vm.ExplanationAnswer.Contains("data", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("information", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("store", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("stores", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("value", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("values", StringComparison.OrdinalIgnoreCase)) &&
                    (vm.ExplanationAnswer.Contains("object", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("class", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("field", StringComparison.OrdinalIgnoreCase));

                vm.ExplanationCorrect = explanationCorrect;
                vm.ExplanationFeedback = explanationCorrect
                    ? "Correct! Fields store data that describes each object of that class."
                    : "Try mentioning that fields store data or values that belong to each object.";

                return View(vm);
            }

            if (actionType == "submit")
            {
                var saved = await SaveLessonProgressAsync("ClassProperties");
                vm.ExplanationCorrect = true;
                vm.ExplanationFeedback = saved
                    ? "Lesson complete! Your progress has been saved."
                    : "Your answers were submitted, but progress could not be saved.";
                return View(vm);
            }

            vm.ShowHint = false;
            vm.ShowSolution = false;

            vm.IsQ1Correct = vm.UserAnswer1.Equals("int", StringComparison.OrdinalIgnoreCase);
            vm.Feedback1 = vm.IsQ1Correct == true
                ? "Correct!"
                : "Year is declared as `int Year`, so its type is `int`.";

            vm.IsQ2Correct = vm.UserAnswer2 == "public";
            vm.Feedback2 = vm.IsQ2Correct == true
                ? "Correct! `public` makes the field accessible from outside the class."
                : "The `public` keyword allows the field to be accessed from outside the class.";

            vm.IsQ3Correct = vm.UserAnswer3.Contains('3');
            vm.Feedback3 = vm.IsQ3Correct == true
                ? "Correct! The Car class has three fields: Model, Year, and Price."
                : "Count each declared field: Model, Year, and Price — that is 3.";

            vm.IsQ4Correct = vm.UserAnswer4 == "True";
            vm.Feedback4 = vm.IsQ4Correct == true
                ? "Correct! Each field can have its own data type."
                : "Fields can each have different types, as seen with string, int, and double in the example.";

            return View(vm);
        }

        [HttpGet]
        public IActionResult Constructors()
        {
            return View(new ConstructorsViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Constructors(ConstructorsViewModel vm, string actionType)
        {
            vm.UserAnswer1 = vm.UserAnswer1?.Trim() ?? "";
            vm.UserAnswer2 = vm.UserAnswer2?.Trim() ?? "";
            vm.UserAnswer3 = vm.UserAnswer3?.Trim() ?? "";
            vm.UserAnswer4 = vm.UserAnswer4?.Trim() ?? "";
            vm.ExplanationAnswer = vm.ExplanationAnswer?.Trim() ?? "";

            if (actionType == "hint")
            {
                vm.ShowHint = true;
                vm.ShowSolution = false;
                return View(vm);
            }

            if (actionType == "solution")
            {
                vm.ShowHint = false;
                vm.ShowSolution = true;
                return View(vm);
            }

            if (actionType == "checkExplanation")
            {
                bool explanationCorrect =
                    (vm.ExplanationAnswer.Contains("initialize", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("initializes", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("set up", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("setup", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("create", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("creates", StringComparison.OrdinalIgnoreCase)) &&
                    (vm.ExplanationAnswer.Contains("object", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("fields", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("values", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("data", StringComparison.OrdinalIgnoreCase));

                vm.ExplanationCorrect = explanationCorrect;
                vm.ExplanationFeedback = explanationCorrect
                    ? "Correct! A constructor initializes an object's fields when it is created."
                    : "Try mentioning that a constructor initializes or sets up an object's data when it is created.";

                return View(vm);
            }

            if (actionType == "submit")
            {
                var saved = await SaveLessonProgressAsync("Constructors");
                vm.ExplanationCorrect = true;
                vm.ExplanationFeedback = saved
                    ? "Lesson complete! Your progress has been saved."
                    : "Your answers were submitted, but progress could not be saved.";
                return View(vm);
            }

            vm.ShowHint = false;
            vm.ShowSolution = false;

            vm.IsQ1Correct = vm.UserAnswer1.Equals("new", StringComparison.OrdinalIgnoreCase);
            vm.Feedback1 = vm.IsQ1Correct == true
                ? "Correct!"
                : "The `new` keyword tells C# to create a new object from the class.";

            vm.IsQ2Correct = vm.UserAnswer2 == "Dog";
            vm.Feedback2 = vm.IsQ2Correct == true
                ? "Correct! A constructor always has the same name as its class."
                : "The constructor name must match the class name exactly — in this case, Dog.";

            vm.IsQ3Correct = vm.UserAnswer3.Contains("Rex", StringComparison.OrdinalIgnoreCase);
            vm.Feedback3 = vm.IsQ3Correct == true
                ? "Correct! The string \"Rex\" is passed as the name argument, so myDog.Name = \"Rex\"."
                : "The argument \"Rex\" is passed to the constructor's name parameter, so myDog.Name will be Rex.";

            vm.IsQ4Correct = vm.UserAnswer4 == "True";
            vm.Feedback4 = vm.IsQ4Correct == true
                ? "Correct! A constructor always shares its class name."
                : "By definition, a constructor must have the same name as the class.";

            return View(vm);
        }

        [HttpGet]
        public IActionResult ClassMethods()
        {
            return View(new ClassMethodsViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClassMethods(ClassMethodsViewModel vm, string actionType)
        {
            vm.UserAnswer1 = vm.UserAnswer1?.Trim() ?? "";
            vm.UserAnswer2 = vm.UserAnswer2?.Trim() ?? "";
            vm.UserAnswer3 = vm.UserAnswer3?.Trim() ?? "";
            vm.UserAnswer4 = vm.UserAnswer4?.Trim() ?? "";
            vm.ExplanationAnswer = vm.ExplanationAnswer?.Trim() ?? "";

            if (actionType == "hint")
            {
                vm.ShowHint = true;
                vm.ShowSolution = false;
                return View(vm);
            }

            if (actionType == "solution")
            {
                vm.ShowHint = false;
                vm.ShowSolution = true;
                return View(vm);
            }

            if (actionType == "checkExplanation")
            {
                bool explanationCorrect =
                    (vm.ExplanationAnswer.Contains("behavior", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("behaviours", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("action", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("actions", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("do", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("perform", StringComparison.OrdinalIgnoreCase)) &&
                    (vm.ExplanationAnswer.Contains("object", StringComparison.OrdinalIgnoreCase) ||
                     vm.ExplanationAnswer.Contains("class", StringComparison.OrdinalIgnoreCase));

                vm.ExplanationCorrect = explanationCorrect;
                vm.ExplanationFeedback = explanationCorrect
                    ? "Correct! Methods define what actions an object can perform."
                    : "Try mentioning the actions or behaviors that an object can perform.";

                return View(vm);
            }

            if (actionType == "submit")
            {
                var saved = await SaveLessonProgressAsync("ClassMethods");
                vm.ExplanationCorrect = true;
                vm.ExplanationFeedback = saved
                    ? "Lesson complete! Your progress has been saved."
                    : "Your answers were submitted, but progress could not be saved.";
                return View(vm);
            }

            vm.ShowHint = false;
            vm.ShowSolution = false;

            vm.IsQ1Correct = vm.UserAnswer1.Contains("Bark", StringComparison.OrdinalIgnoreCase);
            vm.Feedback1 = vm.IsQ1Correct == true
                ? "Correct!"
                : "Look for the method name written after `void` inside the class.";

            vm.IsQ2Correct = vm.UserAnswer2 == "object.Method()";
            vm.Feedback2 = vm.IsQ2Correct == true
                ? "Correct! Use the object name, a dot, then the method name with parentheses."
                : "You call a method on an object using: objectName.MethodName()";

            vm.IsQ3Correct = vm.UserAnswer3.Equals("void", StringComparison.OrdinalIgnoreCase);
            vm.Feedback3 = vm.IsQ3Correct == true
                ? "Correct!"
                : "The return type is written before the method name. Bark returns nothing, so it is `void`.";

            vm.IsQ4Correct = vm.UserAnswer4 == "True";
            vm.Feedback4 = vm.IsQ4Correct == true
                ? "Correct! Instance methods can read and use the fields of their own object."
                : "Methods defined inside a class can directly access and use that class's fields.";

            return View(vm);
        }

        private async Task<bool> SaveLessonProgressAsync(string actionName)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return false;
            }

            var lesson = await _context.Lessons
                .FirstOrDefaultAsync(l => l.ControllerName == "UnitSeven" && l.ActionName == actionName && l.IsPublished);

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
