using Google.OrTools.Bop;

namespace Core;

public class Main
{

    public bool Retorno()
    {
        // ====================================================

        var gerarHorarioOptions = new GerarHorarioOptions
        {
            DiaSemanaInicio = (int)DiaSemanaIso.SEGUNDA,
            DiaSemanaFim = (int)DiaSemanaIso.SABADO
        };

        // ====================================================
        var horarioGeradoEnumerator = Gerador.GerarHorario(gerarHorarioOptions);
        // ====================================================

        var melhorHorario = horarioGeradoEnumerator.First();
        Console.WriteLine($"Melhor horário gerado: {melhorHorario}");

        // ====================================================

        foreach (var horarioGerado in horarioGeradoEnumerator)
        {
            Console.WriteLine($"Horário Gerado: {horarioGerado}");
        }

        // ====================================================

        return true;
    }
}

