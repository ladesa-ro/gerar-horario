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

        foreach (var combinacao in Restricoes.GerarCombinacoesComDisponibilidade(this.Options))
        {
            var intervalo = this.Options.HorarioDeAulaFindByIdStrict(combinacao.intervaloIndex);

            var propostaDeAula = new PropostaDeAula(
                contexto: this,
                turmaId: combinacao.turmaId,
                diarioId: combinacao.diarioId,
                professorId: combinacao.professorId,
                diaSemanaIso: combinacao.diaSemanaIso,
                intervaloIndex: combinacao.intervaloIndex,
                intervalo: intervalo
            );

            this.TodasAsPropostasDeAula.Add(propostaDeAula);

            if (this.Options.LogDebug)
            {
                Console.WriteLine($"--> init proposta de aula | dia: {combinacao.diaSemanaIso} | intervalo: {combinacao.intervaloIndex} | diario: {combinacao.diarioId}");
            }
        }

        Console.WriteLine($"--> Quantidade de propostas: {this.TodasAsPropostasDeAula.Count}");
    }
}