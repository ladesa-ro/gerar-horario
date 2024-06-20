using Allure.NUnit;
using Sisgea.GerarHorario.Core;
using Sisgea.GerarHorario.Core.Dtos.Configuracoes;
using Sisgea.GerarHorario.Core.Dtos.Entidades;


namespace Sisgea.GerarHorario.Tests;

[AllureNUnit]
public class AgruparDisciplinasTest
{
    [SetUp]
    public void Setup()
    {

    }

   //[Test] 
    public void Test1()
    {

        System.Console.WriteLine("Teste AgruparDisciplinas.cs");
        var turmas = new Turma[] {
            new(
                "1",//TURMA
                "1A INFORMATICA",//NOME DA TURMA
                [
                    new Diario (Id: "diario:1_3", TurmaId: "turma:1", ProfessorId: "1", DisciplinaId: "disciplina:3", QuantidadeMaximaSemana: 3),
                    new Diario (Id: "diario:1_1", TurmaId: "turma:1", ProfessorId:  "2", DisciplinaId: "disciplina:1", QuantidadeMaximaSemana: 1),
                    new Diario (Id: "diario:1_2", TurmaId: "turma:1", ProfessorId: "1", DisciplinaId: "disciplina:2", QuantidadeMaximaSemana: 4),
                ],
                [
                    //
                    new DisponibilidadeDia(DiaSemanaIso.SEGUNDA, new Intervalo("07:30", "11:59:59")),//O 1A INFORMATICA TERA AULA NA SEGUNDA DAS 07:30 AS 12:00
                    //
                    new DisponibilidadeDia(DiaSemanaIso.TERCA, new Intervalo("07:30", "11:59:59")),//O 1A INFORMATICA TERA AULA NA TERÇA DAS 07:30 AS 12:00 E DAS 13:00 AS 17:30
                    new DisponibilidadeDia(DiaSemanaIso.TERCA, new Intervalo("13:00", "17:29:59")),
                    //
                    new DisponibilidadeDia(DiaSemanaIso.QUARTA, new Intervalo("07:30", "11:59:59")),//O 1A INFORMATICA TERA AULA NA QUARTA DAS 07:30 AS 12:00
                    //
                    new DisponibilidadeDia(DiaSemanaIso.QUINTA, new Intervalo("07:30", "11:59:59")),//O 1A INFORMATICA TERA AULA NA QUINTA DAS 07:30 AS 12:00 E DAS 13:00 AS 17:30
                    new DisponibilidadeDia(DiaSemanaIso.QUINTA, new Intervalo("13:00", "17:29:59")),
                    //
                    new DisponibilidadeDia(DiaSemanaIso.SEXTA, new Intervalo("13:00", "17:29:59")),//O 1A INFORMATICA TERA AULA NA SEXTA DAS 13:00 AS 17:30
                ]
            ),

            new(
                "2",
                "1B INFORMATICA",
                [
                    new Diario (Id: "diario:2_1", TurmaId: "turma:2", ProfessorId: "2", DisciplinaId: "disciplina:4", QuantidadeMaximaSemana: 1),
                    new Diario (Id: "diario:2_3", TurmaId: "turma:2", ProfessorId: "1", DisciplinaId: "disciplina:1", QuantidadeMaximaSemana: 3),
                    new Diario (Id: "diario:2_2", TurmaId: "turma:2", ProfessorId: "2", DisciplinaId: "disciplina:2", QuantidadeMaximaSemana: 2),
                ],
                [
                    //
                    new DisponibilidadeDia(DiaSemanaIso.SEGUNDA, new Intervalo("13:00", "17:29:59")),//SEGUNDA DAS 13:00 AS 17:30
                    //
                    new DisponibilidadeDia(DiaSemanaIso.TERCA, new Intervalo("07:30", "11:59:59")),//TERCA DAS 07:30 AS 12:00 E AS 13:00 AS 17:30
                    new DisponibilidadeDia(DiaSemanaIso.TERCA, new Intervalo("13:00", "17:29:59")),//TERCA
                    //
                    new DisponibilidadeDia(DiaSemanaIso.QUARTA, new Intervalo("07:30", "11:59:59")),//QUARTA DAS 07:30 AS 12:00
                    //
                    new DisponibilidadeDia(DiaSemanaIso.QUINTA, new Intervalo("07:30", "11:59:59")),//QUINTA DAS 07:30 AS 12:00 E 13:00 AS 17:30
                    new DisponibilidadeDia(DiaSemanaIso.QUINTA, new Intervalo("13:00", "17:29:59")),//QUINTA
                    //
                    new DisponibilidadeDia(DiaSemanaIso.SEXTA, new Intervalo("07:30", "11:59:59")),//SEXTA DAS 07:30 AS 12:00
                ]
            ),

             new(
                "3",
                "1 PERIODO ADS",
                [
                    new Diario (Id: "diario:3_1", TurmaId: "turma:3", ProfessorId: "2", DisciplinaId: "disciplina:4", QuantidadeMaximaSemana: 1),
                    new Diario (Id: "diario:3_3", TurmaId: "turma:3", ProfessorId: "1", DisciplinaId: "disciplina:1", QuantidadeMaximaSemana: 3),
                    new Diario (Id: "diario:3_2", TurmaId: "turma:3", ProfessorId: "2", DisciplinaId: "disciplina:2", QuantidadeMaximaSemana: 2),
                ],
                [
                    //
                    new DisponibilidadeDia(DiaSemanaIso.SEGUNDA, new Intervalo("19:00", "23:29:59")),//SEGUNDA DAS 13:00 AS 17:30
                    //
                    new DisponibilidadeDia(DiaSemanaIso.TERCA, new Intervalo("19:00", "23:29:59")),//TERCA DAS 07:30 AS 12:00 E AS 13:00 AS 17:30
                    new DisponibilidadeDia(DiaSemanaIso.TERCA, new Intervalo("19:00", "23:29:59")),//TERCA
                    //
                    new DisponibilidadeDia(DiaSemanaIso.QUARTA, new Intervalo("19:00", "23:29:59")),//QUARTA DAS 07:30 AS 12:00
                    //
                    new DisponibilidadeDia(DiaSemanaIso.QUINTA, new Intervalo("19:00", "23:29:59")),//QUINTA DAS 07:30 AS 12:00 E 13:00 AS 17:30
                    new DisponibilidadeDia(DiaSemanaIso.QUINTA, new Intervalo("19:00", "23:29:59")),//QUINTA
                    //
                    new DisponibilidadeDia(DiaSemanaIso.SEXTA, new Intervalo("19:00", "23:29:59")),//SEXTA DAS 07:30 AS 12:00
                ]
            ),

             new(
                "4",
                "2 PERIODO ADS",
                [
                    new Diario (Id: "diario:4_1", TurmaId: "turma:4", ProfessorId: "2", DisciplinaId: "disciplina:4", QuantidadeMaximaSemana: 1),
                    new Diario (Id: "diario:4_3", TurmaId: "turma:4", ProfessorId: "1", DisciplinaId: "disciplina:1", QuantidadeMaximaSemana: 3),
                    new Diario (Id: "diario:4_2", TurmaId: "turma:4", ProfessorId: "2", DisciplinaId: "disciplina:2", QuantidadeMaximaSemana: 2),
                ],
                [
                    //
                    new DisponibilidadeDia(DiaSemanaIso.SEGUNDA, new Intervalo("19:00", "23:29:59")),//SEGUNDA DAS 13:00 AS 17:30
                    //
                    new DisponibilidadeDia(DiaSemanaIso.TERCA, new Intervalo("19:00", "23:29:59")),//TERCA DAS 07:30 AS 12:00 E AS 13:00 AS 17:30
                    new DisponibilidadeDia(DiaSemanaIso.TERCA, new Intervalo("19:00", "23:29:59")),//TERCA
                    //
                    new DisponibilidadeDia(DiaSemanaIso.QUARTA, new Intervalo("19:00", "23:29:59")),//QUARTA DAS 07:30 AS 12:00
                    //
                    new DisponibilidadeDia(DiaSemanaIso.QUINTA, new Intervalo("19:00", "23:29:59")),//QUINTA DAS 07:30 AS 12:00 E 13:00 AS 17:30
                    new DisponibilidadeDia(DiaSemanaIso.QUINTA, new Intervalo("19:00", "23:29:59")),//QUINTA
                    //
                    new DisponibilidadeDia(DiaSemanaIso.SEXTA, new Intervalo("19:00", "23:29:59")),//SEXTA DAS 07:30 AS 12:00
                ]
            ),
        };

        var professores = new Professor[] {
            new(
                "1",
                "Flinstons",
                [
                    new DisponibilidadeDia(DiaSemanaIso.SEGUNDA, new Intervalo("13:00", "17:29:59")),
                    new DisponibilidadeDia(DiaSemanaIso.TERCA, new Intervalo("07:30", "17:29:59")),
                    new DisponibilidadeDia(DiaSemanaIso.QUARTA, new Intervalo("07:30", "11:59:59")),
                    new DisponibilidadeDia(DiaSemanaIso.QUINTA, new Intervalo("07:30", "11:59:59")),
                    new DisponibilidadeDia(DiaSemanaIso.SEXTA, new Intervalo("07:00", "11:59:59")),

                    new DisponibilidadeDia(DiaSemanaIso.SEGUNDA, new Intervalo("19:00", "23:29:59")),
                    new DisponibilidadeDia(DiaSemanaIso.TERCA, new Intervalo("19:00", "23:29:59")),
                    new DisponibilidadeDia(DiaSemanaIso.QUARTA, new Intervalo("19:00", "23:29:59")),
                    new DisponibilidadeDia(DiaSemanaIso.QUINTA, new Intervalo("19:00", "23:29:59")),
                    new DisponibilidadeDia(DiaSemanaIso.SEXTA, new Intervalo("19:00", "23:29:59")),
                ]
            ),
            new(
                "2",
                "Poucas",
                [
                    new DisponibilidadeDia(DiaSemanaIso.SEGUNDA, new Intervalo("07:30", "17:29:59")),
                    new DisponibilidadeDia(DiaSemanaIso.TERCA, new Intervalo("07:30", "17:29:59")),
                    new DisponibilidadeDia(DiaSemanaIso.QUARTA, new Intervalo("07:30", "11:59:59")),
                    new DisponibilidadeDia(DiaSemanaIso.QUINTA, new Intervalo("13:00", "17:29:59")),
                    new DisponibilidadeDia(DiaSemanaIso.SEXTA, new Intervalo("13:00", "17:29:59")),

                     new DisponibilidadeDia(DiaSemanaIso.SEGUNDA, new Intervalo("19:00", "23:29:59")),
                    new DisponibilidadeDia(DiaSemanaIso.TERCA, new Intervalo("19:00", "23:29:59")),
                    new DisponibilidadeDia(DiaSemanaIso.QUARTA, new Intervalo("19:00", "23:29:59")),
                    new DisponibilidadeDia(DiaSemanaIso.QUINTA, new Intervalo("19:00", "23:29:59")),
                    new DisponibilidadeDia(DiaSemanaIso.SEXTA, new Intervalo("19:00", "23:29:59")),
                ]
            ),
        };

        var horariosDeAula = new Intervalo[] {
            // =====================
            new("07:30", "08:19:59"),
            new("08:20", "09:09:59"),
            new("09:10", "09:59:59"),
            //
            new("10:20", "11:09:59"),
            new("11:10", "11:59:59"),//INTERVALO INDEX 4
            // =====================
          //  new("12:00", "12:59"),//RECREIO 
            // =====================
            new("13:00", "13:49:59"),
            new("13:50", "14:39:59"),
            new("14:40", "15:29:59"),
            //
            new("15:50", "16:39:59"),
            new("16:40", "17:29:59"),
            // =====================
            new("19:00", "19:49:59"),
            new("19:50", "20:39:59"),
            new("20:40", "21:29:59"),
            //
            new("21:50", "22:39:59"),
            new("22:40", "23:29:59"),
        };
        var datas = new Data[]
       {
            new Data(new DateTime(2024, 2, 12), DiaSemanaIso.SEGUNDA),
            new Data(new DateTime(2024, 2, 13), DiaSemanaIso.TERCA),
            new Data(new DateTime(2024, 2, 14), DiaSemanaIso.QUARTA),
            new Data(new DateTime(2024, 2, 15), DiaSemanaIso.QUINTA),
            new Data(new DateTime(2024, 2, 16), DiaSemanaIso.SEXTA)

       };

        var gerarHorarioOptions = new GerarHorarioOptions(
            diaSemanaInicio: DiaSemanaIso.SEGUNDA,
            diaSemanaFim: DiaSemanaIso.SEXTA,
            dataAnual: datas,
            turmas: turmas,
            professores: professores,
            horariosDeAula: horariosDeAula,
            logDebug: false
        );
        var contexto = new GerarHorarioContext(gerarHorarioOptions, iniciarTodasAsPropostasDeAula: true);
        AgruparDisciplinasTestFunction(AgruparDisciplinas(contexto));


    }


    public static (List<PropostaDeAula>, List<Diario>) AgruparDisciplinas(GerarHorarioContext contexto)
    {
        var propostas = new List<PropostaDeAula>();
        var diarios = new List<Diario>();

        foreach (var turma in contexto.Options.Turmas)
        {
            var horariosUsados = new HashSet<(int DiaSemanaIso, int IntervaloIndex)>();

            foreach (var diario in turma.DiariosDaTurma)
            {
                diarios.Add(diario);
                var propostasDoDiario = from propostaAula in contexto.TodasAsPropostasDeAula
                                        where propostaAula.DiarioId == diario.Id
                                        select propostaAula;

                var consecutivas = new List<PropostaDeAula>();
                int skipCount = 0;
                int skipCount1 = 0;
                int skipCount2 = 10;
                PropostaDeAula propostaAnterior = null;


                while (consecutivas.Count < diario.QuantidadeMaximaSemana && skipCount < propostasDoDiario.Count())
                {
                    var propostasSkipadas = propostasDoDiario.Skip(skipCount).Take(1).ToList();

                    if (diario.QuantidadeMaximaSemana == 4)
                    {
                        var primeiraDivisao = propostasDoDiario.Skip(skipCount1).Take(2).ToList();
                        var segundaDivisao = propostasDoDiario.Skip(skipCount2).Take(2).ToList();
                        propostasSkipadas.Clear();
                        propostasSkipadas.AddRange(primeiraDivisao);
                        propostasSkipadas.AddRange(segundaDivisao);
                    }

                    foreach (var proposta in propostasSkipadas)
                    {
                        if (!horariosUsados.Contains((proposta.DiaSemanaIso, proposta.IntervaloIndex)))
                        {
                            // Verifica se a proposta anterior está nos últimos intervalos do dia (IntervaloIndex == 14)
                            if ((propostaAnterior != null && propostaAnterior.IntervaloIndex == 14) || (propostaAnterior != null && propostaAnterior.IntervaloIndex == 9) || (propostaAnterior != null && propostaAnterior.IntervaloIndex == 4))
                            {
                                // Muda as propostas para o próximo dia
                                int proximoDia = propostaAnterior.DiaSemanaIso + 1;
                                propostaAnterior.DiaSemanaIso = proximoDia;
                                propostaAnterior.IntervaloIndex = proposta.IntervaloIndex;
                                proposta.IntervaloIndex = proposta.IntervaloIndex + 1;
                            }

                            consecutivas.Add(proposta);
                            horariosUsados.Add((proposta.DiaSemanaIso, proposta.IntervaloIndex));
                            propostaAnterior = proposta;
                        }
                    }
                    skipCount++;
                    skipCount1 += 2;
                    skipCount2 += 5;
                }


                // Adiciona a restrição para garantir que todas as propostas que não estão em consecutivas sejam falsas
                foreach (var proposta in propostasDoDiario)
                {

                    if (consecutivas.Contains(proposta))
                    {
                        propostas.Add(proposta);
                    }
                }

            }
        }

        foreach (var proposta in propostas)
        {
            System.Console.WriteLine("Diario CONSECUTIVO: " + proposta.DiarioId + " | Dia: " + proposta.DiaSemanaIso + " Intervalo: " + proposta.IntervaloIndex);

        }
        return (propostas, diarios);
    }

    public static void AgruparDisciplinasTestFunction((List<PropostaDeAula>, List<Diario>) dados)
    {
        foreach (var diario in dados.Item2)
        {
            List<string> pulos = new List<string>();
            List<string> sequencia = new List<string>();
            foreach (var proposta in dados.Item1)
            {


                if (proposta.DiarioId == diario.Id)
                {
                    System.Console.WriteLine("Diario CONSECUTIVO: " + proposta.DiarioId + " | Dia: " + proposta.DiaSemanaIso + " Intervalo: " + proposta.IntervaloIndex);


                    pulos.Add(proposta.DiaSemanaIso.ToString() + "|" + proposta.IntervaloIndex.ToString());
                    sequencia.Add(proposta.DiarioId + "|" + proposta.DiaSemanaIso.ToString() + "|" + proposta.IntervaloIndex.ToString());

                    //IDENTIFICAR CONFLITO
                    for (int i = 0; i < pulos.Count; i++)
                    {
                        for (int j = i + 1; j < pulos.Count; j++)
                        {
                            if (pulos[i] == pulos[j])
                            {
                                Assert.Fail("Error: Conflito encontrado!");
                            }
                        }
                    }


                }


                //IDENTIFICAR SEQUENCIAS INCORRETAS
                for (int i = 1; i < sequencia.Count; i++)
                {
                    if (diario.QuantidadeMaximaSemana != 4)
                    {
                        var anterior = sequencia[i - 1].Split('|');
                        var atual = sequencia[i].Split('|');

                        int diaSemanaAnterior = int.Parse(anterior[1]);
                        int intervaloAnterior = int.Parse(anterior[2]);

                        int diaSemanaAtual = int.Parse(atual[1]);
                        int intervaloAtual = int.Parse(atual[2]);

                        // Verifica se os dias da semana são iguais e os intervalos são consecutivos
                        if (diaSemanaAnterior != diaSemanaAtual || intervaloAtual != intervaloAnterior + 1)
                        {
                            Assert.Fail("Error: Sequencia incorreta!");
                        }
                    }
                }
            }
        }
        foreach (var proposta in dados.Item1)
        {
            //System.Console.WriteLine("Diario CONSECUTIVO: " + proposta.DiarioId + " | Dia: " + proposta.DiaSemanaIso + " Intervalo: " + proposta.IntervaloIndex);

        }



    }
}

