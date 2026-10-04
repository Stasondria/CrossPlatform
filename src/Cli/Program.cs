using System;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Core;

Console.OutputEncoding = System.Text.Encoding.UTF8;

EnvironmentReport report = EnvironmentInfo.Collect();

var outputData = new
{
    Student = "Маліновський Станіслав, група ФЕІ-35",
    OsDescription = report.OsDescription,
    Runtime = report.FrameworkDescription,
    Architecture = report.ProcessArchitecture,
    DetectedRid = report.DetectedRid,
    ReportedRid = report.ReportedRid,
    AppDirectory = report.BaseDirectory,
    CurrentDirectory = Environment.CurrentDirectory,
    TfmNote = report.TargetFrameworkNote,
    Domain = "Замовлення (Customer, Product, Order, OrderLine)"
};

if (args.Contains("--json", StringComparer.OrdinalIgnoreCase))
{
    var options = new JsonSerializerOptions
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic)
    };
    Console.WriteLine(JsonSerializer.Serialize(outputData, options));
}
else
{
    Console.WriteLine("CrossApp — практикум з крос-платформного програмування");
    Console.WriteLine($"Студент             : {outputData.Student}");
    Console.WriteLine(new string('-', 55));
    Console.WriteLine($"ОС                  : {report.OsDescription}");
    Console.WriteLine($"Runtime             : {report.FrameworkDescription}");
    Console.WriteLine($"Архітектура         : {report.ProcessArchitecture}");
    Console.WriteLine($"RID (визначено)     : {report.DetectedRid}");
    Console.WriteLine($"RID (від .NET)      : {report.ReportedRid}");
    Console.WriteLine($"TFM примітка        : {report.TargetFrameworkNote}");
    Console.WriteLine($"Каталог застосунку : {report.BaseDirectory}");
    Console.WriteLine($"Поточний каталог    : {Environment.CurrentDirectory}");
    Console.WriteLine(new string('-', 55));
    Console.WriteLine($"Предметна область   : {outputData.Domain}");
}