using Dapper;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Infrastructure.Database.CommandStore;
using System.Data;

namespace Questao5.Infrastructure.Database.CommandStore.Responses
{
    public class CriarMovimentoContaCorrenteStoreResponse : ICriarMovimentoContaCorrenteStore
    {
        private readonly IDbConnection _dbConnection;

        public CriarMovimentoContaCorrenteStoreResponse() { }

        public CriarMovimentoContaCorrenteStoreResponse(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<bool> IsMovimentoExistsAsync(Guid idRequisicao)
        {
            var query = "SELECT COUNT(1) FROM movimento WHERE idmovimento = @IdRequisicao";
            var count = await _dbConnection.ExecuteScalarAsync<int>(query, new { IdRequisicao = idRequisicao });
            return count > 0;
        }

        public async Task<ContaCorrente> ObterContaCorrenteAsync(string idContaCorrente)
        {            
            var query = "SELECT idcontacorrente AS IdContaCorrente, numero AS Numero, nome AS Nome, ativo AS Ativo FROM contacorrente WHERE idcontacorrente = @IdContaCorrente";
            
            var conta = await _dbConnection.QueryFirstOrDefaultAsync<ContaCorrente>(
                query, new { IdContaCorrente = idContaCorrente }
            );

            if (conta == null)
            {
                return null;
            }

            // Converte o idcontacorrente para Guid
            var idContaCorrenteGuid = Guid.Parse(idContaCorrente);
            //var idContaCorrenteString = conta.IdContaCorrente.ToString();
            

            return new ContaCorrente
            {
                IdContaCorrente = conta.IdContaCorrente,
                Numero = (int)conta.Numero,
                Nome = (string)conta.Nome,
                Ativo = (Status)conta.Ativo
            };            
        }

        public async Task<bool> PersistirMovimentoAsync(string idMovimento, string idContaCorrente, string dataMovimento, char tipoMovimento, decimal valor)
        {
            var query = @"
                INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor)
                VALUES (@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor)";

            var parameters = new
            {
                IdMovimento = idMovimento,
                IdContaCorrente = idContaCorrente,
                DataMovimento = dataMovimento,
                TipoMovimento = tipoMovimento,
                Valor = valor
            };

            try
            {
                var rowsAffected = await _dbConnection.ExecuteAsync(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception)
            {
                // Log exception here or handle it as per your requirements
                return false;
            }
        }
    }
}
