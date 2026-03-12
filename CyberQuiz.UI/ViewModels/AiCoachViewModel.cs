namespace CyberQuiz.UI.ViewModels;

public class AiCoachViewModel
{
    public string Feedback { get; set; } = string.Empty;
    public DateTime GeneratedAt { get; set; }
    public bool IsError { get; set; }
}