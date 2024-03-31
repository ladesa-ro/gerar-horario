using Sisgea.GerarHorario.Core.Dtos.Entidades;

namespace Sisgea.GerarHorario.Core.Dtos.Configuracoes;

public class GerarHorarioOptions
{

    public int DiaSemanaInicio { get; set; }
    public int DiaSemanaFim { get; set; }

    public Turma[] Turmas { get; set; }
    public Professor[] Professores { get; set; }
    public Intervalo[] HorariosDeAula { get; set; }

    public bool LogDebug { get; set; } = false;

    public GerarHorarioOptions(int diaSemanaInicio, int diaSemanaFim, Turma[] turmas, Professor[] professores, Intervalo[] horariosDeAula, bool logDebug = false)
    {
        DiaSemanaInicio = diaSemanaInicio;
        DiaSemanaFim = diaSemanaFim;
        Turmas = turmas;
        Professores = professores;
        HorariosDeAula = horariosDeAula;
        LogDebug = logDebug;
    }

    public Professor? ProfessorFindById(string professorId)
    {
        var professor = this.Professores.ToList().Find(professor => professor.Id == professorId);
        return professor;
    }

    public Professor ProfessorFindByIdStrict(string professorId, string? exceptionContext = null)
    {
        var professor = this.ProfessorFindById(professorId);

        if (professor == null)
        {
            throw new Exception($"Professor não encontrado: {professorId}{exceptionContext}.");
        }

        return professor;
    }

    public IEnumerable<Diario> Diarios
    {
        get
        {
            foreach (var turma in this.Turmas)
            {
                foreach (var diario in turma.DiariosDaTurma)
                {
                    yield return diario;
                }
            }
        }
    }

    public Diario? DiarioFindById(string diarioId)
    {
        var diario = this.Diarios.ToList().Find(diario => diario.Id == diarioId);
        return diario;
    }

    public Diario DiarioFindByIdStrict(string diarioId, string? exceptionContext = null)
    {
        var diario = this.DiarioFindById(diarioId);

        if (diario == null)
        {
            throw new Exception($"Diário não encontrado: {diarioId}{exceptionContext}.");
        }

        return diario;
    }

    public Turma TurmaFindByIdStrict(string turmaId, string? exceptionContext = null)
    {
        var turma = this.TurmaFindById(turmaId);

        if (turma == null)
        {
            throw new Exception($"Diário não encontrado: {turmaId}{exceptionContext}.");
        }

        return turma;
    }

    public Turma? TurmaFindById(string turmaId)
    {
        var turma = this.Turmas.ToList().Find(turma => turma.Id == turmaId);
        return turma;
    }

    public Intervalo? HorarioDeAulaByIndex(int horarioDeAulaIndex)
    {
        var horarioDeAula = this.HorariosDeAula[horarioDeAulaIndex];
        return horarioDeAula;
    }
    public Intervalo HorarioDeAulaFindByIdStrict(int horarioDeAulaIndex, string? exceptionContext = null)
    {
        var horarioDeAula = this.HorarioDeAulaByIndex(horarioDeAulaIndex);

        if (horarioDeAula == null)
        {
            throw new Exception($"Horário de aula não encontrado: índice {horarioDeAulaIndex}.");
        }

        return horarioDeAula;
    }

    public IEnumerable<Diario> DiariosByTurmaId(string turmaId)
    {
        var turma = this.TurmaFindByIdStrict(turmaId);

        foreach (var diario in turma.DiariosDaTurma)
        {
            yield return diario;
        }
    }

    public bool ProfessorEstaVinculadoAoDiario(string professorId, string diarioId)
    {
        var diario = this.DiarioFindByIdStrict(diarioId);
        var professor = this.ProfessorFindByIdStrict(professorId);

        return diario.ProfessorId == professor.Id;
    }

    public override string ToString()
    {
        return "GerarHorarioOptions { nenhuma configuração }";
    }
}
