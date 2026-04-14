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
    // Normalize inputs
    vm.Q1Answer = vm.Q1Answer?.Trim() ?? "";
    vm.Q2Answer = vm.Q2Answer?.Trim() ?? "";
    vm.Q3Answer = vm.Q3Answer?.Trim() ?? "";
    vm.ExplanationAnswer = vm.ExplanationAnswer?.Trim() ?? "";

    // -----------------------------
    // HINT
    // -----------------------------
    if (actionType == "hint")
    {
        vm.ShowHint = true;
        vm.ShowSolution = false;
        return View(vm);
    }

    // -----------------------------
    // SOLUTION
    // -----------------------------
    if (actionType == "solution")
    {
        vm.ShowHint = false;
        vm.ShowSolution = true;
        return View(vm);
    }

    // -----------------------------
    // CHECK QUIZ (Step 1)
    // -----------------------------
    if (actionType == "check")
    {
        // Q1: average explanation
        if (vm.Q1Answer.Contains("divide", StringComparison.OrdinalIgnoreCase) &&
            (vm.Q1Answer.Contains("total", StringComparison.OrdinalIgnoreCase) ||
             vm.Q1Answer.Contains("sum", StringComparison.OrdinalIgnoreCase)))
        {
            vm.IsQ1Correct = true;
            vm.Feedback1 = "Correct! Add values, then divide by count.";
        }
        else
        {
            vm.IsQ1Correct = false;
            vm.Feedback1 = "Hint: you need both total AND division.";
        }

        // Q2: total
        if (vm.Q2Answer == "20")
        {
            vm.IsQ2Correct = true;
            vm.Feedback2 = "Correct!";
        }
        else
        {
            vm.IsQ2Correct = false;
            vm.Feedback2 = "Not quite. Add all numbers together.";
        }

        // Q3: operation
        if (vm.Q3Answer == "divide")
        {
            vm.IsQ3Correct = true;
            vm.Feedback3 = "Correct!";
        }
        else
        {
            vm.IsQ3Correct = false;
            vm.Feedback3 = "Average requires division.";
        }

        return View(vm);
    }

    // -----------------------------
    // CHECK EXPLANATION (Step 2)
    // -----------------------------
    if (actionType == "checkExplanation")
    {
        bool isCorrect =
            vm.ExplanationAnswer.Contains("data", StringComparison.OrdinalIgnoreCase) &&
            (vm.ExplanationAnswer.Contains("useful", StringComparison.OrdinalIgnoreCase) ||
             vm.ExplanationAnswer.Contains("information", StringComparison.OrdinalIgnoreCase) ||
             vm.ExplanationAnswer.Contains("process", StringComparison.OrdinalIgnoreCase));

        vm.ExplanationCorrect = isCorrect;

        vm.ExplanationFeedback = isCorrect
            ? "Correct! Data processing turns raw data into useful information."
            : "Try mentioning turning raw data into useful information.";

        return View(vm);
    }

    // -----------------------------
    // FINAL SUBMIT
    // -----------------------------
    if (actionType == "submit")
    {
        // You could track completion here later
        return RedirectToAction("Simulation");
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
