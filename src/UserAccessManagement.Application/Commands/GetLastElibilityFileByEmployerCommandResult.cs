﻿using UserAccessManagement.Application.Base;
using UserAccessManagement.Application.Models;

namespace UserAccessManagement.Application.Commands;

public record GetLastElibilityFileByEmployerCommandResult(bool Success, string Message, EligibilityFileModel? ElibilityFile) 
    : CommandResult(Success, Message);