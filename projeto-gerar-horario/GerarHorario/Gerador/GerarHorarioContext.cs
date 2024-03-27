


using Google.OrTools.Sat;
using Sisgea.GerarHorario.Core.Dtos.Configuracoes;

namespace Sisgea.GerarHorario.Core;

public class GerarHorarioContext
{
  public GerarHorarioOptions Options { get; init; }
  public CpModel Model { get; init; }

  public List<PropostaAula> TodasAsPropostasDeAula { get; init; }


  public GerarHorarioContext(GerarHorarioOptions options, CpModel? model = null, List<PropostaAula>? todasAsPropostasDeAula = null)
  {
    Options = options;
    Model = model ?? new CpModel();
    TodasAsPropostasDeAula = todasAsPropostasDeAula ?? [];
  }
}