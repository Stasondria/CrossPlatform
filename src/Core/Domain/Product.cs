using Core.Dto;

namespace Core.Domain;

public sealed class Product
{
    public string Id { get; }
    public string Name { get; }
    public decimal Price { get; private set; }

    private Product(string id, string name, decimal price)
    {
        Id = id;
        Name = name;
        Price = price;
    }

    public static Product Create(string id, string name, decimal price)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Ідентифікатор товару обов'язковий.", nameof(id));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Назва товару не може бути порожньою.", nameof(name));

        if (price < 0)
            throw new ArgumentOutOfRangeException(nameof(price), price, "Ціна товару не може бути від'ємною.");

        return new Product(id.Trim().ToUpperInvariant(), name.Trim(), price);
    }

    public void ChangePrice(decimal newPrice)
    {
        if (newPrice < 0)
            throw new ArgumentOutOfRangeException(nameof(newPrice), newPrice, "Нова ціна не може бути від'ємною.");

        Price = newPrice;
    }

    public ProductDto ToDto() => new(Id, Name, Price);

    public static Product FromDto(ProductDto dto) => Create(dto.Id, dto.Name, dto.Price);

    public override string ToString() => $"{Id}: {Name} ({Price:F2} грн)";
}