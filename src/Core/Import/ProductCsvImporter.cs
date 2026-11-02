using System.Globalization;
using System.Text;
using Core.Dto;

namespace Core.Import;

public static class ProductCsvImporter
{
	private const char Separator = ';';

	public static ImportResult<ProductDto> Load(string path)
	{
		var items = new List<ProductDto>();
		var errors = new List<string>();

		string[] lines = File.ReadAllLines(path, Encoding.UTF8);
		int totalProcessed = 0;

		for (int i = 0; i < lines.Length; i++)
		{
			int number = i + 1;
			string line = lines[i];

			// Пропуск порожніх рядків, коментарів та заголовка
			if (string.IsNullOrWhiteSpace(line) || line.StartsWith('#'))
				continue;

			if (number == 1 && line.StartsWith("id", StringComparison.OrdinalIgnoreCase))
				continue;

			totalProcessed++;

			switch (ParseLine(line))
			{
				case ParseOk ok:
					items.Add(ok.Value);
					break;
				case ParseCustomerOk customerOk:
					// Пропускаємо запис клієнта або реєструємо як спеціальну примітку
					break;
				case ParseFailed failed:
					errors.Add($"рядок {number}: {failed.Reason}");
					break;
			}
		}

		return new ImportResult<ProductDto>(items, errors, totalProcessed);
	}

	private static ParseOutcome ParseLine(string line)
	{
		string[] parts = line.Split(Separator, StringSplitOptions.TrimEntries);

		return parts switch
		{
			// Реляційний патерн і патерн властивостей
			{ Length: < 3 } => new ParseFailed($"очікую 3 колонки, отримав {parts.Length}"),

			// Патерн списку та логічний патерн or (перевірка на порожні назви чи Id)
			["", _, _] or [_, "", _] => new ParseFailed("ID або назва товару є порожніми"),

			// Додаткове завдання #2: Розпізнавання запису клієнта за префіксом C-
			[var id, var name, _] when id.StartsWith("C-", StringComparison.OrdinalIgnoreCase)
				=> new ParseCustomerOk(new CustomerDto(id, name)),

			// Охоронна умова when та TryParse з CultureInfo.InvariantCulture
			[var id, var name, var priceStr] when !decimal.TryParse(priceStr, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal p) || p < 0
				=> new ParseFailed($"ціна '{priceStr}' не є коректним невід'ємним числом"),

			// Патерн списку з деконструкцією 3-х колонок
			[var id, var name, var priceStr]
				=> new ParseOk(new ProductDto(id, name, decimal.Parse(priceStr, CultureInfo.InvariantCulture))),

			// Гілка за замовчуванням
			_ => new ParseFailed($"занадто багато колонок: {parts.Length}")
		};
	}

	private abstract record ParseOutcome;
	private sealed record ParseOk(ProductDto Value) : ParseOutcome;
	private sealed record ParseCustomerOk(CustomerDto Customer) : ParseOutcome;
	private sealed record ParseFailed(string Reason) : ParseOutcome;
}