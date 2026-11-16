namespace Core.Dto;

// Record DTO для представлення товару в предметній області "Замовлення"
public record ProductDto(
    string Id,
    string Name,
    decimal Price,
    string? Category = null);