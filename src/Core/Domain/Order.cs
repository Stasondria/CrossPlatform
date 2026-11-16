using Core.Dto;

namespace Core.Domain;

public sealed class Order
{
    private readonly List<OrderLine> _lines = [];

    public string Id { get; }
    public string CustomerId { get; }
    public OrderStatus Status { get; private set; }
    public IReadOnlyList<OrderLine> Lines => _lines.AsReadOnly();
    public decimal Total => _lines.Sum(line => line.Subtotal);

    private Order(string id, string customerId, OrderStatus status = OrderStatus.Draft)
    {
        Id = id;
        CustomerId = customerId;
        Status = status;
    }

    public static Order Create(string id, string customerId)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Номер замовлення обов'язковий.", nameof(id));

        if (string.IsNullOrWhiteSpace(customerId))
            throw new ArgumentException("Ідентифікатор клієнта обов'язковий.", nameof(customerId));

        return new Order(id.Trim().ToUpperInvariant(), customerId.Trim().ToUpperInvariant());
    }

    public void AddLine(string productId, string productName, decimal price, int quantity)
    {
        if (Status != OrderStatus.Draft)
            throw new InvalidOperationException($"Неможливо змінити замовлення {Id} у статусі {Status}.");

        // Створюємо рядок через фабрику (вона перевірить валідність price та quantity)
        var newLine = OrderLine.Create(productId, productName, price, quantity);

        var existingLine = _lines.FirstOrDefault(l => l.ProductId == newLine.ProductId);
        if (existingLine is not null)
        {
            existingLine.AddQuantity(quantity);
        }
        else
        {
            _lines.Add(newLine);
        }
    }

    public void Confirm()
    {
        if (_lines.Count == 0)
            throw new InvalidOperationException($"Неможливо підтвердити порожнє замовлення {Id}.");

        ChangeStatus(OrderStatus.Confirmed);
    }

    public void Cancel()
    {
        ChangeStatus(OrderStatus.Cancelled);
    }

    // Додаткове завдання #3: перевірка допустимих переходів станів через switch expression
    private void ChangeStatus(OrderStatus targetStatus)
    {
        Status = (Status, targetStatus) switch
        {
            (OrderStatus.Draft, OrderStatus.Confirmed) => OrderStatus.Confirmed,
            (OrderStatus.Draft, OrderStatus.Cancelled) => OrderStatus.Cancelled,
            (OrderStatus.Confirmed, OrderStatus.Cancelled) => OrderStatus.Cancelled,
            _ => throw new InvalidOperationException($"Недопустимий перехід статусу замовлення {Id}: {Status} -> {targetStatus}.")
        };
    }

    public OrderDto ToDto() => new(
        Id,
        CustomerId,
        Status.ToString(),
        _lines.Select(l => l.ToDto()).ToList());

    public static Order FromDto(OrderDto dto)
    {
        var order = Create(dto.Id, dto.CustomerId);

        foreach (var lineDto in dto.Lines)
        {
            order.AddLine(lineDto.ProductId, lineDto.ProductName, lineDto.Price, lineDto.Quantity);
        }

        if (!Enum.TryParse<OrderStatus>(dto.Status, ignoreCase: true, out var parsedStatus))
            throw new ArgumentException($"Невідомий статус замовлення: '{dto.Status}'.", nameof(dto));

        if (parsedStatus == OrderStatus.Confirmed)
            order.Confirm();
        else if (parsedStatus == OrderStatus.Cancelled)
            order.Cancel();

        return order;
    }

    public override string ToString() =>
        $"Замовлення {Id} (Клієнт: {CustomerId}) | Статус: {Status} | Позицій: {_lines.Count} | Сума: {Total:F2} грн";
}