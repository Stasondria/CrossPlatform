using Core.Dto;

namespace Core.Domain;

public sealed class OrderLine
{
    public string ProductId { get; }
    public string ProductName { get; }
    public decimal Price { get; }
    public int Quantity { get; private set; }
    public decimal Subtotal => Price * Quantity;

    private OrderLine(string productId, string productName, decimal price, int quantity)
    {
        ProductId = productId;
        ProductName = productName;
        Price = price;
        Quantity = quantity;
    }

    public static OrderLine Create(string productId, string productName, decimal price, int quantity)
    {
        if (string.IsNullOrWhiteSpace(productId))
            throw new ArgumentException("Ідентифікатор товару в рядку обов'язковий.", nameof(productId));

        if (string.IsNullOrWhiteSpace(productName))
            throw new ArgumentException("Назва товару в рядку обов'язкова.", nameof(productName));

        if (price <= 0)
            throw new ArgumentOutOfRangeException(nameof(price), price, "Ціна товару в замовленні має бути більшою за нуль.");

        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), quantity, "Кількість одиниць товару має бути більшою за нуль.");

        return new OrderLine(productId.Trim().ToUpperInvariant(), productName.Trim(), price, quantity);
    }

    internal void AddQuantity(int additionalQuantity)
    {
        if (additionalQuantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(additionalQuantity), additionalQuantity, "Додаткова кількість має бути більшою за нуль.");

        Quantity += additionalQuantity;
    }

    public OrderLineDto ToDto() => new(ProductId, ProductName, Price, Quantity);

    public static OrderLine FromDto(OrderLineDto dto) => Create(dto.ProductId, dto.ProductName, dto.Price, dto.Quantity);

    public override string ToString() => $"{ProductName} [{ProductId}] x{Quantity} = {Subtotal:F2} грн";
}