using MediatR;
using Nesi.Application.DTOs.Auth;

namespace Nesi.Application.Commands.Auth;

public record LoginCommand(string Username, string Password) : IRequest<LoginResponse>;
