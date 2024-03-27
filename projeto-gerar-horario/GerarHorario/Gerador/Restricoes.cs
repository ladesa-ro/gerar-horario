




using Google.OrTools.Sat;
using Sisgea.GerarHorario.Core.Dtos.Configuracoes;

namespace Sisgea.GerarHorario.Core;

public class Restricoes
{
  public static void AplicarLimiteDiarioSemana(
    GerarHorarioContext contexto
  )
  {
    foreach (var turma in contexto.Options.Turmas)
    {
      foreach (var diario in turma.DiariosDaTurma)
      {
        var propostasDoDiario = from propostaAula in contexto.TodasAsPropostasDeAula
                                where
                                    propostaAula.DiarioId == diario.Id
                                select propostaAula.ModelBoolVar;

        contexto.Model.Add(LinearExpr.Sum(propostasDoDiario) <= diario.QuantidadeMaximaSemana);
      }
    }

  }
}