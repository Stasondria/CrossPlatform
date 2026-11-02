using System.Text.Json;
using Core.Dto;

namespace Core.Import;

public static class ProductJsonImporter
{
    public static ImportResult<ProductDto> Load(string path)
    {
        try
        {
            string json = File.ReadAllText(path);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            List<ProductDto> items = JsonSerializer.Deserialize<List<ProductDto>>(json, options) ?? [];
            return new ImportResult<ProductDto>(items, new List<string>(), items.Count);
        }
        catch (Exception ex)
        {
            return new ImportResult<ProductDto>(new List<ProductDto>(), new[] { $"Помилка JSON: {ex.Message}" }, 0);
        }
    }
}