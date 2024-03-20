using Google.OrTools.Bop;

namespace Core;

public class Class1
{

    public bool Retorno()
    {
        var gerador = new Gerador();
        return gerador.GerarHorario();
    }
}

