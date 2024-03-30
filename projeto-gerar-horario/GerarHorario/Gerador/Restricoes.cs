using Google.OrTools.Sat;
using Sisgea.GerarHorario.Core.Dtos.Entidades;

namespace Sisgea.GerarHorario.Core;

public class Restricoes
{
    public static bool VerificarIntervaloEmDisponibilidades(
        IEnumerable<DisponibilidadeDia> disponibilidades,
        int diaSemanaIso,
#pragma warning disable IDE0060 // Remover o parâmetro não utilizado
        Intervalo intervalo
#pragma warning restore IDE0060 // Remover o parâmetro não utilizado
    )
    {
        return disponibilidades.Any(disponibilidade =>
        {
            if (disponibilidade.DiaSemanaIso == diaSemanaIso)
            {
                // TODO: retornar corretamente
                // return Intervalo.VerificarIntervalo(disponibilidade.Intervalo, intervaloAula)
            }
            return true;
        });
    }

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
                                        && propostaAula.TurmaId == turma.Id // mesma turma
                                     select propostaAula.ModelBoolVar).ToList();



                    Console.WriteLine($"Dia: {diaSemanaIso} | Intervalo: {contexto.Options.HorariosDeAula[intervaloIndex]} | {turma.Id} | Quantidade de Propostas: {propostas.Count}");

                    contexto.Model.AddAtMostOne(propostas);
                }
            }

            Console.WriteLine("");
        }
    }

    ///<summary>
    /// RESTRIÇÃO: PREVENCAO CASO O MESMO PROFESSOR ESTEJA DANDO AULA EM DIAS IGUAIS E EM MESMOS HORARIOS
    ///</summary>
    public static void AplicarLimiteDeNoMaximoUmDiarioAtivoPorProfessorEmUmHorario(GerarHorarioContext contexto)
    {

        foreach (var professor in contexto.Options.Professores)
        {
            foreach (var diaSemanaIso in Enumerable.Range(contexto.Options.DiaSemanaInicio, contexto.Options.DiaSemanaFim))
            {
                foreach (var intervaloIndex in Enumerable.Range(0, contexto.Options.HorariosDeAula.Length))
                {
                    var propostas = from propostaDeAula in contexto.TodasAsPropostasDeAula
                                    where
                                       propostaDeAula.DiaSemanaIso == diaSemanaIso
                                       &&
                                       propostaDeAula.IntervaloIndex == intervaloIndex
                                       &&
                                        contexto.Options.Turmas.Any(turma => turma.DiariosDaTurma.Any(diario => diario.ProfessorId == professor.Id))
                                    select propostaDeAula.ModelBoolVar;

                    contexto.Model.AddAtMostOne(propostas);
                }
            }
        }
    }
}