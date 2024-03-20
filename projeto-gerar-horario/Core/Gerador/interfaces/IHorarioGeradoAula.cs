namespace Core;

public interface IHorarioGeradoAula
{
    object Diario { get; set; }
    object IntervaloDeTempo { get; set; }
    int DiaDaSemanaIso { get; set; }
}
