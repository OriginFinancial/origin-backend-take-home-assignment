﻿using System.ComponentModel;

namespace UserAccessManagement.Domain.Enums;

public enum ElegibilityFileStatus
{
    [Description("Processing")]
    Processing = 1,

    [Description("Processed")]
    Processed = 2,

    [Description("Processed with Errors")]
    ProcessedWithErrors = 3
}