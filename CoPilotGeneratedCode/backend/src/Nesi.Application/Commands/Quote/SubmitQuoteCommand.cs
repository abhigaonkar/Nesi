using MediatR;

namespace Nesi.Application.Commands.Quote;

public record SubmitQuoteCommand(int QuoteId) : IRequest<Unit>;
