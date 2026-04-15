using MediatR;

namespace Nesi.Application.Commands.Quote;

public record ApproveQuoteCommand(int QuoteId, int ApprovedBy) : IRequest<Unit>;
