using MediatR;

namespace Nesi.Application.Commands.Quote;

public record ConvertQuoteToWorkOrderCommand(int QuoteId) : IRequest<int>;
