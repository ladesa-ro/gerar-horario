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
                    new Diario ("diario:1_3", "turma:1", "professor:1", "disciplina:3", 1),
                    new Diario ("diario:1_1", "turma:1", "professor:1", "disciplina:1", 3),
                    new Diario ("diario:1_2", "turma:1", "professor:1", "disciplina:2", 2),
                ],
                [
                    //
                    new DisponibilidadeDia(1, new Intervalo("07:30", "12:00")),//O 1A INFORMATICA TERA AULA NA SEGUNDA DAS 07:30 AS 12:00
                    //
                    new DisponibilidadeDia(2, new Intervalo("07:30", "12:00")),//O 1A INFORMATICA TERA AULA NA TERÇA DAS 07:30 AS 12:00 E DAS 13:00 AS 17:30
                    new DisponibilidadeDia(2, new Intervalo("13:00", "17:30")),
                    //
                    new DisponibilidadeDia(3, new Intervalo("07:30", "12:00")),//O 1A INFORMATICA TERA AULA NA QUARTA DAS 07:30 AS 12:00
                    //
                    new DisponibilidadeDia(4, new Intervalo("07:30", "12:00")),//O 1A INFORMATICA TERA AULA NA QUINTA DAS 07:30 AS 12:00 E DAS 13:00 AS 17:30
                    new DisponibilidadeDia(4, new Intervalo("13:00", "17:30")),
                    //
                    new DisponibilidadeDia(5, new Intervalo("13:00", "17:30")),//O 1A INFORMATICA TERA AULA NA SEXTA DAS 13:00 AS 17:30
                ]
            ),



            new(
                "2",
                "1B INFORMATICA",
                [
                    new Diario ("diario:2_1", "turma:2", "professor:2", "disciplina:4", 1),
                    new Diario ("diario:2_3", "turma:2", "professor:2", "disciplina:1", 3),
                    new Diario ("diario:2_2", "turma:2", "professor:2", "disciplina:2", 2),
                ],
                [
                    //
                    new DisponibilidadeDia(1, new Intervalo("13:00", "17:30")),//SEGUNDA DAS 13:00 AS 17:30
                    //
                    new DisponibilidadeDia(2, new Intervalo("07:30", "12:00")),//TERCA DAS 07:30 AS 12:00 E AS 13:00 AS 17:30
                    new DisponibilidadeDia(2, new Intervalo("13:00", "17:30")),//TERCA
                    //
                    new DisponibilidadeDia(3, new Intervalo("07:30", "12:00")),//QUARTA DAS 07:30 AS 12:00
                    //
                    new DisponibilidadeDia(4, new Intervalo("07:30", "12:00")),//QUINTA DAS 07:30 AS 12:00 E 13:00 AS 17:30
                    new DisponibilidadeDia(4, new Intervalo("13:00", "17:30")),//QUINTA
                    //
                    new DisponibilidadeDia(5, new Intervalo("07:30", "12:00")),//SEXTA DAS 07:30 AS 12:00
                ]
            ),
        };

        var professores = new Professor[] {
            new(
                "1",
                "Flinstons",
                [
                   new DisponibilidadeDia(1, new Intervalo("13:00", "17:30"))
                ]
            ),
        };

        var horariosDeAula = new Intervalo[] {
            new("07:30", "08:20"),
            new("08:20", "09:10"),
            new("09:10", "10:00"),
            new("10:20", "11:10"),
            new("11:10", "12:00"),
        };

        var gerarHorarioOptions = new GerarHorarioOptions((int)DiaSemanaIso.SEGUNDA, (int)DiaSemanaIso.SEXTA, turmas, professores, horariosDeAula);

        // ====================================================
        Console.WriteLine("[debug] vamo chamar o GerarHorario");
        var horarioGeradoEnumerator = Gerador.GerarHorario(gerarHorarioOptions);
        Console.WriteLine("[debug] <- GerarHorario retornou");
        // ====================================================

        var limiteGeracao = 1;
        var indiceGeracao = 0;

        foreach (var horarioGerado in horarioGeradoEnumerator)
        {

            if (indiceGeracao < limiteGeracao)
            {
                Console.WriteLine($"\n\n+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                Console.WriteLine($"[ HORARIO {indiceGeracao + 1} ]");
                Console.WriteLine($"+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");

                Console.WriteLine("============================");
                Console.WriteLine("Horário gerado:");
                Console.WriteLine("");
                Console.WriteLine(horarioGerado);
                Console.WriteLine("============================");
                Console.WriteLine("");

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


        Console.WriteLine("");
        Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
        Console.WriteLine($"-- quantidade de horários gerados: {indiceGeracao} | limite: {limiteGeracao}");
        Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");

        // ====================================================

        return true;
    }
}

