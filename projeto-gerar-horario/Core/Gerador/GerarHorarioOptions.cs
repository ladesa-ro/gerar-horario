namespace Core;
public class GerarHorarioOptions : IGerarHorarioOptions
{

    public DiaSemanaIso DiaInicio { get; set; }
    public DiaSemanaIso DiaFim { get; set; }

    public override string ToString()
    {
        return "GerarHorarioOptions { nenhuma configuração }";
    }
}