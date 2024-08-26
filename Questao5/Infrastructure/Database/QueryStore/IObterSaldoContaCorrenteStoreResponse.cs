namespace Questao5.Infrastructure.Database.QueryStore
{
    public interface IObterSaldoContaCorrenteStoreResponse
    {
        Task<dynamic> ObterContaCorrenteAsync(Guid idContaCorrente);
        Task<decimal> ObterTotalCreditosAsync(Guid idContaCorrente);
        Task<decimal> ObterTotalDebitosAsync(Guid idContaCorrente);
    }
}
