using Google.OrTools.Sat;
using Sisgea.GerarHorario.Core.Dtos.Configuracoes;
using Sisgea.GerarHorario.Core.Dtos.HorarioGerado;

namespace Sisgea.GerarHorario.Core;

public class Gerador
{
    ///<summary>
    /// Ponto de partida que inicia, restringe e otimizar o modelo para
    /// solucionar o problema da geração de horário.
    ///</summary>
    public static GerarHorarioContext PrepararModelComRestricoes(GerarHorarioOptions options)
    {
        // ====================================================================
        // contexto.Model -> Google.OrTools.Sat.CpModel;
        // contexto.Options -> GerarHorarioOptions;
        // contexto.TodasAsPropostasDeAula -> List<PropostaDeAula>;
        var contexto = new GerarHorarioContext(options, iniciarTodasAsPropostasDeAula: true);
        // ================================================


        // ====================================================================
        // RESTRIÇÃO: Turma: não ter mais de uma aula ativa ao mesmo tempo.
        Restricoes.AplicarLimiteDeNoMaximoUmDiarioAtivoPorTurmaEmUmHorario(contexto);
        
        // ======================================
        // RESTRIÇÃO: Professor: não ter mais de uma aula ativa ao mesmo tempo.
        //Restricoes.AplicarLimiteDeNoMaximoUmDiarioAtivoPorProfessorEmUmHorario(contexto);
        // ======================================
        // RESTRIÇÃO: Diário: respeitar limite de quantidade máxima na semana.
      //Restricoes.AplicarLimiteDeDiarioNaSemana(contexto);
        // ======================================
        //RESTRIÇÃO: Aplicar horario de almoço.
        Console.WriteLine("ONDE EU QUERO TESTAR ESTA ABAIXO");
       Restricoes.AplicarHorarioDeAlmoco(contexto);
        // ====================================================================



        // Ajudar o modelo para gerar o resultado mais satisfatório dentre
        // todas as soluções possíveis.
        Gerador.OtimizarResultadoDeAcordoComAsPreferencias(contexto);

        // ====================================================================

        return contexto;
    }

    public static IEnumerable<HorarioGerado> GerarHorario(
      GerarHorarioOptions options)
    {
        // CRIA UM MODELO COM AS RESTRIÇÕES VINDAS DAS OPÇÕES
        var contexto = PrepararModelComRestricoes(options);


        // ==============================================================

        // Gatilho para quando o "o thread de solução foi iniciado".
        var tickThreadStarted = new AutoResetEvent(false);

        // Gatilho para quando "um horário foi gerado".
        var tickGenerated = new AutoResetEvent(false);

        // Gatilho para quando "houver permissão para gerar o próximo horário".
        var tickGenerateNext = new AutoResetEvent(false);

        HorarioGerado? horarioGerado = null;

        // thread de solução de horário para essa requisição
        var solutionGeneratorThread = new Thread(() =>
        {
            Console.WriteLine("==> [thread de solução] | iniciado");
            tickThreadStarted.Set();

            Console.WriteLine("==> [thread de solução] | aguardando permissão para iniciar o solver.Solve");
            tickGenerateNext.WaitOne();

            Console.WriteLine("==> [thread de solução] | recebeu permissão para iniciar a geração");

            long? previousScore = null;

            do
            {
                var solver = new CpSolver
                {
                    StringParameters = "enumerate_all_solutions:true"
                };

                var solutionPrinter = new GeradorSolutionCallback(contexto, (spHorarioGerado) =>
                {
                    horarioGerado = spHorarioGerado;
                    Console.WriteLine("==> [thread de solução] | gerou um horário");

                    Console.WriteLine("==> [thread de solução] | disparando horário foi gerado");
                    tickGenerated.Set();

                    Console.WriteLine("==> [thread de solução] | aguardando permissão para continuar a geração");
                    tickGenerateNext.WaitOne();
                    Console.WriteLine("==> [thread de solução] | recebeu permissão para continuar a geração");
                });

                if (previousScore != null)
                {
                    Gerador.OtimizarResultadoDeAcordoComAsPreferencias(contexto, previousScore - 1);
                }

                var sat = solver.Solve(contexto.Model, solutionPrinter);

                Console.WriteLine($"==> [thread de solução] | sat: {sat}");

                if (sat == CpSolverStatus.Feasible || sat == CpSolverStatus.Optimal)
                {
                    var solverScore = solver.ObjectiveValue;
                    Console.WriteLine($"==> [thread de solução] | solverScore: {solverScore}.");
                    previousScore = (long)solverScore;
                }
                else
                {
                    previousScore = 0;
                }

                Console.WriteLine("");
                Console.WriteLine("============================");
                Console.WriteLine("Estatísticas");
                Console.WriteLine($"Status da solução: {sat}");

                if (sat == CpSolverStatus.Optimal || sat == CpSolverStatus.Feasible)
                {
                    Console.WriteLine($"  - Score do horário: {solver.ObjectiveValue}");
                }
                else
                {
                    Console.WriteLine("  - Score do horário: Solução viável não foi encontrada.");
                }

                Console.WriteLine($"  - conflitos : {solver.NumConflicts()}");
                Console.WriteLine($"  - galhos    : {solver.NumBranches()}");
                Console.WriteLine($"  - wall time : {solver.WallTime()}s");

                Console.WriteLine("============================");
                Console.WriteLine("");
            } while (previousScore > 0);

            Console.WriteLine("==> [thread de solução] | terminou a geração de todas as soluções possíveis");

            horarioGerado = null;
            tickGenerated.Set();
        });


        solutionGeneratorThread.Start();

        Console.WriteLine("=> Gerador#gerarHorario | aguarda o thread de solução iniciar");
        tickThreadStarted.WaitOne();
        Console.WriteLine("=> Gerador#gerarHorario | thread de solução iniciou");

        do
        {
            Console.WriteLine("=> Gerador#gerarHorario | permite a geração do próximo horário");
            tickGenerateNext.Set();

            Console.WriteLine("=> Gerador#gerarHorario | aguardando a geração do próximo horário");
            tickGenerated.WaitOne();

            if (horarioGerado != null)
            {
                Console.WriteLine("=> Gerador#gerarHorario | geração do próximo horário recebida");
                yield return horarioGerado;
            }
            else
            {
                Console.WriteLine("=> Gerador#gerarHorario | recebeu um horário nulo. bye.");
            }

        } while (horarioGerado != null);

        yield break;
    }

    ///<summary>
    /// Visto que podem haver várias soluções válidas possíveis, precisamos
    /// otimizar a resposta para que seja a mais satisfatória possível de
    /// acordo com as preferências de agrupamento da turma e preferências
    /// de cada professor.
    ///</summary>
    public static void OtimizarResultadoDeAcordoComAsPreferencias(GerarHorarioContext contexto, long? limiteScore = null)
    {
        var qualidade = LinearExpr.NewBuilder();

        foreach (var propostaDeAula in contexto.TodasAsPropostasDeAula)
        {
            qualidade.AddTerm((IntVar)propostaDeAula.ModelBoolVar, 1);
        }

        if (limiteScore != null)
        {
            contexto.Model.Add(qualidade <= contexto.Model.NewConstant((long)limiteScore));
        }

        contexto.Model.Maximize(qualidade);
    }

}

