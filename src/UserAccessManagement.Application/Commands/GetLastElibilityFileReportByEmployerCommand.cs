﻿using System.Text.Json.Serialization;
using UserAccessManagement.Application.Base;

namespace UserAccessManagement.Application.Commands;

public record GetLastElibilityFileReportByEmployerCommand([property: JsonPropertyName("employer_name")] string EmployerName)
    : ICommand<GetLastElibilityFileReportByEmployerCommandResult>
{
    public string? ValidationMessages { get; private set; }

    public bool Validate()
    {
        var valid = true;

        if (string.IsNullOrEmpty(EmployerName))
        {
            valid = false;
            ValidationMessages = $"{nameof(EmployerName)} is required.";
        }

        return valid;
    }
}