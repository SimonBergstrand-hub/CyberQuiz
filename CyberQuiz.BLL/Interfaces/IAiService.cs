using CyberQuiz.BLL.DTOs;

namespace CyberQuiz.BLL.Interfaces
{
    public interface IAiService
    {
        Task<AiResponseDto> GetPersonalFeedbackAsync(string userId);
    }
}
