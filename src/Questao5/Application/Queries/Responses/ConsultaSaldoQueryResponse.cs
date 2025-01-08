namespace Questao5.Application.Queries.Responses;

public sealed class ConsultaSaldoQueryResponse
{
    public long ContaCorrente { get; set; }

    public string Titular { get; set; }

    public DateTime DataHoraReposta { get; set; } = DateTime.UtcNow;

    public decimal Saldo { get; set; } = decimal.Zero;
}