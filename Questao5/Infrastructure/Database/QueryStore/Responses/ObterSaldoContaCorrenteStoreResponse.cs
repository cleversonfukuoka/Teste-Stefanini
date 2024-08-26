using Dapper;
using Questao5.Infrastructure.Database.QueryStore;
using System.Data;

namespace Questao5.Infrastructure.Database.CommandStore.Responses
{
    public class ObterSaldoContaCorrenteStoreResponse : IObterSaldoContaCorrenteStoreResponse
    {
        private readonly IDbConnection _dbConnection;

        public ObterSaldoContaCorrenteStoreResponse(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<dynamic> ObterContaCorrenteAsync(Guid idContaCorrente)
        {
            var sql = "SELECT * FROM contacorrente WHERE idcontacorrente = @IdContaCorrente";
            return await _dbConnection.QueryFirstOrDefaultAsync<dynamic>(sql, new { IdContaCorrente = idContaCorrente });
        }

        public async Task<decimal> ObterTotalCreditosAsync(Guid idContaCorrente)
        {
            var sql = "SELECT IFNULL(SUM(valor), 0) FROM movimento WHERE idcontacorrente = @IdContaCorrente AND tipomovimento = 'C'";
            return await _dbConnection.QueryFirstOrDefaultAsync<decimal>(sql, new { IdContaCorrente = idContaCorrente });
        }

        public async Task<decimal> ObterTotalDebitosAsync(Guid idContaCorrente)
        {
            var sql = "SELECT IFNULL(SUM(valor), 0) FROM movimento WHERE idcontacorrente = @IdContaCorrente AND tipomovimento = 'D'";
            return await _dbConnection.QueryFirstOrDefaultAsync<decimal>(sql, new { IdContaCorrente = idContaCorrente });
        }        
    }
}
