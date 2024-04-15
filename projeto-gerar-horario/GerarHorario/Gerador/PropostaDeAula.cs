using Google.OrTools.Sat;
using Sisgea.GerarHorario.Core.Dtos.Entidades;

namespace Sisgea.GerarHorario.Core;

public class PropostaDeAula(
    GerarHorarioContext contexto,
    string turmaId,
    string diarioId,
    string professorId,
    int diaSemanaIso,
    int intervaloIndex,
    Intervalo intervalo,
    BoolVar? modelBoolVar = null
)
{

    public GerarHorarioContext Contexto { get; set; } = contexto;
    //
    public string TurmaId { get; set; } = turmaId;
    //
    public string DiarioId { get; set; } = diarioId;
    //
    public string ProfessorId { get; set; } = professorId;
    //
    public int DiaSemanaIso { get; set; } = diaSemanaIso;
    //
    public int IntervaloIndex { get; set; } = intervaloIndex;

    public Intervalo Intervalo { get; set; } = intervalo;

    //
    private BoolVar? CreatedModelBoolVar { get; set; } = modelBoolVar;

    public BoolVar ModelBoolVar
    {
        get
        {
            if (this.CreatedModelBoolVar == null)
            {
                var propostaLabel = $"dia_{this.DiaSemanaIso}::intervalo_{this.IntervaloIndex}::diario_{this.DiarioId}::turma_{this.TurmaId}";
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
