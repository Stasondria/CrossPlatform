using Core.Dto;
using Core.Import;

Console.OutputEncoding = System.Text.Encoding.UTF8;

// Отримання шляху до файлу з аргументів або за замовчуванням
string path = args.Length > 0 ? args[0] : Path.Combine("data", "sample.csv");

if (!File.Exists(path))
{
    Console.WriteLine($"Помилка: Файл не знайдено за шляхом: {Path.GetFullPath(path)}");
    return 1;
}

ImportResult<ProductDto> result;
string extension = Path.GetExtension(path).ToLowerInvariant();

// Вибір імпортера за розширенням файлу через switch expression
result = extension switch
{
    ".csv" => ProductCsvImporter.Load(path),
    ".json" => ProductJsonImporter.Load(path),
    _ => new ImportResult<ProductDto>([], [$"Формат '{extension}' не підтримується."])
};

Console.WriteLine($"=== Звіт імпорту даних ({Path.GetFileName(path)}) ===");
Console.WriteLine($"Завантажено записів : {result.Items.Count}");
Console.WriteLine($"Оброблено рядків   : {result.TotalProcessedLines}");
Console.WriteLine($"Пропущено рядків   : {result.Errors.Count}");
Console.WriteLine($"Відсоток помилок   : {result.ErrorRate}%");
Console.WriteLine(new string('-', 55));

Console.WriteLine("Перші завантажені позиції:");
foreach (ProductDto p in result.Items.Take(5))
{
    Console.WriteLine($" {p.Id,-6} | {p.Name,-30} | {p.Price,9:F2} грн.");
}

if (result.Errors.Count > 0)
{
    Console.WriteLine(new string('-', 55));
    Console.WriteLine("Перелік помилок:");
    foreach (string err in result.Errors)
    {
        Console.WriteLine($" ! {err}");
    }
}

return 0;