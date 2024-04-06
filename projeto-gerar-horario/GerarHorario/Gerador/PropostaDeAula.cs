using Google.OrTools.Sat;

namespace Sisgea.GerarHorario.Core;

public class PropostaDeAula(GerarHorarioContext contexto,string turmaId,string diarioId,int diaSemanaIso,int intervaloIndex,BoolVar? modelBoolVar = null)
{

    public GerarHorarioContext Contexto { get; set; } = contexto;
    //
    public string TurmaId { get; set; } = turmaId;
    public string DiarioId { get; set; } = diarioId;
    //
    public int DiaSemanaIso { get; set; } = diaSemanaIso;
    public int IntervaloIndex { get; set; } = intervaloIndex;
    //

    private BoolVar? CreatedModelBoolVar { get; set; } = modelBoolVar;

    public BoolVar ModelBoolVar
    {
        get
        {
            if (this.CreatedModelBoolVar == null)
            {
                var propostaLabel = $"dia_{this.DiaSemanaIso}::intervalo_{this.IntervaloIndex}::diario_{this.DiarioId}";
                this.CreatedModelBoolVar = this.Contexto.Model.NewBoolVar(propostaLabel);
            }


            return this.CreatedModelBoolVar!;
        }

        set
        {
            this.CreatedModelBoolVar = value;
        }
    }
};
