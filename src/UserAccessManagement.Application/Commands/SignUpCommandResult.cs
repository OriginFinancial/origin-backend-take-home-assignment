﻿using UserAccessManagement.Application.Base;
using UserAccessManagement.Application.Models;

namespace UserAccessManagement.Application.Commands;

public record SignUpCommandResult(bool Success, string Message, UserModel? User)
    : CommandResult(Success, Message);