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
        // Q1: definition of simulation
        if (vm.Q1Answer.Contains("model", StringComparison.OrdinalIgnoreCase) ||
            vm.Q1Answer.Contains("simulate", StringComparison.OrdinalIgnoreCase) ||
            vm.Q1Answer.Contains("real", StringComparison.OrdinalIgnoreCase))
        {
            vm.IsQ1Correct = true;
            vm.Feedback1 = "Correct! A simulation models real-world systems.";
        }
        else
        {
            vm.IsQ1Correct = false;
            vm.Feedback1 = "Hint: it represents or models real-world behavior.";
        }

        // Q2: random(1,6)
        if (vm.Q2Answer.Contains("dice", StringComparison.OrdinalIgnoreCase) ||
            vm.Q2Answer.Contains("roll", StringComparison.OrdinalIgnoreCase))
        {
            vm.IsQ2Correct = true;
            vm.Feedback2 = "Correct! It simulates a dice roll.";
        }
        else
        {
            vm.IsQ2Correct = false;
            vm.Feedback2 = "Think about what has 6 possible random outcomes.";
        }

        // Q3: multiple choice
        if (vm.Q3Answer == "dice")
        {
            vm.IsQ3Correct = true;
            vm.Feedback3 = "Correct!";
        }
        else
        {
            vm.IsQ3Correct = false;
            vm.Feedback3 = "A dice roll is a simulation.";
        }

        return View(vm);
    }

    // -----------------------------
    // CHECK EXPLANATION (Step 2)
    // -----------------------------
    if (actionType == "checkExplanation")
    {
        bool isCorrect =
            vm.ExplanationAnswer.Contains("real", StringComparison.OrdinalIgnoreCase) ||
            vm.ExplanationAnswer.Contains("model", StringComparison.OrdinalIgnoreCase) ||
            vm.ExplanationAnswer.Contains("predict", StringComparison.OrdinalIgnoreCase);

        vm.ExplanationCorrect = isCorrect;

        vm.ExplanationFeedback = isCorrect
            ? "Correct! Simulations help model or predict real-world systems."
            : "Try mentioning modeling or predicting real-world behavior.";

        return View(vm);
    }

    // -----------------------------
    // FINAL SUBMIT
    // -----------------------------
    if (actionType == "submit")
    {
        return RedirectToAction("DesigningProgram");
    }

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
        // Q1: designing program
        if (vm.Q1Answer.Contains("plan", StringComparison.OrdinalIgnoreCase) ||
            vm.Q1Answer.Contains("structure", StringComparison.OrdinalIgnoreCase) ||
            vm.Q1Answer.Contains("break", StringComparison.OrdinalIgnoreCase))
        {
            vm.IsQ1Correct = true;
            vm.Feedback1 = "Correct! Designing involves planning and structuring a program.";
        }
        else
        {
            vm.IsQ1Correct = false;
            vm.Feedback1 = "Hint: think about planning before coding.";
        }

        // Q2: modules benefit
        if (vm.Q2Answer.Contains("easy", StringComparison.OrdinalIgnoreCase) ||
            vm.Q2Answer.Contains("manage", StringComparison.OrdinalIgnoreCase) ||
            vm.Q2Answer.Contains("debug", StringComparison.OrdinalIgnoreCase) ||
            vm.Q2Answer.Contains("reuse", StringComparison.OrdinalIgnoreCase))
        {
            vm.IsQ2Correct = true;
            vm.Feedback2 = "Correct! Modules make programs easier to manage and reuse.";
        }
        else
        {
            vm.IsQ2Correct = false;
            vm.Feedback2 = "Think about organization, reuse, or debugging.";
        }

        // Q3: correct option
        if (vm.Q3Answer == "input")
        {
            vm.IsQ3Correct = true;
            vm.Feedback3 = "Correct! Input is part of program structure.";
        }
        else
        {
            vm.IsQ3Correct = false;
            vm.Feedback3 = "Incorrect. Programs typically include input, process, and output.";
        }

        return View(vm);
    }

    // -----------------------------
    // CHECK EXPLANATION (Step 2)
    // -----------------------------
    if (actionType == "checkExplanation")
    {
        bool isCorrect =
            vm.ExplanationAnswer.Contains("plan", StringComparison.OrdinalIgnoreCase) ||
            vm.ExplanationAnswer.Contains("organize", StringComparison.OrdinalIgnoreCase) ||
            vm.ExplanationAnswer.Contains("structure", StringComparison.OrdinalIgnoreCase) ||
            vm.ExplanationAnswer.Contains("manage", StringComparison.OrdinalIgnoreCase);

        vm.ExplanationCorrect = isCorrect;

        vm.ExplanationFeedback = isCorrect
            ? "Correct! Designing helps organize and manage programs effectively."
            : "Try mentioning planning, organizing, or structuring programs.";

        return View(vm);
    }

    // -----------------------------
    // FINAL SUBMIT
    // -----------------------------
    if (actionType == "submit")
    {
        return RedirectToAction("Index", "Lessons");
    }

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
