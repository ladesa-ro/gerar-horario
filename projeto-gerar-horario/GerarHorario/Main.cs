using Sisgea.GerarHorario.Core.Dtos.Configuracoes;
using Sisgea.GerarHorario.Core.Dtos.Entidades;

namespace Sisgea.GerarHorario.Core;

public class Main
{

    public bool Retorno()
    {
        var gerarTodosOsHorarios = false;

        // ====================================================

        var gerarHorarioOptions = new GerarHorarioOptions((int)DiaSemanaIso.SEGUNDA, (int)DiaSemanaIso.SABADO, [], [], []);

        // ====================================================
        var horarioGeradoEnumerator = Gerador.GerarHorario(gerarHorarioOptions);
        // ====================================================

        var melhorHorario = horarioGeradoEnumerator.First();
        Console.WriteLine($"Melhor horário gerado: {melhorHorario}");


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

