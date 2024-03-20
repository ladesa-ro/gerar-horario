namespace Core;

public class HorarioGerado : IHorarioGerado
{

    public IHorarioGeradoAula[] Aulas { get; set; } = [];

    public override string ToString()
    {
        return "HorarioGerado {  }";
    }
}