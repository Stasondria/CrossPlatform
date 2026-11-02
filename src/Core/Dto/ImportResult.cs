namespace Core.Dto;

// Узагальнений record результату імпорту з розкриттям статистики
public sealed record ImportResult<T>(
    IReadOnlyList<T> Items,
    IReadOnlyList<string> Errors,
    int TotalProcessedLines = 0)
{
    // Додаткове завдання #3: Розрахунок відсотка помилок
    public double ErrorRate => TotalProcessedLines == 0
        ? 0
        : Math.Round((double)Errors.Count / TotalProcessedLines * 100, 1);
}