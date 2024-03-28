


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

    for (int diaSemanaIso = this.Options.DiaSemanaInicio; diaSemanaIso <= this.Options.DiaSemanaFim; diaSemanaIso++)
    {
      for (int intervaloIndex = 0; intervaloIndex < this.Options.HorariosDeAula.Length; intervaloIndex++)
      {
        foreach (var turma in this.Options.Turmas)
        {
          foreach (var diario in turma.DiariosDaTurma)
          {
            var propostaLabel = $"dia_{diaSemanaIso}::intervalo_{intervaloIndex}::diario_{diario.Id}";

            var modelBoolVar = this.Model.NewBoolVar(propostaLabel);

            var propostaDeAula = new PropostaDeAula(turma.Id, diario.Id, diaSemanaIso, intervaloIndex, modelBoolVar);

            this.TodasAsPropostasDeAula.Add(propostaDeAula);

            if (this.Options.LogDebug)
            {
              Console.WriteLine($"--> init proposta de aula | {propostaLabel}");
            }

          }
        }
      }
    }

    Console.WriteLine($"--> Quantidade de propostas: {this.TodasAsPropostasDeAula.Count}");
  }
}