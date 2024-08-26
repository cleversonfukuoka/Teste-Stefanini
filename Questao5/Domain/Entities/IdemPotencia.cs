namespace Questao5.Domain.Entities
{
    public class IdemPotencia
    {
        public Guid ChaveIdempotencia { get; set; }
        public string Requisicao { get; set; }
        public string Resultado { get; set; }

        public IdemPotencia(Guid chaveIdempotencia, string requisicao, string resultado)
        {
            ChaveIdempotencia = chaveIdempotencia;
            Requisicao = requisicao;
            Resultado = resultado;
        }

        public void AtualizarResultado(string resultado)
        {
            Resultado = resultado;
        }
    }
}
