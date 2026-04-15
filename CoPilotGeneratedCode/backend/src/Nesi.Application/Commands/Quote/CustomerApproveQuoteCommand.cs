using MediatR;

namespace Nesi.Application.Commands.Quote;

public record CustomerApproveQuoteCommand(int QuoteId, string ApprovedBy) : IRequest<bool>;
