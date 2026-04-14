using MediatR;
using Nesi.Domain.Enums;

namespace Nesi.Application.Commands.Auth;

public record RegisterCommand(
    string Username,
    string Email,
    string FirstName,
    string LastName,
    string Password,
    UserRole Role) : IRequest<int>;
