using Google.OrTools.Sat;
using Sisgea.GerarHorario.Core.Dtos.HorarioGerado;

namespace Sisgea.GerarHorario.Core;

public class GeradorSolutionCallback : CpSolverSolutionCallback
{
  public Action<HorarioGerado> Action { get; }
  public GerarHorarioContext Contexto { get; init; }


  public GeradorSolutionCallback(GerarHorarioContext contexto, Action<HorarioGerado> action)
  {
    this.Contexto = contexto;
    this.Action = action;
  }

  public override void OnSolutionCallback()
  {
    var propostasAtivas = from propostaAula in this.Contexto.TodasAsPropostasDeAula
                          where
                            BooleanValue(propostaAula.ModelBoolVar)
                          select new HorarioGeradoAula(propostaAula.TurmaId, propostaAula.DiarioId, propostaAula.IntervaloIndex, propostaAula.DiaSemanaIso);

    var horarioGerado = new HorarioGerado
    {
      Aulas = propostasAtivas.ToArray()
    };

    this.Action(horarioGerado);
  }
}

