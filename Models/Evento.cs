namespace MauiAppEventos.Models;

public class Evento
{
    public string Nome { get; set; } = string.Empty;
    public DateTime DataInicio { get; set; }
    public DateTime DataTermino { get; set; }
    public int NumeroParticipantes { get; set; }
    public string Local { get; set; } = string.Empty;
    public double CustoPorParticipante { get; set; }

    public TimeSpan Duracao { get => DataTermino - DataInicio; }
    public int DuracaoDias { get => Duracao.Days; }
    public double CustoTotal { get => NumeroParticipantes * CustoPorParticipante; }
}
