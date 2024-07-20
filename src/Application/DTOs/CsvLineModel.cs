using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class CsvLineModel
{
    [Required]
    public string Email { get; set; }
    public string? FullName { get; set; }
    [Required]
    public string Country { get; set; }
    public string BirthDate { get; set; }
    public decimal? Salary { get; set; }
}