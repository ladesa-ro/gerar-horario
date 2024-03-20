using Google.OrTools.Bop;

namespace Core;

public class Main
{

    public bool Retorno()
    {
        var gerador = new Gerador();

        var gerarHorarioOptions = new GerarHorarioOptions { };

        return gerador.GerarHorario(gerarHorarioOptions);
    }
}

