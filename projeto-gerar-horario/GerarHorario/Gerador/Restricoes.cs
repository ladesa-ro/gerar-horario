using Google.OrTools.Sat;

namespace Sisgea.GerarHorario.Core;

public class Restricoes
{

    ///<summary>
    /// RESTRIÇÃO: Diário: respeitar limite de quantidade máxima na semana.
    ///</summary>
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

    ///<summary>
    /// RESTRIÇÃO: Turma: não ter mais de uma aula ativa ao mesmo tempo.
    ///</summary>
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