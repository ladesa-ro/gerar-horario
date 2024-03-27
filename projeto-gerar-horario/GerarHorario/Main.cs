using Sisgea.GerarHorario.Core.Dtos.Configuracoes;
using Sisgea.GerarHorario.Core.Dtos.Entidades;

namespace Sisgea.GerarHorario.Core;

public class Main
{

    public bool Retorno()
    {
        var gerarTodosOsHorarios = false;

        // ====================================================

        var turmas = new Turma[] {
            new(
                "turma:1",
                "Turma da Pesada",
                [
                    new Diario ("diario:1", "turma:1", "professor:1", "disciplina:1", 3),
                    new Diario ("diario:2", "turma:1", "professor:1", "disciplina:1", 2),
                ],
                [
                    new DisponibilidadeDia(1, new Intervalo("13:00", "17:30"))
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

        var gerarHorarioOptions = new GerarHorarioOptions((int)DiaSemanaIso.SEGUNDA, (int)DiaSemanaIso.SEGUNDA, turmas, professores, horariosDeAula);

        // ====================================================
        var horarioGeradoEnumerator = Gerador.GerarHorario(gerarHorarioOptions);
        // ====================================================

        var melhorHorario = horarioGeradoEnumerator.First();
        Console.WriteLine($"Melhor horário gerado: {melhorHorario}");

        foreach (var aula in melhorHorario.Aulas)
        {
            Console.WriteLine($"Aula - {aula.DiaDaSemanaIso}  - {aula.IntervaloDeTempo} - {aula.DiarioId}");
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

