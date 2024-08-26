using Questao5.Application.Queries.Responses;
using MediatR;

namespace Questao5.Application.Queries.Requests
{
    public class ObterSaldoContaCorrenteQuery : IRequest<ObterSaldoContaCorrenteResponse>
    {
        public Guid IdContaCorrente { get; set; }
    }
}
