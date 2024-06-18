public class Data
{
    public int? diaSemanaIso {get; set;}
    public DateTime dataAnual { get; init; }

    public Data(DateTime dataAnual, int? diaSemanaIso)
    {
        this.dataAnual = dataAnual;
        this.diaSemanaIso = diaSemanaIso;
    }
}