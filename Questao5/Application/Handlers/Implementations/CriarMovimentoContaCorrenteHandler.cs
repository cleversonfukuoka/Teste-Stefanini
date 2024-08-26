using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Infrastructure.Database.CommandStore;
using Questao5.Infrastructure.Database.CommandStore.Responses;
using System.Net;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Questao5.Application.Handlers.Implementations
{
    public class CriarMovimentoContaCorrenteHandler : IRequestHandler<CriarMovimentoContaCorrenteCommand, CriarMovimentoContaCorrenteResponse>
    {
        private readonly ICriarMovimentoContaCorrenteStore _criarMovimentoContaCorrenteStoreResponse;        

        public CriarMovimentoContaCorrenteHandler(ICriarMovimentoContaCorrenteStore storeResponse)
        {
            _criarMovimentoContaCorrenteStoreResponse = storeResponse ?? throw new ArgumentNullException(nameof(storeResponse));        
        }        

        public async Task<CriarMovimentoContaCorrenteResponse> Handle(CriarMovimentoContaCorrenteCommand request, CancellationToken cancellationToken)
        {
            var response = new CriarMovimentoContaCorrenteResponse();

            // Verifica idempotência
            if (await _criarMovimentoContaCorrenteStoreResponse.IsMovimentoExistsAsync(request.IdemPotenciaKey))
            {
                response.IsValid = true;
                response.Message = "Requisição já processada.";
                return response;
            }
            
            // Validar se a conta existe e está ativa
            var conta = await _criarMovimentoContaCorrenteStoreResponse.ObterContaCorrenteAsync(request.IdContaCorrente.ToString().ToUpper());
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

            // Validar valor
            if (request.Valor <= 0)
            {
                response.IsValid = false;
                response.Message = "Valor inválido.";
                response.ErrorType = ErrorType.INVALID_VALUE;
                return response;
            }

            // Validar tipo de movimento
            if (request.TipoMovimento != 'C' && request.TipoMovimento != 'D')
            {
                response.IsValid = false;
                response.Message = "Tipo de movimento inválido.";
                response.ErrorType = ErrorType.INVALID_TYPE;
                return response;
            }

            // Persistir o movimento
            var idMovimento = Guid.NewGuid().ToString();
            var dataMovimento = DateTime.Now.ToString("dd/MM/yyyy");
            
            var sucesso = await _criarMovimentoContaCorrenteStoreResponse.PersistirMovimentoAsync(idMovimento.ToUpper(), request.IdContaCorrente.ToString().ToUpper(), dataMovimento, request.TipoMovimento, request.Valor);

            if (sucesso)
            {
                response.IsValid = true;
                response.Message = "Movimento realizado com sucesso.";
                response.IdMovimento = Guid.Parse(idMovimento.ToUpper());                
            }
            else
            {
                response.IsValid = false;
                response.Message = "Falha ao persistir o movimento.";
                response.ErrorType = ErrorType.PERSISTENCE_FAILURE;
            }

            return response;
        }
    }
}
