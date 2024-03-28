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
                "turma:1",
                "Turma da Pesada",
                [
                    new Diario ("diario:1_3", "turma:1", "professor:1", "química", 1),
                    new Diario ("diario:1_1", "turma:1", "professor:1", "língua portuguesa", 3),
                    new Diario ("diario:1_2", "turma:1", "professor:1", "geografia", 2),
                ],
                [
                    //
                    new DisponibilidadeDia(1, new Intervalo("07:30", "12:00")),
                    //
                    new DisponibilidadeDia(2, new Intervalo("07:30", "12:00")),
                    new DisponibilidadeDia(2, new Intervalo("13:00", "17:30")),
                    //
                    new DisponibilidadeDia(3, new Intervalo("07:30", "12:00")),
                    //
                    new DisponibilidadeDia(4, new Intervalo("07:30", "12:00")),
                    new DisponibilidadeDia(4, new Intervalo("13:00", "17:30")),
                    //
                    new DisponibilidadeDia(5, new Intervalo("13:00", "17:30")),
                ]
            ),
            new(
                "turma:2",
                "Turma diferenciada",
                [
                    new Diario ("diario:2_1", "turma:2", "professor:2", "química", 1),
                    new Diario ("diario:2_2", "turma:2", "professor:2", "redes", 2),
                    new Diario ("diario:2_3", "turma:2", "professor:2", "filosofia", 3),
                    new Diario ("diario:2_4", "turma:2", "professor:2", "educação física", 2),
                ],
                [
                    //
                    new DisponibilidadeDia(1, new Intervalo("13:00", "17:30")),
                    //
                    new DisponibilidadeDia(2, new Intervalo("07:30", "12:00")),
                    new DisponibilidadeDia(2, new Intervalo("13:00", "17:30")),
                    //
                    new DisponibilidadeDia(3, new Intervalo("07:30", "12:00")),
                    //
                    new DisponibilidadeDia(4, new Intervalo("07:30", "12:00")),
                    new DisponibilidadeDia(4, new Intervalo("13:00", "17:30")),
                    //
                    new DisponibilidadeDia(5, new Intervalo("07:30", "12:00")),
                ]
            ),
        };

        var professores = new Professor[] {
            new(
                "professor:1",
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

        var limiteGeracao = 2;
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

                        Console.WriteLine($"- Dia: {dia} | Intervalo: {horariosDeAula[aula.IntervaloDeTempo]} | {diario.DisciplinaId}");

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

