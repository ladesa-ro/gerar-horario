using System.Globalization;
using System.Runtime.InteropServices;
using Google.OrTools.Sat;
using Sisgea.GerarHorario.Core.Dtos.Configuracoes;
using Sisgea.GerarHorario.Core.Dtos.Entidades;

namespace Sisgea.GerarHorario.Core;

using CombinacaoAula = (int diaSemanaIso, int intervaloIndex, string turmaId, string diarioId, string professorId);

public class Restricoes
{
    ///<summary>
    /// UTILITÁRIO: Verifica que um (diaSemanaIso, intervalo)
    /// pode ocorer em um conjunto de disponibilidades.
    ///</summary>
    public static bool VerificarIntervaloEmDisponibilidades(
        IEnumerable<DisponibilidadeDia> disponibilidades,
        int diaSemanaIso,
        Intervalo intervalo
    )
    {

        return disponibilidades.Any(disponibilidade =>
        {
            if (disponibilidade.DiaSemanaIso == diaSemanaIso)
            {
                return Intervalo.VerificarIntervalo(disponibilidade.Intervalo, intervalo);
            }

            return false;
        });
    }

    ///<summary>
    /// UTILITÁRIO: Gera uma lista com todas as combinações de aula possíveis
    /// sem respeitar nenhum critério.
    ///</summary>
    public static IEnumerable<CombinacaoAula> GerarTodasAsCombinacoesPossiveisInclusiveIndisponiveis(GerarHorarioOptions options)
    {
        for (int diaSemanaIso = options.DiaSemanaInicio; diaSemanaIso <= options.DiaSemanaFim; diaSemanaIso++)
        {
            for (int intervaloIndex = 0; intervaloIndex < options.HorariosDeAula.Length; intervaloIndex++)
            {
                foreach (var turma in options.Turmas)
                {
                    foreach (var diario in options.DiariosByTurmaId(turma.Id))
                    {
                        yield return (diaSemanaIso, intervaloIndex, turma.Id, diario.Id, diario.ProfessorId);
                    }
                }
            }
        }
    }

    ///<summary>
    /// UTILITÁRIO: Gera uma lista com todas as combinações de aula possíveis,
    /// respeitando as disponibilidades da turma e disponibilidades do professor.
    ///</summary>
    public static IEnumerable<CombinacaoAula> GerarCombinacoesComDisponibilidade(GerarHorarioOptions options)
    {
        foreach (var combinacao in Restricoes.GerarTodasAsCombinacoesPossiveisInclusiveIndisponiveis(options))
        {
            // =====================================================================================
            var intervaloDeTempo = options.HorarioDeAulaFindByIdStrict(combinacao.intervaloIndex);

            var turma = options.TurmaFindByIdStrict(combinacao.turmaId);
            var diario = options.DiarioFindByIdStrict(combinacao.diarioId);

            var professor = options.ProfessorFindByIdStrict(
                diario.ProfessorId,
                exceptionContext: $" (diário: {diario.Id}, turma: {turma.Id})"
            )!;

            // =====================================================================================

            var disponivelParaTurma = Restricoes.VerificarIntervaloEmDisponibilidades(turma.Disponibilidades, combinacao.diaSemanaIso, intervaloDeTempo);

            // ===================================

            var disponivelParaProfessor = Restricoes.VerificarIntervaloEmDisponibilidades(professor.Disponibilidades, combinacao.diaSemanaIso, intervaloDeTempo);

            // ===================================

            var disponivel = disponivelParaTurma && disponivelParaProfessor;

            // =====================================================================================

            if (disponivel)
            {
                yield return combinacao;
            }
        }
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

                if (propostasDoDiario.Any())
                {
                    contexto.Model.Add(LinearExpr.Sum(propostasDoDiario) <= diario.QuantidadeMaximaSemana);
                }
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

                    if (propostas.Any())
                    {
                        Console.WriteLine($"Dia: {diaSemanaIso} | Intervalo: {contexto.Options.HorariosDeAula[intervaloIndex]} | {turma.Id} | Quantidade de Propostas: {propostas.Count}");

                        contexto.Model.AddAtMostOne(propostas);
                    }

                }
            }

            Console.WriteLine("");
        }
    }

    ///<summary>
    /// RESTRIÇÃO: Professor: não ter mais de uma aula ativa ao mesmo tempo.
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
                                        contexto.Options.ProfessorEstaVinculadoAoDiario(diarioId: propostaDeAula.DiarioId, professorId: professor.Id)
                                    select propostaDeAula.ModelBoolVar;

                    if (propostas.Any())
                    {

                        contexto.Model.AddAtMostOne(propostas);
                    }

                }
            }
        }
    }

    public static void HorarioAlmocoProfessor(GerarHorarioContext contexto)
    {
        foreach (var professor in contexto.Options.Professores)
        {
            foreach (var diaSemanaIso in Enumerable.Range(contexto.Options.DiaSemanaInicio, contexto.Options.DiaSemanaFim))
            {
                var propostaAulaProfessor = from proposta in contexto.TodasAsPropostasDeAula
                                            where proposta.DiaSemanaIso == diaSemanaIso
                                                && proposta.ProfessorId == professor.Id
                                                && (
                                                    Intervalo.VerificarIntervalo(
                                                        new Intervalo("11:30:00", "12:00:00"),
                                                        proposta.Intervalo.HorarioFim
                                                    )
                                                    || Intervalo.VerificarIntervalo(
                                                        new Intervalo("13:00:00", "13:30:00"),
                                                        proposta.Intervalo.HorarioInicio
                                                    )
                                                )
                                            select proposta.ModelBoolVar;

                contexto.Model.AddAtMostOne(propostaAulaProfessor);
            }
        }
    }

}