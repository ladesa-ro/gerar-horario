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

    public override string ToString()
    {
        return $"[{HorarioInicio} - {HorarioFim}]";
    }

    public static bool VerificarIntervalo(Intervalo intervalo, TimeSpan horario){
        
        TimeSpan horarioInicio = TimeSpan.Parse(intervalo.HorarioInicio);
        TimeSpan horarioFim = TimeSpan.Parse(intervalo.HorarioFim);

        if ((horarioInicio <= horario) && (horario <= horarioFim)){
            return true;
        }
        else{
            return false;
        }
    }

    public static bool VerificarIntervalo(Intervalo intervalo, string horario){
        
      
        TimeSpan horarioConvertido = TimeSpan.Parse(horario);

        return VerificarIntervalo(intervalo, horarioConvertido);
    }

     public static bool VerificarIntervalo(Intervalo intervalo, Intervalo intervalo2){
          return VerificarIntervalo(intervalo, intervalo2.HorarioInicio) && VerificarIntervalo(intervalo, intervalo2.HorarioFim);
    }
}
