using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNetCoreSqlDb.Data;
using DotNetCoreSqlDb.Models;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;
using DotNetCoreSqlDb.ViewModels;

namespace DotNetCoreSqlDb.Controllers
{
    public class SyntaxGameController : Controller
    {
        private readonly MyDatabaseContext _context;

        public SyntaxGameController(MyDatabaseContext context)
        {
            _context = context;
        }

        // GET: /SyntaxGame/Index?questionNumber=1&language=Python
        [HttpGet]
        public async Task<IActionResult> Index(int questionNumber = 1, string language = "Python")
        {
            var question = await _context.SyntaxGameQuestions
                .FirstOrDefaultAsync(q => q.QuestionNumber == questionNumber);

            if (question == null)
            {
                ViewBag.GameComplete = true;
                return View(new SyntaxGameViewModel
                {
                    QuestionNumber = questionNumber,
                    SelectedLanguage = language,
                    FeedbackMessage = "You finished all questions."
                });
            }

            var vm = new SyntaxGameViewModel
            {
                QuestionNumber = question.QuestionNumber,
                Pseudocode = question.Pseudocode,
                SelectedLanguage = language
            };

            if (TempData["FeedbackMessage"] != null)
            {
                vm.FeedbackMessage = TempData["FeedbackMessage"]?.ToString();
            }

            if (TempData["IsCorrect"] != null)
            {
                vm.IsCorrect = bool.Parse(TempData["IsCorrect"]!.ToString()!);
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(SyntaxGameViewModel vm)
        {
            var question = await _context.SyntaxGameQuestions
                .FirstOrDefaultAsync(q => q.QuestionNumber == vm.QuestionNumber);

            if (question == null)
            {
                vm.FeedbackMessage = "Question not found.";
                return View("Index", vm);
            }

            string correctAnswer = GetCorrectAnswer(question, vm.SelectedLanguage);

            bool isCorrect = NormalizeCode(vm.UserAnswer) == NormalizeCode(correctAnswer);

            if (isCorrect)
            {
                TempData["FeedbackMessage"] = $"Correct. Moving to question {vm.QuestionNumber + 1}.";
                TempData["IsCorrect"] = true;

                return RedirectToAction("Index", new
                {
                    questionNumber = vm.QuestionNumber + 1,
                    language = vm.SelectedLanguage
                });
            }

            vm.Pseudocode = question.Pseudocode;
            vm.FeedbackMessage = "Incorrect. Try again.";
            vm.IsCorrect = false;

            return View("Index", vm);
        }

        private string GetCorrectAnswer(SyntaxGameQuestions question, string language)
        {
            return language.ToLower() switch
            {
                "java" => question.Java ?? "",
                "c#" => question.CSharp ?? "",
                "csharp" => question.CSharp ?? "",
                "python" => question.Python ?? "",
                _ => ""
            };
        }

        private string NormalizeCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return "";

            // Basic normalization:
            // - trim outer whitespace
            // - normalize line endings
            // - trim each line
            var normalized = code
                .Replace("\r\n", "\n")
                .Replace("\r", "\n")
                .Split('\n')
                .Select(line => line.Trim())
                .ToArray();

            return string.Join("\n", normalized).Trim();
        }
    }
}