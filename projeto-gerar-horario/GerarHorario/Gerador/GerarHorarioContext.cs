using Google.OrTools.Sat;
using Sisgea.GerarHorario.Core.Dtos.Configuracoes;

namespace Sisgea.GerarHorario.Core;

public class GerarHorarioContext
{
    public GerarHorarioOptions Options { get; init; }
    public CpModel Model { get; init; }

    public List<PropostaDeAula> TodasAsPropostasDeAula { get; init; }


    public GerarHorarioContext(GerarHorarioOptions options, CpModel? model = null, List<PropostaDeAula>? todasAsPropostasDeAula = null, bool iniciarTodasAsPropostasDeAula = true)
    {
        Options = options;
        Model = model ?? new CpModel();
        TodasAsPropostasDeAula = todasAsPropostasDeAula ?? [];

        if (iniciarTodasAsPropostasDeAula)
        {
            this.IniciarTodasAsPropostasDeAula();
        }
    }

    public void IniciarTodasAsPropostasDeAula()
    {
        this.TodasAsPropostasDeAula.Clear();

        IEnumerable<(int, int, string, string)> GerarTodasAsCombinacoes()
        {
            for (int diaSemanaIso = this.Options.DiaSemanaInicio; diaSemanaIso <= this.Options.DiaSemanaFim; diaSemanaIso++)
            {
                for (int intervaloIndex = 0; intervaloIndex < this.Options.HorariosDeAula.Length; intervaloIndex++)
                {
                    foreach (var turma in this.Options.Turmas)
                    {
                        foreach (var diario in turma.DiariosDaTurma)
                        {
                            yield return (diaSemanaIso, intervaloIndex, turma.Id, diario.Id);
                        }
                    }
                }
            }
        }

        IEnumerable<(int, int, string, string)> GerarCombinacoesComDisponibilidade()
        {
            foreach (var combinacao in GerarTodasAsCombinacoes())
            {
                var (diaSemanaIso, intervaloIndex, turmaId, diarioId) = combinacao;

                var intervaloAula = this.Options.HorariosDeAula[intervaloIndex];

                if (intervaloAula == null)
                {
                    throw new Exception($"Intervalo não encontrado: {intervaloIndex}.");
                }

                var turma = this.Options.Turmas.ToList().Find(turma => turma.Id == turmaId);

                if (turma == null)
                {
                    throw new Exception($"Turma não encontrada: {turmaId}.");
                }

                var diario = turma.DiariosDaTurma.ToList().Find(diario => diario.Id == diarioId);

                if (diario == null)
                {
                    throw new Exception($"Diário não encontrado: {diarioId}.");
                }

                var professor = this.Options.Professores.ToList().Find(professor => professor.Id == diario.ProfessorId);

                if (professor == null)
                {
                    throw new Exception($"Professor não encontrado: {diario.ProfessorId} (diário: {diario.Id}, turma: {turma.Id}).");
                }

                var disponivelParaTurma = Restricoes.VerificarIntervaloEmDisponibilidades(turma.Disponibilidades, diaSemanaIso, intervaloAula);
                var disponivelParaProfessor = Restricoes.VerificarIntervaloEmDisponibilidades(professor.Disponibilidades, diaSemanaIso, intervaloAula);

                var disponivel = disponivelParaTurma && disponivelParaProfessor;

                if (disponivel)
                {
                    yield return combinacao;
                }
            }
        }

        foreach (var combinacao in GerarCombinacoesComDisponibilidade())
        {
            var (diaSemanaIso, intervaloIndex, turmaId, diarioId) = combinacao;

            var propostaDeAula = new PropostaDeAula(this, turmaId, diarioId, diaSemanaIso, intervaloIndex);
            this.TodasAsPropostasDeAula.Add(propostaDeAula);

            if (this.Options.LogDebug)
            {
                Console.WriteLine($"--> init proposta de aula | dia: {diaSemanaIso} | intervalo: {intervaloIndex} | diario: {diarioId}");
            }
        }

        Console.WriteLine($"--> Quantidade de propostas: {this.TodasAsPropostasDeAula.Count}");
    }
}