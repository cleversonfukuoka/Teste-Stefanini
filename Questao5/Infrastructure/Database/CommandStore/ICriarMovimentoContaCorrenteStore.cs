using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.CommandStore
{
    public interface ICriarMovimentoContaCorrenteStore
    {
        Task<bool> IsMovimentoExistsAsync(Guid idRequisicao);
        Task<ContaCorrente> ObterContaCorrenteAsync(string idContaCorrente);
        Task<bool> PersistirMovimentoAsync(string idMovimento, string idContaCorrente, string dataMovimento, char tipoMovimento, decimal valor);
    }
}
