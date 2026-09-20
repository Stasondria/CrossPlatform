using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;

// Встановлюємо кодування UTF-8 для консолі
Console.OutputEncoding = System.Text.Encoding.UTF8;

// Збір даних про середовище
var envInfo = new
{
    Student = "Маліновський Станіслав, група ФЕІ-35",
    OsDescription = RuntimeInformation.OSDescription,
    OsVersion = Environment.OSVersion.ToString(),
    ProcessArchitecture = RuntimeInformation.ProcessArchitecture.ToString(),
    DotNetVersion = Environment.Version.ToString(),
    Runtime = RuntimeInformation.FrameworkDescription,
    AppDirectory = AppContext.BaseDirectory,
    CurrentDirectory = Environment.CurrentDirectory,
    Domain = "Замовлення (Customer, Product, Order, OrderLine)"
};

// Додаткове завдання: якщо є прапорець --json, виводити JSON
if (args.Contains("--json", StringComparer.OrdinalIgnoreCase))
{
    var options = new JsonSerializerOptions
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic)
    };
    Console.WriteLine(JsonSerializer.Serialize(envInfo, options));
}
else
{
    Console.WriteLine("CrossApp — практикум з крос-платформного програмування");
    Console.WriteLine($"Студент: {envInfo.Student}");
    Console.WriteLine(new string('-', 52));
    Console.WriteLine($"ОС (OSDescription)    : {envInfo.OsDescription}");
    Console.WriteLine($"ОС (Environment)      : {envInfo.OsVersion}");
    Console.WriteLine($"Архітектура процесу   : {envInfo.ProcessArchitecture}");
    Console.WriteLine($"Версія .NET (CLR)     : {envInfo.DotNetVersion}");
    Console.WriteLine($"Runtime               : {envInfo.Runtime}");
    Console.WriteLine($"Каталог застосунку   : {envInfo.AppDirectory}");
    Console.WriteLine($"Поточний каталог      : {envInfo.CurrentDirectory}");
    Console.WriteLine(new string('-', 52));
    Console.WriteLine($"Предметна область: {envInfo.Domain}");
}