namespace Sisgea.GerarHorario.Core.Dtos.Entidades;

public record DisponibilidadeDia
{
    public int DiaSemanaIso { get; init; }
    public Intervalo Intervalo { get; init; }

    public DisponibilidadeDia(int diaSemanaIso, Intervalo intervalo)
    {
        DiaSemanaIso = diaSemanaIso;
        Intervalo = intervalo;
    }
}
