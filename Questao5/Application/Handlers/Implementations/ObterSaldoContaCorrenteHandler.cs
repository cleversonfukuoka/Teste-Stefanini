using MediatR;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Infrastructure.Database.CommandStore.Responses;
using Questao5.Infrastructure.Database.QueryStore;
using System.Data;

namespace Questao5.Application.Handlers.Implementations
{
    public class ObterSaldoContaCorrenteHandler : IRequestHandler<ObterSaldoContaCorrenteQuery, ObterSaldoContaCorrenteResponse>
    {
        private readonly IObterSaldoContaCorrenteStoreResponse _obterSaldoContaCorrenteStoreResponse;

        public ObterSaldoContaCorrenteHandler(IObterSaldoContaCorrenteStoreResponse obterSaldoContaCorrenteStoreResponse)
        {
            _obterSaldoContaCorrenteStoreResponse = obterSaldoContaCorrenteStoreResponse;
        }

        public async Task<ObterSaldoContaCorrenteResponse> Handle(ObterSaldoContaCorrenteQuery request, CancellationToken cancellationToken)
        {
            var response = new ObterSaldoContaCorrenteResponse();

            // Validar se a conta existe e está ativa
            var conta = await _obterSaldoContaCorrenteStoreResponse.ObterContaCorrenteAsync(request.IdContaCorrente);
            if (conta == null)
            {
                response.IsValid = false;
                response.Message = "Conta corrente não cadastrada.";                
                response.ErrorType = ErrorType.INVALID_ACCOUNT;
                return response;
            }

            if (conta.Ativo == 0)
            {
                response.IsValid = false;
                response.Message = "Conta corrente inativa.";
                response.ErrorType = ErrorType.INACTIVE_ACCOUNT;
                return response;
            }

            // Calcular saldo
            var totalCreditos = await _obterSaldoContaCorrenteStoreResponse.ObterTotalCreditosAsync(request.IdContaCorrente);
            var totalDebitos = await _obterSaldoContaCorrenteStoreResponse.ObterTotalDebitosAsync(request.IdContaCorrente);

            response.Saldo = totalCreditos - totalDebitos;
            response.NumeroConta = conta.numero.ToString();
            response.NomeTitular = conta.nome;
            response.DataHoraConsulta = DateTime.Now;
            response.IsValid = true;
            response.Message = "Saldo consultado com sucesso.";

            return response;
        }

    }
}
