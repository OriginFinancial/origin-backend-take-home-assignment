namespace Domain.Models;

public class EligibilityProcessResult
{
    public HashSet<int> ProcessedLines { get; init; } = new();
    public HashSet<int> NonProcessedLines { get; init; } = new();
    public List<string> Errors { get; init; } = new List<string>();
}