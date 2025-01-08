using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Questao2.Domain.Interfaces;
using Questao2.Integration.Interfaces;
using Questao2.Integration.Services;
using Refit;


public class Program
{
    static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        var app = ActivatorUtilities.CreateInstance<ConsoleApp>(host.Services);
        await app.RunAsync();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
            })
            .ConfigureServices((context, services) =>
            {
                services.AddRefitClient<IPartidasApi>()
                    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://jsonmock.hackerrank.com"));
                services.AddScoped<IPartidasService, PartidasService>();
            });
}

public class ConsoleApp
{
    private readonly IPartidasService _partidaService;

    public ConsoleApp(IPartidasService partidaService)
    {
        _partidaService = partidaService;
    }

    public async Task RunAsync()
    {
        var firstTeamName = "Paris Saint-Germain";
        var firstTeamYear = 2013;

        var secondTeamName = "Chelsea";
        var secondTeamYear = 2014;

        var tasks = new Dictionary<string, Task<int>>
        {
            ["PSG"] = GetTotalScoredGoals("Paris Saint-Germain", 2013),
            ["Chelsea"] = GetTotalScoredGoals("Chelsea", 2014)
        };

        await Task.WhenAll(tasks.Values);

        Console.WriteLine($"Team Paris Saint-Germain scored {tasks["PSG"].Result} goals in 2013");
        Console.WriteLine($"Team Chelsea scored {tasks["Chelsea"].Result} goals in 2014");

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }

    public async Task<int> GetTotalScoredGoals(string team, int year) =>
        await _partidaService.ObterTotalGolsPorTimeEAno(team, year);
}