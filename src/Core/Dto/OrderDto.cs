namespace Core.Dto;

public record OrderLineDto(
	string ProductId,
	string ProductName,
	decimal Price,
	int Quantity);

public record OrderDto(
	string Id,
	string CustomerId,
	string Status,
	IReadOnlyList<OrderLineDto> Lines);