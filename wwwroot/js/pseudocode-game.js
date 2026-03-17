document.addEventListener("DOMContentLoaded", function () {
    const checkButton = document.getElementById("checkSolutionBtn");
    const hintButton = document.getElementById("hintBtn");
    const solutionButton = document.getElementById("solutionBtn");
    const answerBox = document.getElementById("studentAnswer");
    const feedbackBox = document.getElementById("feedbackBox");

    if (!checkButton || !answerBox || !feedbackBox) {
        return;
    }

    function normalizeText(value) {
        return value
            .replace(/\r\n/g, "\n")
            .replace(/\s+/g, " ")
            .trim()
            .toLowerCase();
    }

    function showMessage(message, type) {
        feedbackBox.textContent = message;
        feedbackBox.classList.remove("success", "error", "hint", "solution");
        feedbackBox.classList.add("show");
        feedbackBox.classList.add(type);
    }

    checkButton.addEventListener("click", function () {
        const expected = checkButton.dataset.expected || "";
        const currentText = answerBox.value || "";

        if (!currentText.trim()) {
            showMessage("Type your answer first.", "error");
            return;
        }

        const isCorrect =
            normalizeText(currentText).includes(normalizeText(expected));

        if (isCorrect) {
            showMessage("Correct! Nice work.", "success");
        } else {
            showMessage("Not quite. Try using the hint or check the required value again.", "error");
        }
    });

    if (hintButton) {
        hintButton.addEventListener("click", function () {
            const hint = hintButton.dataset.hint || "No hint available.";
            showMessage(`Hint: ${hint}`, "hint");
        });
    }

    if (solutionButton) {
        solutionButton.addEventListener("click", function () {
            const solution = solutionButton.dataset.solution || "";
            answerBox.value = solution;
            showMessage("Solution inserted into the code box.", "solution");
        });
    }
});