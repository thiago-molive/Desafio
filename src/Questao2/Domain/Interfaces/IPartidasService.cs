namespace Questao2.Domain.Interfaces;

public interface IPartidasService
{
    Task<int> ObterTotalGolsPorTimeEAno(string team, int year);
}