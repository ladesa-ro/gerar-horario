namespace Core;

public interface IGerarHorarioOptions
{
    public DiaSemanaIso DiaInicio { get; set; }
    public DiaSemanaIso DiaFim { get; set; }
}
