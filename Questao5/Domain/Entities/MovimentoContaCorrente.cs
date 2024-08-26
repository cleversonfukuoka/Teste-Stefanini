using Questao5.Domain.Enumerators;

namespace Questao5.Domain.Entities
{
    public class MovimentoContaCorrente
    {
        public string IdMovimento { get; set; }
        public string IdContaCorrente { get; set; }
        public DateTime DataMovimento { get; set; }
        public TipoMovimento TipoMovimento { get; set; } // 'C' para crédito, 'D' para débito
        public decimal Valor { get; set; }

        public MovimentoContaCorrente() { }
        public MovimentoContaCorrente(string idMovimento, string idContaCorrente, decimal valor, TipoMovimento tipoMovimento)
        {
            IdMovimento = idMovimento;
            IdContaCorrente = idContaCorrente;
            DataMovimento = DateTime.UtcNow;
            TipoMovimento = tipoMovimento;
            Valor = valor;
        }

        public void AtualizarValor(decimal novoValor)
        {
            Valor = novoValor;
        }

        public void AtualizarTipoMovimento(TipoMovimento novoTipo)
        {
            if (novoTipo != TipoMovimento.Credito && novoTipo != TipoMovimento.Debito)
            {
                throw new ArgumentException("Tipo de movimento inválido. Use 'C' para crédito ou 'D' para débito.");
            }
            TipoMovimento = novoTipo;
        }
    }
}
