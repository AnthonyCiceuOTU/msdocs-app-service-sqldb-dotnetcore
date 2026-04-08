using DotNetCoreSqlDb.Models.AI;

namespace DotNetCoreSqlDb.Services
{
    public interface IAiShortAnswerGrader
    {
        Task<AiShortAnswerGradeResult> GradeAsync(ShortAnswerEvaluationRequest request);
    }
}