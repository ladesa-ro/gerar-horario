namespace Core;

public interface IGerarHorarioOptions
{
    public int DiaSemanaInicio { get; }
    public int DiaSemanaFim { get; }

    public ITurma[] Turmas { get; }
    public IProfessor[] Professores { get; }
    public IIntervalo[] IntervalosDeAula { get; }
}
