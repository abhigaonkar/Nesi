using MediatR;
using Nesi.Domain.Interfaces;
using Nesi.Domain.Entities;

namespace Nesi.Application.Commands.Auth;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;

    public RegisterCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        // Check if username already exists
        var existingUsers = await _unitOfWork.Repository<User>().GetAllAsync();
        if (existingUsers.Any(u => u.Username == request.Username))
        {
            throw new InvalidOperationException("Username already exists");
        }

        // Check if email already exists
        if (existingUsers.Any(u => u.Email == request.Email))
        {
            throw new InvalidOperationException("Email already exists");
        }

        // Hash password
        var hashedPassword = HashPassword(request.Password);

        // Create new user
        var user = new User(
            request.Username,
            request.Email,
            request.FirstName,
            request.LastName,
            hashedPassword,
            request.Role);

        await _unitOfWork.Repository<User>().AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return user.Id;
    }

    private static string HashPassword(string password)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var bytes = System.Text.Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}
