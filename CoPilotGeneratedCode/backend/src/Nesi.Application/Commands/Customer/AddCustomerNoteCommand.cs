using MediatR;

namespace Nesi.Application.Commands.Customer;

public record AddCustomerNoteCommand(
    int CustomerId,
    string Note,
    int CreatedByUserId) : IRequest<int>;
