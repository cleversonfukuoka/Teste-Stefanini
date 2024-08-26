using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Queries.Requests;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContaCorrenteController : Controller
    {

        private readonly IMediator _mediator;

        public ContaCorrenteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Movimentar")]
        public async Task<IActionResult> Movimentar([FromBody] CriarMovimentoContaCorrenteCommand command)
        {
            var response = await _mediator.Send(command);
            if (!response.IsValid)
            {
                return BadRequest(new { response.Message });
            }

            return Ok(new { response.IdMovimento });
        }

        [HttpGet("Saldo")]
        public async Task<IActionResult> ConsultarSaldo([FromQuery] ObterSaldoContaCorrenteQuery query)
        {
            var response = await _mediator.Send(query);
            if (response == null || !response.IsValid)
            {
                return BadRequest(new { Message = response?.Message ?? "Erro desconhecido ao consultar o saldo." });
            }

            return Ok(new
            {
                response.NumeroConta,
                response.NomeTitular,
                response.DataHoraConsulta,
                response.Saldo
            });
        }
    }
}
