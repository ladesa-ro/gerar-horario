namespace Sisgea.GerarHorario.Core.Dtos.HorarioGerado;

public class HorarioGerado
{

    public HorarioGeradoAula[] Aulas { get; set; } = [];//?

    public override string ToString()
    {
        return $@"HorarioGerado {{
    Aulas ({Aulas.Length}) [
        
    ]
}}";
    }
}