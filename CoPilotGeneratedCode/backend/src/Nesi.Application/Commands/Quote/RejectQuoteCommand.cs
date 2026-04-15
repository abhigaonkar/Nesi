using MediatR;

namespace Nesi.Application.Commands.Quote;

public record RejectQuoteCommand(int QuoteId, int RejectedBy, string Reason) : IRequest<Unit>;
