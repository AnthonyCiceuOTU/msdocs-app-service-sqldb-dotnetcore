using System.Text.Json;
using DotNetCoreSqlDb.Models.AI;
using DotNetCoreSqlDb.Models.Config;
using Google.GenAI;
using Microsoft.Extensions.Options;

namespace DotNetCoreSqlDb.Services
{
    public class GeminiShortAnswerGrader : IAiShortAnswerGrader
    {
        private readonly GeminiOptions _options;
        private readonly ILogger<GeminiShortAnswerGrader> _logger;

        public GeminiShortAnswerGrader(
            IOptions<GeminiOptions> options,
            ILogger<GeminiShortAnswerGrader> logger)
        {
            _options = options.Value;
            _logger = logger;
        }

        public async Task<AiShortAnswerGradeResult> GradeAsync(ShortAnswerEvaluationRequest request)
        {
            _logger.LogInformation("Gemini GradeAsync started.");
            _logger.LogInformation("Gemini model configured: {Model}", _options.Model);
            _logger.LogInformation("Gemini API key present: {HasKey}", !string.IsNullOrWhiteSpace(_options.ApiKey));
            _logger.LogInformation("QuestionText: {QuestionText}", request.QuestionText);
            _logger.LogInformation("StudentAnswer: {StudentAnswer}", request.StudentAnswer);

            if (string.IsNullOrWhiteSpace(_options.ApiKey))
            {
                _logger.LogError("Gemini API key is missing before request is sent.");
                throw new InvalidOperationException("Gemini API key is missing.");
            }

            string prompt = BuildPrompt(request);
            _logger.LogInformation("Gemini prompt built. Prompt length: {PromptLength}", prompt.Length);

            try
            {
                _logger.LogInformation("Creating Gemini client.");
                var client = new Client(apiKey: _options.ApiKey);

                _logger.LogInformation("About to send GenerateContentAsync request to Gemini.");
                var response = await client.Models.GenerateContentAsync(
                    model: _options.Model,
                    contents: prompt);

                _logger.LogInformation("Gemini request completed.");

                string rawText = response.Text ?? "";
                _logger.LogInformation("Gemini raw response length: {Length}", rawText.Length);
                _logger.LogInformation("Gemini raw response text: {RawResponse}", rawText);

                var cleanedText = CleanJsonResponse(rawText);
                _logger.LogInformation("Gemini cleaned response text: {CleanedResponse}", cleanedText);

                var result = JsonSerializer.Deserialize<AiShortAnswerGradeResult>(
                    cleanedText,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                if (result == null)
                {
                    _logger.LogError("Gemini JSON deserialized to null.");
                    throw new Exception("Gemini returned empty JSON.");
                }

                result.Score = Math.Clamp(result.Score, 0, 100);
                result.Feedback ??= "No feedback returned.";
                result.ReasoningSummary ??= "";

                _logger.LogInformation(
                    "Gemini parsed result successfully. IsCorrect: {IsCorrect}, Score: {Score}, Feedback: {Feedback}",
                    result.IsCorrect,
                    result.Score,
                    result.Feedback);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gemini request or parsing failed.");

                return new AiShortAnswerGradeResult
                {
                    IsCorrect = false,
                    Score = 0,
                    Feedback = "We could not evaluate your answer right now. Please try again.",
                    ReasoningSummary = ""
                };
            }
        }

        private static string BuildPrompt(ShortAnswerEvaluationRequest request)
        {
            return $@"
You are grading a student's short-answer response for an educational web app.

Your job:
- Compare the student's answer to the expected answer and grading rubric.
- Be fair to wording differences.
- Accept answers that are conceptually correct even if wording is imperfect.
- Reject answers that are vague, unrelated, or factually wrong.
- Keep feedback short and student-friendly.
- Do not include markdown.
- Return ONLY valid JSON.

Scoring rules:
- 90-100: clearly correct
- 70-89: mostly correct, minor issue
- 40-69: partially correct
- 0-39: incorrect

Mark isCorrect = true if the answer is clearly correct or mostly correct.
Mark isCorrect = false if the answer is only partially correct or incorrect.

Return this exact JSON shape:
{{
  ""isCorrect"": true,
  ""score"": 0,
  ""feedback"": ""string"",
  ""reasoningSummary"": ""string""
}}

Question:
{request.QuestionText}

Expected answer:
{request.ExpectedAnswer}

Grading rubric:
{request.GradingRubric}

Student answer:
{request.StudentAnswer}
";
        }

        private static string CleanJsonResponse(string rawText)
        {
            if (string.IsNullOrWhiteSpace(rawText))
            {
                return "";
            }

            var trimmed = rawText.Trim();

            if (trimmed.StartsWith("```json"))
            {
                trimmed = trimmed.Substring(7).Trim();
            }
            else if (trimmed.StartsWith("```"))
            {
                trimmed = trimmed.Substring(3).Trim();
            }

            if (trimmed.EndsWith("```"))
            {
                trimmed = trimmed.Substring(0, trimmed.Length - 3).Trim();
            }

            return trimmed;
        }
    }
}