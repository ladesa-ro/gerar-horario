




using Google.OrTools.Sat;
using Sisgea.GerarHorario.Core.Dtos.Configuracoes;

namespace Sisgea.GerarHorario.Core;

public class Restricoes
{

  public static void AplicarLimiteDeDiarioNaSemana(
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
  public static void AplicarLimiteDeNoMaximoUmDiarioAtivoPorTurmaEmUmHorario(GerarHorarioContext contexto)
  {
    foreach (var diaSemanaIso in Enumerable.Range(contexto.Options.DiaSemanaInicio, contexto.Options.DiaSemanaFim))
    {
      foreach (var intervaloIndex in Enumerable.Range(0, contexto.Options.HorariosDeAula.Length))
      {
        foreach (var turma in contexto.Options.Turmas)
        {
          var propostas = (from propostaAula in contexto.TodasAsPropostasDeAula
                           where
                              propostaAula.DiaSemanaIso == diaSemanaIso // mesmo dia
                              && propostaAula.IntervaloIndex == intervaloIndex // mesmo horário
                              && turma.DiariosDaTurma.Any(diario => diario.Id == propostaAula.DiarioId)
                           select propostaAula.ModelBoolVar).ToList();



          Console.WriteLine($"Dia: {diaSemanaIso} | Intervalo: {contexto.Options.HorariosDeAula[intervaloIndex]} | {turma.Id} | Quantidade de Propostas: {propostas.Count}");

          contexto.Model.AddAtMostOne(propostas);
        }
      }

      Console.WriteLine("");
    }
  }
}