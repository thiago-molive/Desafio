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
            (var team1Response, totalGoals) = await ObterGolsComoTeam1(team, year, currentPage, totalGoals);

            (totalGoals, hasMorePages) = await ObterGolsComoTeam2(team, year, currentPage, team1Response, totalGoals, hasMorePages);

            currentPage++;
        }

        return totalGoals;
    }

    private async Task<(int totalGoals, bool hasMorePages)> ObterGolsComoTeam2(string team, int year, int currentPage, ApiResponse? team1Response, int totalGoals, bool hasMorePages)
    {
        var team2Response = await _api.ObterPartidas(year, null, team, currentPage);
        if (team2Response == null)
            return (totalGoals, hasMorePages);

        foreach (var match in team2Response.Data)
        {
            if (!int.TryParse(match.Team2Goals, out var gols))
                continue;

            totalGoals += gols;
        }

        hasMorePages = currentPage < team1Response?.Total_Pages || currentPage < team2Response.Total_Pages;

        return (totalGoals, hasMorePages);
    }

    private async Task<(ApiResponse? team1Response, int totalGoals)> ObterGolsComoTeam1(string team, int year, int currentPage, int totalGoals)
    {
        var team1Response = await _api.ObterPartidas(year, team, null, currentPage);
        if (team1Response == null)
            return (team1Response, totalGoals);

        foreach (var match in team1Response.Data)
        {
            if (!int.TryParse(match.Team1Goals, out var gols))
                continue;

            totalGoals += gols;
        }

        return (team1Response, totalGoals);
    }
}

