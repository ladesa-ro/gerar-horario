using Google.OrTools.Sat;

namespace Sisgea.GerarHorario.Core;

public record PropostaAula(
    //
    string TurmaId,
    string DiarioId,
    //
    int DiaSemanaIso,
    int IntervaloIndex,
    //
    BoolVar ModelBoolVar
//
);
