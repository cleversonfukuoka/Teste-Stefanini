using MediatR;
using Questao5.Application.Commands.Responses;
using System.Text.Json.Serialization;

namespace Questao5.Application.Commands.Requests
{
    public class CriarMovimentoContaCorrenteCommand : IRequest<CriarMovimentoContaCorrenteResponse>
    {
        public Guid IdContaCorrente { get; set; }
        public decimal Valor { get; set; }
        public char TipoMovimento { get; set; } //'C' para credito e 'D' para debito
        public Guid IdemPotenciaKey { get; set; }
        [JsonIgnore]
        public bool Ativa { get; set; }

        public CriarMovimentoContaCorrenteCommand() { }
        
    }
}
