namespace Questao5.Application.Queries.Responses
{
    public class ObterSaldoContaCorrenteResponse : BaseResponse
    {        
        public string NumeroConta { get; set; }
        public string NomeTitular { get; set; }
        public decimal Saldo { get; set; }
        public bool Ativa { get; set; }        
        public DateTime DataHoraConsulta { get; set; }


    }
}
