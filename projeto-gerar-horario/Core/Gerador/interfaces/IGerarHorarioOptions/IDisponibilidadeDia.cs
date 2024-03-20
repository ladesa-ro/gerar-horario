namespace Core;

public interface IDisponibilidadeDia
{
  public int DiaSemanaIso { get; }
  public IIntervalo Intervalo { get; }
}