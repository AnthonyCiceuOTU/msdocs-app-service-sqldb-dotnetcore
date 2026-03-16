using Microsoft.AspNetCore.Mvc;
using DotNetCoreSqlDb.ViewModels;

namespace DotNetCoreSqlDb.Controllers
{
    public class UnitOneController : Controller
    {
        [HttpGet]
        public IActionResult Algorithms()
        {
            return View(new AlgorithmsViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Algorithms(AlgorithmsViewModel vm, string actionType)
        {
            vm.UserAnswer1 = vm.UserAnswer1?.Trim() ?? "";
            vm.UserAnswer2 = vm.UserAnswer2?.Trim() ?? "";

            if (actionType == "hint")
            {
                vm.ShowHint = true;
                vm.ShowSolution = false;
                vm.FeedbackMessage = "Hint: use the variables that store the two prices.";
                vm.IsCorrect = null;
                return View(vm);
            }

            if (actionType == "solution")
            {
                vm.ShowHint = false;
                vm.ShowSolution = true;
                vm.FeedbackMessage = "Solution shown below.";
                vm.IsCorrect = null;
                return View(vm);
            }

            bool isCorrect =
                (vm.UserAnswer1.Equals("price1", StringComparison.OrdinalIgnoreCase) &&
                 vm.UserAnswer2.Equals("price2", StringComparison.OrdinalIgnoreCase))
                ||
                (vm.UserAnswer1.Equals("price2", StringComparison.OrdinalIgnoreCase) &&
                 vm.UserAnswer2.Equals("price1", StringComparison.OrdinalIgnoreCase));

            vm.IsCorrect = isCorrect;
            vm.ShowHint = false;
            vm.ShowSolution = false;
            vm.FeedbackMessage = isCorrect
                ? "Correct! The algorithm adds the two item prices together."
                : "Not quite. Try using the two variables defined above.";

            return View(vm);
        }
    }
}