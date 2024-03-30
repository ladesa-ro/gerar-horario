using Google.OrTools.ConstraintSolver;

namespace Sisgea.GerarHorario.Core.Dtos.Entidades;

public record Intervalo
{
    public string HorarioInicio { get; init; }
    public string HorarioFim { get; init; }

    public Intervalo(string horarioInicio, string horarioFim)
    {
        HorarioInicio = horarioInicio;
        HorarioFim = horarioFim;
    }

    public Intervalo(){
        
    }

    public override string ToString()
    {
        return $"[{HorarioInicio} - {HorarioFim}]";
    }

    public static bool VerificarIntervalo(Intervalo intervalo, TimeSpan horario){
        
        TimeSpan horarioInicio = TimeSpan.Parse(intervalo.HorarioInicio);
        TimeSpan horarioFim = TimeSpan.Parse(intervalo.HorarioFim);

        if ((horarioInicio <= horario) && (horario <= horarioFim)){
            System.Console.WriteLine("O horario "+horario+" esta dentro do intervalo " + intervalo.HorarioInicio + " - " + intervalo.HorarioFim);
            return true;
        }
        else{
            System.Console.WriteLine("O horario "+horario+" NAO esta dentro do intervalo " + intervalo.HorarioInicio + " - " + intervalo.HorarioFim);
            return false;
        }
    }

    public static bool VerificarIntervalo(Intervalo intervalo, string horario){
        
        TimeSpan horarioInicio = TimeSpan.Parse(intervalo.HorarioInicio);
        TimeSpan horarioFim = TimeSpan.Parse(intervalo.HorarioFim);
        TimeSpan horarioConvertido = TimeSpan.Parse(horario);

        return VerificarIntervalo(intervalo, horarioConvertido);
    }

     public static bool VerificarIntervalo(Intervalo intervalo, Intervalo intervalo2){
          return VerificarIntervalo(intervalo, intervalo2.HorarioInicio) && VerificarIntervalo(intervalo, intervalo2.HorarioFim);
    }
}
