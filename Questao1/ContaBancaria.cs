using System.Globalization;

namespace Questao1
{
    public class ContaBancaria {
    public int _numeroConta { get; private set; }
    public string _titular { get; set; }
    public double _saldo { get; private set; }

    private const double taxaSaque = 3.50;

    // Construtor que permite definir o saldo inicial opcional
    public ContaBancaria(int numeroConta, string titular, double depositoInicial = 0)
    {
        _numeroConta = numeroConta;
        _titular = titular;
        Deposito(depositoInicial);
    }

    // Método para realizar um depósito
    public void Deposito(double valor)
    {
        if (valor > 0)
        {
            _saldo += valor;
        }
    }

    // Método para realizar um saque com a taxa incluída
    public void Saque(double valor)
    {
        _saldo -= (valor + taxaSaque);
    }

    // Método para exibir os dados da conta
    public override string ToString()
    {
        return $"Conta {_numeroConta}, Titular: {_titular}, Saldo: $ {_saldo:F2}";
    }
       
    }
}
