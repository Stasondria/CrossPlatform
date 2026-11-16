# CrossApp

Наскрізний проєкт з курсу «Крос-платформне програмування».  
**Предметна область:** Замовлення (`Order`, `OrderLine`, `Product`, `CustomerDto`).  
**Призначення:** Оформлення замовлень покупцями, імпорт товарних номенклатур із CSV/JSON, контроль бізнес-інваріантів та розрахунок підсумкової вартості замовлення.

## Структура Solution

- `src/Core/` — бібліотека класів (Class Library):
  - `Dto/` — незмінні `record`-типи (`ProductDto`, `CustomerDto`, `OrderDto`, `OrderLineDto`, `ImportResult<T>`).
  - `Domain/` — доменна модель з інваріантами та поведінкою (`Order`, `OrderLine`, `Product`, `OrderStatus`, `DomainMapper`).
  - `Import/` — парсери вхідних даних (`ProductCsvImporter`, `ProductJsonImporter`).
  - `EnvironmentInfo.cs` — збір інформації про середовище виконання.
- `src/Cli/` — консольний застосунок (`Program.cs`), точка входу для демонстрації роботи домену та виводу результатів.
- `data/` — вхідні файли (`sample.csv`, `sample.json`).

## Запуск

1. Збірка проєкту:
   ```bash
   dotnet build