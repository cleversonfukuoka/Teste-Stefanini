using MediatR;
using Microsoft.Data.Sqlite;
using Questao5.Infrastructure.Sqlite;
using Questao5.Application.Handlers.Implementations;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Infrastructure.Database.CommandStore.Responses;
using System.Data;
using System.Reflection;
using Questao5.Infrastructure.Database.CommandStore;
using Questao5.Infrastructure.Database.QueryStore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure MediatR
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

// Register SQLite connection
builder.Services.AddSingleton<IDbConnection>(provider =>
{
    var connectionString = builder.Configuration.GetValue<string>("DatabaseName", "Data Source=database.sqlite");
    return new SqliteConnection(connectionString);
});

// Register DatabaseConfig
builder.Services.AddSingleton(new DatabaseConfig
{
    Name = builder.Configuration.GetValue<string>("DatabaseName", "Data Source=database.sqlite")
});

// Register the store response service
builder.Services.AddTransient<ICriarMovimentoContaCorrenteStore, CriarMovimentoContaCorrenteStoreResponse>();
builder.Services.AddTransient<IObterSaldoContaCorrenteStoreResponse, ObterSaldoContaCorrenteStoreResponse>();


// Register the database bootstrap service
builder.Services.AddSingleton<IDatabaseBootstrap, DatabaseBootstrap>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Initialize database
#pragma warning disable CS8602 // Dereference of a possibly null reference.
app.Services.GetService<IDatabaseBootstrap>()?.Setup();
#pragma warning restore CS8602 // Dereference of a possibly null reference.

app.Run();
