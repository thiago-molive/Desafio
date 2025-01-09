using Abstractions.Data;
using FluentAssertions.Common;
using FluentValidation;
using MediatR;
using Microsoft.OpenApi.Models;
using Questao5.Application.Events;
using Questao5.Application.Handlers.Queries.Interfaces;
using Questao5.Behaviors;
using Questao5.Domain.Interfaces;
using Questao5.Infrastructure.Database.CommandStore;
using Questao5.Infrastructure.Database.QueryStore;
using Questao5.Infrastructure.Providers.DapperProvider;
using Questao5.Infrastructure.Sqlite;
using Questao5.Middleware;
using Questao5.SwaggerExamples;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

// sqlite
builder.Services.AddSingleton(new DatabaseConfig { Name = builder.Configuration.GetValue<string>("DatabaseName", "Data Source=database.sqlite") });
builder.Services.AddSingleton<IDatabaseBootstrap, DatabaseBootstrap>();
builder.Services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();

// dependencies
builder.Services.AddScoped<IContaCorrenteQueryStore, ContaCorrenteQueryStore>();
builder.Services.AddScoped<IMovimentacaoCommandStore, MovimentacaoCommandStore>();
builder.Services.AddScoped<IIdempotencyCommandStore, IdempotencyCommandStore>();
builder.Services.AddScoped<IEventPublisher, EventPublisher>();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(IdempotencyBehavior<,>));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Minha API", Version = "v1" });

    // Inclui comentários XML no Swagger
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
    c.ExampleFilters();
});

builder.Services.AddSwaggerExamplesFromAssemblyOf<MovimentaContaCorrenteCommandExample>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<MovimentarContaCorrenteResponseExample>();

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly, includeInternalTypes: true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// sqlite
#pragma warning disable CS8602 // Dereference of a possibly null reference.
app.Services.GetService<IDatabaseBootstrap>().Setup();
#pragma warning restore CS8602 // Dereference of a possibly null reference.

app.Run();

// Informações úteis:
// Tipos do Sqlite - https://www.sqlite.org/datatype3.html


