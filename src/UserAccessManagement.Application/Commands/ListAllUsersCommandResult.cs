using UserAccessManagement.Application.Base;
using UserAccessManagement.Application.Models;

namespace UserAccessManagement.Application.Commands;

public record ListAllUsersCommandResult(bool Success, string Message, IEnumerable<UserModel> Users)
    : CommandResult(Success, Message);