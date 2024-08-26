
using Microsoft.AspNetCore.Mvc;

namespace Questao5.Application.Commands.Responses
{
    public class CriarMovimentoContaCorrenteResponse : BaseResponse
    {        
        public Guid IdMovimento { get; set; }   
        public bool Ativa { get; set; }

    }
}
