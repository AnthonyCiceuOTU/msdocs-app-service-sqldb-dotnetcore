using Microsoft.AspNetCore.Mvc;
using DotNetCoreSqlDb.ViewModels;
using System;

namespace DotNetCoreSqlDb.Controllers
{
    public class UnitEightController : Controller
    {
        // -----------------------------
        // Lesson 33 — Data Processing
        // -----------------------------
        [HttpGet]
        public IActionResult DataProcessing()
        {
            return View(new DataProcessingViewModel());
        }

[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult DataProcessing(DataProcessingViewModel vm, string actionType)
{
    if (actionType == "next")
    {
        vm.CurrentStep++;
        return View(vm);
    }

    if (actionType == "prev")
    {
        vm.CurrentStep--;
        return View(vm);
    }

    if (actionType == "check")
    {
        int score = 0;

        // Q1
        if (!string.IsNullOrEmpty(vm.Q1Answer) &&
            vm.Q1Answer.ToLower().Contains("divide"))
        {
            score++;
        }

        // Q2
        if (vm.Q2Answer == "20")
        {
            score++;
        }

        // Q3
        if (vm.Q3Answer == "divide")
        {
            score++;
        }

        vm.IsCorrect = score == 3;
        vm.FeedbackMessage = $"You got {score}/3 correct.";

        vm.CurrentStep = 3;
    }

    return View(vm);
}
        // -----------------------------
        // Lesson 34 — Simulation
        // -----------------------------
        [HttpGet]
        public IActionResult Simulation()
        {
            return View(new SimulationViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Simulation(SimulationViewModel vm, string actionType)
        {
            vm.UserAnswer = vm.UserAnswer?.Trim() ?? "";

            if (actionType == "hint")
            {
                vm.ShowHint = true;
                vm.ShowSolution = false;
                vm.IsCorrect = null;
                vm.FeedbackMessage = "Hint: think about modelling real systems like dice rolls or weather.";
                return View(vm);
            }

            if (actionType == "solution")
            {
                vm.ShowHint = false;
                vm.ShowSolution = true;
                vm.IsCorrect = null;
                vm.FeedbackMessage = "Solution shown below.";
                return View(vm);
            }

            bool isCorrect =
                vm.UserAnswer.Contains("dice", StringComparison.OrdinalIgnoreCase) ||
                vm.UserAnswer.Contains("random", StringComparison.OrdinalIgnoreCase) ||
                vm.UserAnswer.Contains("model", StringComparison.OrdinalIgnoreCase);

            vm.IsCorrect = isCorrect;
            vm.ShowHint = false;
            vm.ShowSolution = false;

            vm.FeedbackMessage = isCorrect
                ? "Correct! Simulations model real-world systems like dice rolls."
                : "Not quite. Think about randomness and modelling real systems.";

            return View(vm);
        }

        // -----------------------------
        // Lesson 35 — Designing a Program
        // -----------------------------
        [HttpGet]
        public IActionResult DesigningProgram()
        {
            return View(new DesigningProgramViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DesigningProgram(DesigningProgramViewModel vm, string actionType)
        {
            vm.UserAnswer = vm.UserAnswer?.Trim() ?? "";

            if (actionType == "hint")
            {
                vm.ShowHint = true;
                vm.ShowSolution = false;
                vm.IsCorrect = null;
                vm.FeedbackMessage = "Hint: think about breaking a problem into smaller parts (modules).";
                return View(vm);
            }

            if (actionType == "solution")
            {
                vm.ShowHint = false;
                vm.ShowSolution = true;
                vm.IsCorrect = null;
                vm.FeedbackMessage = "Solution shown below.";
                return View(vm);
            }

            bool isCorrect =
                vm.UserAnswer.Contains("module", StringComparison.OrdinalIgnoreCase) ||
                vm.UserAnswer.Contains("planning", StringComparison.OrdinalIgnoreCase) ||
                vm.UserAnswer.Contains("break", StringComparison.OrdinalIgnoreCase);

            vm.IsCorrect = isCorrect;
            vm.ShowHint = false;
            vm.ShowSolution = false;

            vm.FeedbackMessage = isCorrect
                ? "Correct! Designing a program involves breaking problems into modules."
                : "Not quite. Think about planning and splitting problems into parts.";

            return View(vm);
        }

        // -----------------------------
        // Index
        // -----------------------------
        public IActionResult Index()
        {
            return View();
        }
    }
}
