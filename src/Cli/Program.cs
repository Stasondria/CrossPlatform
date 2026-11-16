using Core.Domain;
using Core.Dto;
using Core.Import;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("=== Сценарій 1: Успішна робота з доменом (Замовлення) ===");
var laptop = Product.Create("p-001", "Ноутбук Lenovo ThinkPad", 28500.50m);
var mouse = Product.Create("p-004", "Бездротова миша Logitech", 1850.75m);

var order = Order.Create("ord-100", "cust-01");
Console.WriteLine($"Створено: {order}");

order.AddLine(laptop.Id, laptop.Name, laptop.Price, 1);
order.AddLine(mouse.Id, mouse.Name, mouse.Price, 2);
Console.WriteLine($"Після додавання товарів: {order}");
foreach (var line in order.Lines)
{
    Console.WriteLine($"   - {line}");
}

order.Confirm();
Console.WriteLine($"Після підтвердження: {order}");

// Перевірка ToDto / FromDto
OrderDto orderDto = order.ToDto();
Order restoredOrder = Order.FromDto(orderDto);
Console.WriteLine($"Відновлено з DTO: {restoredOrder}");
Console.WriteLine();

Console.WriteLine("=== Сценарій 2: Порушення інваріантів ===");
TryDo("Порожній номер замовлення", () => Order.Create("", "CUST-01"));
TryDo("Від'ємна ціна товару", () => Product.Create("P-99", "Брак", -100m));
TryDo("Нульова кількість у рядку", () =>
{
    var draft = Order.Create("ORD-101", "CUST-01");
    draft.AddLine("P-001", "Ноутбук", 1000m, 0);
});
TryDo("Додавання рядка у підтверджене замовлення", () =>
    order.AddLine("P-005", "Навушники", 2000m, 1));
TryDo("Підтвердження порожнього замовлення", () =>
{
    var emptyOrder = Order.Create("ORD-102", "CUST-02");
    emptyOrder.Confirm();
});
TryDo("Повторне підтвердження замовлення", () => order.Confirm());

Console.WriteLine($"\nСтан замовлення після всіх помилкових спроб не змінився:\n{order}");
Console.WriteLine();

// Додаткове завдання #1: Інтеграція з імпортом тижня 3
string csvPath = Path.Combine("data", "sample.csv");
if (File.Exists(csvPath))
{
    Console.WriteLine("=== Додаткове: Перетворення ImportResult -> Доменні сутності ===");
    var importResult = ProductCsvImporter.Load(csvPath);
    var (domainProducts, errors) = DomainMapper.ToDomainProducts(importResult);
    Console.WriteLine($"Валідних доменних товарів: {domainProducts.Count}, Всього помилок: {errors.Count}");
}

static void TryDo(string title, Action action)
{
    try
    {
        action();
        Console.WriteLine($" [ПОМИЛКА ТЕСТУ] {title}: виняток НЕ спрацював!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($" {title}: {ex.GetType().Name} — {ex.Message}");
    }
}