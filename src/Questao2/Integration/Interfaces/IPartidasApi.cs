using Questao2.Domain.Models;
using Refit;

namespace Questao2.Integration.Interfaces;

public interface IPartidasApi
{
    [Get("/api/football_matches")]
    Task<ApiResponse?> ObterPartidas(int? year = null, string? team1 = null, string? team2 = null, int? page = null);
}