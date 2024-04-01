using Sisgea.GerarHorario.Core.Dtos.Configuracoes;
using Sisgea.GerarHorario.Core.Dtos.Entidades;

namespace Sisgea.GerarHorario.Core;

public class Main
{

    public bool Retorno()
    {
        // ====================================================

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
                    new DisponibilidadeDia(5, new Intervalo("13:00", "17:29:59")),//O 1A INFORMATICA TERA AULA NA SEXTA DAS 13:00 AS 17:30
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
                    new DisponibilidadeDia(DiaSemanaIso.SEXTA, new Intervalo("13:00", "17:29:59")),
                ]
            ),
            new(
                "2",
                "Poucas",
                [
                    new DisponibilidadeDia(DiaSemanaIso.SEGUNDA, new Intervalo("07:30", "17:29:59")),
                    new DisponibilidadeDia(DiaSemanaIso.TERCA, new Intervalo("07:30", "17:29:59")),
                    new DisponibilidadeDia(DiaSemanaIso.QUARTA, new Intervalo("07:30", "11:59:59")),
                    new DisponibilidadeDia(DiaSemanaIso.QUINTA, new Intervalo("07:30", "11:59:59")),
                    new DisponibilidadeDia(DiaSemanaIso.SEXTA, new Intervalo("13:00", "17:29:59")),
                ]
            ),
        };

        var horariosDeAula = new Intervalo[] {
            new("07:30", "08:19:59"),
            new("08:20", "09:09:59"),
            new("09:10", "09:59:59"),
            new("10:20", "11:09:59"),
            new("11:10", "11:59:59"),
        };

        var gerarHorarioOptions = new GerarHorarioOptions(
            diaSemanaInicio: DiaSemanaIso.SEGUNDA,
            diaSemanaFim: DiaSemanaIso.SEXTA,
            turmas: turmas,
            professores: professores,
            horariosDeAula: horariosDeAula,
            logDebug: false
        );

        // ====================================================
        var horarioGeradoEnumerator = Gerador.GerarHorario(gerarHorarioOptions);
        // ====================================================

        var limiteGeracao = 1;
        var indiceGeracao = 0;

        foreach (var horarioGerado in horarioGeradoEnumerator)
        {

            if (indiceGeracao < limiteGeracao)
            {
                /*Console.WriteLine($"\n\n+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                Console.WriteLine($"[ HORARIO {indiceGeracao + 1} ]");
                Console.WriteLine($"+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");

                Console.WriteLine("============================");
                Console.WriteLine("Horário gerado:");
                Console.WriteLine("");
                Console.WriteLine(horarioGerado);
                Console.WriteLine("============================");
                Console.WriteLine("");*/

                string? diaAnterior = null;

                foreach (var turma in gerarHorarioOptions.Turmas)
                {
                    Console.WriteLine($"Turma (Id={turma.Id}, Nome={turma.Nome ?? "Sem nome"})");

                    var turmaAulas = from aula in horarioGerado.Aulas
                                     where aula.TurmaId == turma.Id
                                     select aula;


                    foreach (var aula in turmaAulas)
                    {
                        string dia = Convert.ToString(aula.DiaDaSemanaIso);

                        switch (aula.DiaDaSemanaIso)
                        {
                            case 0:
                                {
                                    dia = "DOM";
                                    break;
                                }
                            case 1:
                                {
                                    dia = "SEG";
                                    break;
                                }
                            case 2:
                                {
                                    dia = "TER";
                                    break;
                                }
                            case 3:
                                {
                                    dia = "QUA";
                                    break;
                                }
                            case 4:
                                {
                                    dia = "QUI";
                                    break;
                                }
                            case 5:
                                {
                                    dia = "SEX";
                                    break;
                                }
                            case 6:
                                {
                                    dia = "SAB";
                                    break;
                                }
                        }

                        var diario = turma.DiariosDaTurma.Where(diario => diario.Id == aula.DiarioId).First();

                        if (dia != diaAnterior)
                        {
                            Console.WriteLine("");
                        }

                        Console.WriteLine($"- Dia: {dia} | Intervalo: {horariosDeAula[aula.IntervaloDeTempo]} | Professor: {diario.ProfessorId}");




                        diaAnterior = dia;
                    }
                    Console.WriteLine();

                }

                indiceGeracao++;
            }
            else
            {
                break;
            }

        }


        /*Console.WriteLine("");
        Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
        Console.WriteLine($"-- quantidade de horários gerados: {indiceGeracao} | limite: {limiteGeracao}");
        Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");

        // ====================================================*/

        return true;
    }
}

