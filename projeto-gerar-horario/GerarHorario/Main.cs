using Sisgea.GerarHorario.Core.Dtos.Configuracoes;
using Sisgea.GerarHorario.Core.Dtos.Entidades;

namespace Sisgea.GerarHorario.Core;

public class Main
{

    public bool Retorno()
    {
        var verbose = false;

        var gerarTodosOsHorarios = false;

        // ====================================================

        var turmas = new Turma[] {
            new(
                "turma:1",
                "Turma da Pesada",
                [
                    new Diario ("diario:1_3", "turma:1", "professor:1", "disciplina:3", 1),
                    new Diario ("diario:1_1", "turma:1", "professor:1", "disciplina:1", 3),
                    new Diario ("diario:1_2", "turma:1", "professor:1", "disciplina:2", 2),
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
                    new Diario ("diario:2_1", "turma:2", "professor:2", "disciplina:4", 1),
                    new Diario ("diario:2_3", "turma:2", "professor:2", "disciplina:1", 3),
                    new Diario ("diario:2_2", "turma:2", "professor:2", "disciplina:2", 2),
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
        var horarioGeradoEnumerator = Gerador.GerarHorario(gerarHorarioOptions, verbose);
        // ====================================================

        var melhorHorario = horarioGeradoEnumerator.First();
        Console.WriteLine("============================");
        Console.WriteLine("Melhor horário gerado:");
        Console.WriteLine("");
        Console.WriteLine(melhorHorario);
        Console.WriteLine("============================");
        Console.WriteLine("");

        foreach (var turma in gerarHorarioOptions.Turmas)
        {
            Console.WriteLine($"Turma (Id={turma.Id}, Nome={turma.Nome ?? "Sem nome"})");
            var turmaAulas = from aula in melhorHorario.Aulas
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

                Console.WriteLine($"- Dia: {dia} | Intervalo: {horariosDeAula[aula.IntervaloDeTempo]} | {diario.DisciplinaId}");

            }
            Console.WriteLine();

        }

        // ====================================================

        if (gerarTodosOsHorarios)
        {
            foreach (var horarioGerado in horarioGeradoEnumerator)
            {
                Console.WriteLine($"Horário Gerado: {horarioGerado}");
            }
        }

        // ====================================================

        return true;
    }
}

