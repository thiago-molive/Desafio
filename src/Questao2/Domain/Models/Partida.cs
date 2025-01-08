namespace Questao2.Domain.Models;

public class Partida
{
    public string Competition { get; set; }
    public int Year { get; set; }
    public string Round { get; set; }
    public string Team1 { get; set; }
    public string Team2 { get; set; }
    public string Team1Goals { get; set; }
    public string Team2Goals { get; set; }
}

public class ApiResponse
{
    public int Page { get; set; }
    public int Per_Page { get; set; }
    public int Total { get; set; }
    public int Total_Pages { get; set; }
    public List<Partida> Data { get; set; }
}