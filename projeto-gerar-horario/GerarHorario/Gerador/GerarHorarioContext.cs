


using Google.OrTools.Sat;
using Sisgea.GerarHorario.Core.Dtos.Configuracoes;

namespace Sisgea.GerarHorario.Core;

public class GerarHorarioContext
{
    public GerarHorarioOptions Options { get; init; }
    public CpModel Model { get; init; }

    public List<PropostaDeAula> TodasAsPropostasDeAula { get; init; }


    public GerarHorarioContext(GerarHorarioOptions options, CpModel? model = null, List<PropostaDeAula>? todasAsPropostasDeAula = null)
    {
        Options = options;
        Model = model ?? new CpModel();
        TodasAsPropostasDeAula = todasAsPropostasDeAula ?? [];
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
                var disponivel = true;

                // TODO: filtrar de acordo com a disponibilidade da turma e do professor.
                if (disponivel)
                {
                    yield return combinacao;
                }
            }
        }

        foreach (var combinacao in GerarCombinacoesComDisponibilidade())
        {
            var (diaSemanaIso, intervaloIndex, turmaId, diarioId) = combinacao;

            var propostaLabel = $"dia_{diaSemanaIso}::intervalo_{intervaloIndex}::diario_{diarioId}";

            var modelBoolVar = this.Model.NewBoolVar(propostaLabel);

            var propostaDeAula = new PropostaDeAula(turmaId, diarioId, diaSemanaIso, intervaloIndex, modelBoolVar);

            this.TodasAsPropostasDeAula.Add(propostaDeAula);

            if (this.Options.LogDebug)
            {
                Console.WriteLine($"--> init proposta de aula | {propostaLabel}");
            }
        }

        Console.WriteLine($"--> Quantidade de propostas: {this.TodasAsPropostasDeAula.Count}");
    }
}