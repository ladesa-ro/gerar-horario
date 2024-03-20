namespace Core;
public class GerarHorarioOptions : IGerarHorarioOptions
{

    public int DiaSemanaInicio { get; set; }
    public int DiaSemanaFim { get; set; }

    public ITurma[] Turmas { get; set; }
    public IProfessor[] Professores { get; set; }
    public IIntervalo[] IntervalosDeAula { get; set; }

    public override string ToString()
    {
        return "GerarHorarioOptions { nenhuma configuração }";
    }
}