using System;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using Xunit;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Infrastructure.Database.CommandStore;
using Questao5.Infrastructure.Database.CommandStore.Responses;
using Questao5.Domain.Enumerators;
using Questao5.Application.Handlers.Implementations;
using Questao5.Application;
using Questao5.Domain.Entities;

public class CriarMovimentoContaCorrenteHandlerTests
{
    private readonly ICriarMovimentoContaCorrenteStore _criarMovimentoContaCorrenteStore;
    private readonly CriarMovimentoContaCorrenteHandler _handler;

    public CriarMovimentoContaCorrenteHandlerTests()
    {
        _criarMovimentoContaCorrenteStore = Substitute.For<ICriarMovimentoContaCorrenteStore>();
        _handler = new CriarMovimentoContaCorrenteHandler(_criarMovimentoContaCorrenteStore);
    }

    [Fact]
    public async Task Handle_ReturnsValidResponse_WhenMovimentoExists()
    {
        // Arrange
        var command = new CriarMovimentoContaCorrenteCommand
        {
            IdemPotenciaKey = Guid.NewGuid(),
            IdContaCorrente = Guid.NewGuid(),
            Valor = 100m,
            TipoMovimento = 'D'
        };

        _criarMovimentoContaCorrenteStore.IsMovimentoExistsAsync(command.IdemPotenciaKey).Returns(true);

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(response.IsValid);
        Assert.Equal("Requisição já processada.", response.Message);
    }

    [Fact]
    public async Task Handle_ReturnsInvalidAccountResponse_WhenContaDoesNotExist()
    {
        // Arrange
        var command = new CriarMovimentoContaCorrenteCommand
        {
            IdemPotenciaKey = Guid.NewGuid(),
            IdContaCorrente = Guid.NewGuid(),
            Valor = 100m,
            TipoMovimento = 'D'
        };

        _criarMovimentoContaCorrenteStore.IsMovimentoExistsAsync(command.IdemPotenciaKey).Returns(false);
        _criarMovimentoContaCorrenteStore.ObterContaCorrenteAsync(command.IdContaCorrente.ToString().ToUpper()).Returns((ContaCorrente)null);

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(response.IsValid);
        Assert.Equal("Conta corrente não cadastrada.", response.Message);
        Assert.Equal(ErrorType.INVALID_ACCOUNT, response.ErrorType);
    }

    [Fact]
    public async Task Handle_ReturnsInactiveAccountResponse_WhenContaIsInactive()
    {
        // Arrange
        var command = new CriarMovimentoContaCorrenteCommand
        {
            IdemPotenciaKey = Guid.NewGuid(),
            IdContaCorrente = Guid.NewGuid(),
            Valor = 100m,
            TipoMovimento = 'D'
        };

        var conta = new ContaCorrente
        {
            IdContaCorrente = command.IdContaCorrente.ToString().ToUpper(),
            Ativo = Status.Inativo
        };

        _criarMovimentoContaCorrenteStore.IsMovimentoExistsAsync(command.IdemPotenciaKey).Returns(false);
        _criarMovimentoContaCorrenteStore.ObterContaCorrenteAsync(command.IdContaCorrente.ToString().ToUpper()).Returns(conta);

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(response.IsValid);
        Assert.Equal("Conta corrente inativa.", response.Message);
        Assert.Equal(ErrorType.INACTIVE_ACCOUNT, response.ErrorType);
    }

    [Fact]
    public async Task Handle_ReturnsInvalidValueResponse_WhenValorIsInvalid()
    {
        // Arrange
        var command = new CriarMovimentoContaCorrenteCommand
        {
            IdemPotenciaKey = Guid.NewGuid(),
            IdContaCorrente = Guid.NewGuid(),
            Valor = -100m, // Valor inválido
            TipoMovimento = 'D'
        };

        var conta = new ContaCorrente
        {
            IdContaCorrente = command.IdContaCorrente.ToString().ToUpper(),
            Ativo = Status.Ativo
        };

        _criarMovimentoContaCorrenteStore.IsMovimentoExistsAsync(command.IdemPotenciaKey).Returns(false);
        _criarMovimentoContaCorrenteStore.ObterContaCorrenteAsync(command.IdContaCorrente.ToString().ToUpper()).Returns(conta);

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(response.IsValid);
        Assert.Equal("Valor inválido.", response.Message);
        Assert.Equal(ErrorType.INVALID_VALUE, response.ErrorType);
    }

    [Fact]
    public async Task Handle_ReturnsInvalidTypeResponse_WhenTipoMovimentoIsInvalid()
    {
        // Arrange
        var command = new CriarMovimentoContaCorrenteCommand
        {
            IdemPotenciaKey = Guid.NewGuid(),
            IdContaCorrente = Guid.NewGuid(),
            Valor = 100m,
            TipoMovimento = 'X' // Tipo inválido
        };

        var conta = new ContaCorrente
        {
            IdContaCorrente = command.IdContaCorrente.ToString().ToUpper(),
            Ativo = Status.Ativo
        };

        _criarMovimentoContaCorrenteStore.IsMovimentoExistsAsync(command.IdemPotenciaKey).Returns(false);
        _criarMovimentoContaCorrenteStore.ObterContaCorrenteAsync(command.IdContaCorrente.ToString().ToUpper()).Returns(conta);

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(response.IsValid);
        Assert.Equal("Tipo de movimento inválido.", response.Message);
        Assert.Equal(ErrorType.INVALID_TYPE, response.ErrorType);
    }

    [Fact]
    public async Task Handle_ReturnsSuccessResponse_WhenMovimentoIsPersisted()
    {
        // Arrange
        var command = new CriarMovimentoContaCorrenteCommand
        {
            IdemPotenciaKey = Guid.NewGuid(),
            IdContaCorrente = Guid.NewGuid(),
            Valor = 100m,
            TipoMovimento = 'D'
        };

        var conta = new ContaCorrente
        {
            IdContaCorrente = command.IdContaCorrente.ToString().ToUpper(),
            Ativo = Status.Ativo
        };

        _criarMovimentoContaCorrenteStore.IsMovimentoExistsAsync(command.IdemPotenciaKey).Returns(false);
        _criarMovimentoContaCorrenteStore.ObterContaCorrenteAsync(command.IdContaCorrente.ToString().ToUpper()).Returns(conta);
        _criarMovimentoContaCorrenteStore.PersistirMovimentoAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<char>(), Arg.Any<decimal>()).Returns(true);

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(response.IsValid);
        Assert.Equal("Movimento realizado com sucesso.", response.Message);
        Assert.NotNull(response.IdMovimento);
    }

    [Fact]
    public async Task Handle_ReturnsPersistenceFailureResponse_WhenPersistFails()
    {
        // Arrange
        var command = new CriarMovimentoContaCorrenteCommand
        {
            IdemPotenciaKey = Guid.NewGuid(),
            IdContaCorrente = Guid.NewGuid(),
            Valor = 100m,
            TipoMovimento = 'D'
        };

        var conta = new ContaCorrente
        {
            IdContaCorrente = command.IdContaCorrente.ToString().ToUpper(),
            Ativo = Status.Ativo
        };

        _criarMovimentoContaCorrenteStore.IsMovimentoExistsAsync(command.IdemPotenciaKey).Returns(false);
        _criarMovimentoContaCorrenteStore.ObterContaCorrenteAsync(command.IdContaCorrente.ToString().ToUpper()).Returns(conta);
        _criarMovimentoContaCorrenteStore.PersistirMovimentoAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<char>(), Arg.Any<decimal>()).Returns(false);

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(response.IsValid);
        Assert.Equal("Falha ao persistir o movimento.", response.Message);
        Assert.Equal(ErrorType.PERSISTENCE_FAILURE, response.ErrorType);
    }
}
