using MediatR;
using Nesi.Application.DTOs.Auth;
using Nesi.Domain.Interfaces;

namespace Nesi.Application.Commands.Auth;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public LoginCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // Find user by username
        var users = await _unitOfWork.Repository<Domain.Entities.User>()
            .GetAllAsync();
        
        var user = users.FirstOrDefault(u => u.Username == request.Username);

        if (user == null || !user.IsActive)
        {
            throw new UnauthorizedAccessException("Invalid username or password");
        }

        // Verify password (using simple SHA256 hash for demo)
        var hashedPassword = HashPassword(request.Password);
        if (user.PasswordHash != hashedPassword)
        {
            throw new UnauthorizedAccessException("Invalid username or password");
        }

        // Generate a simple token (in production, use JWT)
        var token = GenerateToken(user.Id, user.Username);

        return new LoginResponse
        {
            UserId = user.Id,
            Username = user.Username,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = user.Role,
            Token = token
        };
    }

    private static string HashPassword(string password)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var bytes = System.Text.Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    private static string GenerateToken(int userId, string username)
    {
        // Simple token for demo purposes
        // In production, use JWT with proper signing
        var tokenData = $"{userId}:{username}:{DateTime.UtcNow:O}";
        var bytes = System.Text.Encoding.UTF8.GetBytes(tokenData);
        return Convert.ToBase64String(bytes);
    }
}
