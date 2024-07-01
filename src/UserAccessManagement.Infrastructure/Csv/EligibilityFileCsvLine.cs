using CsvHelper.Configuration.Attributes;

namespace UserAccessManagement.Infrastructure.Csv;

public class EligibilityFileCsvLine
{
    [Name("email")]
    public string? Email { get; set; }

    [Name("full_name")]
    public string? FullName { get; set; }

    [Name("country")]
    public string? Country { get; set; }

    [Name("birth_date")]
    public DateTime? BirthDate { get; set; }

    [Name("salary")]
    public decimal? Salary { get; set; }

    [Ignore]
    public string? RawContent { get; set; }

    [Ignore]
    public string? ErrorMessage { get; set; }

    public bool HasError() => !string.IsNullOrWhiteSpace(ErrorMessage);
}