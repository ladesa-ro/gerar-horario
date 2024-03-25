using Core.Dtos.Configuracoes;
using Core.Dtos.Entidades;

namespace Core;

public class Main
{

    public bool Retorno()
    {
        var gerarTodosOsHorarios = false;

        // ====================================================

        var gerarHorarioOptions = new GerarHorarioOptions
        {
            DiaSemanaInicio = (int)DiaSemanaIso.SEGUNDA,
            DiaSemanaFim = (int)DiaSemanaIso.SABADO,
        };

        // ====================================================
        var horarioGeradoEnumerator = Gerador.Gerador.GerarHorario(gerarHorarioOptions);
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

