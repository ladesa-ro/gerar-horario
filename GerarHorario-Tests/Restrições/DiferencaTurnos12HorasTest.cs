using Allure.NUnit;
using Sisgea.GerarHorario.Core;
using Sisgea.GerarHorario.Core.Dtos.Configuracoes;
using Sisgea.GerarHorario.Core.Dtos.Entidades;


namespace Sisgea.GerarHorario.Tests;

[AllureNUnit]
public class DiferencaTurnos12HorasTest
{
    [SetUp]
    public void Setup()
    {

    }

    public static bool Retorno { get; set; }
    [Test]
    public void Test1()
    {
        System.Console.WriteLine("Teste DiferencaTurnos12Horas.cs");
        var turmas = new Turma[] {
            new(
                "1",//TURMA
                "1A INFORMATICA",//NOME DA TURMA
                [
                    new Diario (Id: "diario:1_3", TurmaId: "turma:1", ProfessorId: "1", DisciplinaId: "disciplina:3", QuantidadeMaximaSemana: 1),
                    new Diario (Id: "diario:1_1", TurmaId: "turma:1", ProfessorId:  "2", DisciplinaId: "disciplina:1", QuantidadeMaximaSemana: 3),
                    new Diario (Id: "diario:1_2", TurmaId: "turma:1", ProfessorId: "1", DisciplinaId: "disciplina:2", QuantidadeMaximaSemana: 2),
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
        var gerarHorarioOptions = new GerarHorarioOptions(
           diaSemanaInicio: DiaSemanaIso.SEGUNDA,
           diaSemanaFim: DiaSemanaIso.SEXTA,
           turmas: turmas,
           professores: professores,
           horariosDeAula: horariosDeAula,
           logDebug: false
       );
        var contexto = new GerarHorarioContext(gerarHorarioOptions, iniciarTodasAsPropostasDeAula: true);


        foreach (var diaSemanaIso in Enumerable.Range(contexto.Options.DiaSemanaInicio, contexto.Options.DiaSemanaFim - 1))
        {
            foreach (var professor in contexto.Options.Professores)
            {
                var propostasNoite = new List<PropostaDeAula>();

                foreach (var proposta in contexto.TodasAsPropostasDeAula)
                {
                    if (proposta.ProfessorId == professor.Id && proposta.DiaSemanaIso == diaSemanaIso && proposta.IntervaloIndex >= 10 && proposta.IntervaloIndex <= 14)
                    {
                        propostasNoite.Add(proposta);
                    }
                }

                foreach (var propostaNoite in propostasNoite)
                {
                    int diaSemanaIsoSeguinte = (diaSemanaIso % 7) + 1;

                    var propostasConflitantesManhaSeguinte = new List<PropostaDeAula>();

                    foreach (var proposta in contexto.TodasAsPropostasDeAula)
                    {
                        if (proposta.DiaSemanaIso == diaSemanaIsoSeguinte && proposta.ProfessorId == propostaNoite.ProfessorId && proposta.IntervaloIndex >= 0 && proposta.IntervaloIndex <= 4 && proposta.IntervaloIndex <= propostaNoite.IntervaloIndex - 10)
                        {
                            propostasConflitantesManhaSeguinte.Add(proposta);
                        }
                    }

                    foreach (var propostaConflitante in propostasConflitantesManhaSeguinte)
                    {
                        Console.WriteLine($"- Dia: {propostaConflitante.DiaSemanaIso} | Intervalo: {horariosDeAula[propostaConflitante.IntervaloIndex]} | Professor: {propostaConflitante.ProfessorId} | Turma: {propostaConflitante.TurmaId}");
                        Retorno = true;

                    }
                }
            }
            System.Console.WriteLine("\n");
        }
        Assert.That(Retorno, Is.False);
    }
}

