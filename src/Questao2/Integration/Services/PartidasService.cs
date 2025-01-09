using Questao2.Domain.Interfaces;
using Questao2.Domain.Models;
using Questao2.Integration.Interfaces;

namespace Questao2.Integration.Services;

public sealed class PartidasService : IPartidasService
{
    private readonly IPartidasApi _api;

    public PartidasService(IPartidasApi api)
    {
        _api = api;
    }

    public async Task<int> ObterTotalGolsPorTimeEAno(string team, int year)
    {
        int totalGoals = 0;
        int currentPage = 1;
        bool hasMorePages = true;

        while (hasMorePages)
        {
            var tasks = new Dictionary<string, Task<(ApiResponse? teamResponse, int totalGoals)>>
            {
                ["TEAM_1"] = ObterGolsComoTeam1(team, year, currentPage),
                ["TEAM_2"] = ObterGolsComoTeam2(team, year, currentPage)
            };

            await Task.WhenAll(tasks.Values);


            var (team1Response, team1Goals) = tasks["TEAM_1"].Result;

            var (team2Response, team2Goals) = tasks["TEAM_2"].Result;

            totalGoals += team1Goals + team2Goals;

            hasMorePages = currentPage < team1Response?.Total_Pages || currentPage < team2Response?.Total_Pages;

            currentPage++;
        }

        return totalGoals;
    }

    private async Task<(ApiResponse? teamResponse, int totalGoals)> ObterGolsComoTeam1(string team, int year, int currentPage)
    {
        var totalGoals = 0;
        var team1Response = await _api.ObterPartidas(year, team, null, currentPage);
        if (team1Response == null)
            return (team1Response, totalGoals);

        foreach (var match in team1Response.Data)
        {
            if (!int.TryParse(match.Team1Goals, out var goals))
                continue;

            totalGoals += goals;
        }

        return (team1Response, totalGoals);
    }

    private async Task<(ApiResponse? team2Response, int totalGoals)> ObterGolsComoTeam2(string team, int year, int currentPage)
    {
        var totalGoals = 0;
        var team2Response = await _api.ObterPartidas(year, null, team, currentPage);
        if (team2Response == null)
            return (team2Response, 0);

        foreach (var match in team2Response.Data)
        {
            if (!int.TryParse(match.Team2Goals, out var goals))
                continue;

            totalGoals += goals;
        }

        return (team2Response, totalGoals);
    }
}

