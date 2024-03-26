namespace Sisgea.GerarHorario.Core.Dtos.Entidades;

public record Intervalo
{
    public string HorarioInicio { get; init; }
    public string HorarioFim { get; init; }

    public Intervalo(string horarioInicio, string horarioFim)
    {
        HorarioInicio = horarioInicio;
        HorarioFim = horarioFim;
    }
}
