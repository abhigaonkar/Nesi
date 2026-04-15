using MediatR;
using Nesi.Application.DTOs.Quote;

namespace Nesi.Application.Queries.Quote;

public record GetQuotesQuery : IRequest<IEnumerable<QuoteDto>>;
