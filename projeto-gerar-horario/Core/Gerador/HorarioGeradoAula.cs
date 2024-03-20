namespace Core;

public class HorarioGeradoAula : IHorarioGeradoAula
{
    public object Diario { get; set; } = new { };
    public object IntervaloDeTempo { get; set; } = new { };
    public int DiaDaSemanaIso { get; set; } = 0;
}
