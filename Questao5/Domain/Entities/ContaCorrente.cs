using Questao5.Domain.Enumerators;
using System.ComponentModel.DataAnnotations.Schema;

namespace Questao5.Domain.Entities
{
    public class ContaCorrente
    {
        [Column("idcontacorrente")]
        public string IdContaCorrente { get; set; }
        [Column("numero")]
        public int Numero { get; set; }
        [Column("nome")]
        public string Nome { get; set; }
        [Column("ativo")]
        public Status Ativo { get; set; }

        public ContaCorrente() { }
        public ContaCorrente(string idContaCorrente, int numero, string nome, Status ativo)
        {
            IdContaCorrente = idContaCorrente;
            Numero = numero;
            Nome = nome;
            Ativo = ativo;
        }
    }
}
