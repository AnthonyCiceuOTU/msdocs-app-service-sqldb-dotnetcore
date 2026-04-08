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
            if (string.IsNullOrWhiteSpace(_options.ApiKey))
            {
                throw new InvalidOperationException("Gemini API key is missing.");
            }

            var client = new Client(apiKey: _options.ApiKey);

            string prompt = BuildPrompt(request);

            var response = await client.Models.GenerateContentAsync(
                model: _options.Model,
                contents: prompt);

            string rawText = response.Text ?? "";

            _logger.LogInformation("Gemini grading raw response: {RawResponse}", rawText);

            try
            {
                var result = JsonSerializer.Deserialize<AiShortAnswerGradeResult>(
                    rawText,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                if (result == null)
                {
                    throw new Exception("Gemini returned empty JSON.");
                }

                result.Score = Math.Clamp(result.Score, 0, 100);
                result.Feedback ??= "No feedback returned.";
                result.ReasoningSummary ??= "";

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to parse Gemini grading response.");

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
    }
}