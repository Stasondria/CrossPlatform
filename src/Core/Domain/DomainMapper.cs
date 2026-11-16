using Core.Dto;

namespace Core.Domain;

public static class DomainMapper
{
    public static (IReadOnlyList<Product> Products, IReadOnlyList<string> Errors) ToDomainProducts(
        ImportResult<ProductDto> importResult)
    {
        var validProducts = new List<Product>();
        var domainErrors = new List<string>(importResult.Errors);

        foreach (var dto in importResult.Items)
        {
            try
            {
                validProducts.Add(Product.FromDto(dto));
            }
            catch (Exception ex)
            {
                domainErrors.Add($"Доменна помилка для '{dto.Id}': {ex.Message}");
            }
        }

        return (validProducts.AsReadOnly(), domainErrors.AsReadOnly());
    }
}