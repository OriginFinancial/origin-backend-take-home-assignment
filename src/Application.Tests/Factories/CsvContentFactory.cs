using Application.DTOs;
using Bogus;

namespace Application.Tests.Factories;


public static class CsvContentFactory
{
    public static (string, List<CsvLineModel>) GenerateCsvContent(int numberOfLines)
    {
        var faker = new Faker<CsvLineModel>()
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.FullName, f => f.Name.FullName())
            .RuleFor(u => u.Country, f => f.Address.Country())
            .RuleFor(u => u.BirthDate, f => f.Date.Past(30).ToString("MM/dd/yyyy"))
            .RuleFor(u => u.Salary, f => f.Random.Decimal(30000, 100000));

        var csvLines = new List<string> { "Email,FullName,Country,BirthDate,Salary" };
        var models = new List<CsvLineModel>();
        for (int i = 0; i < numberOfLines; i++)
        {
            var line = faker.Generate();
            models.Add(line);
            csvLines.Add($"{line.Email},{line.FullName},{line.Country},{line.BirthDate},{line.Salary}");
        }

        return (string.Join("\n", csvLines), models);
    }
}