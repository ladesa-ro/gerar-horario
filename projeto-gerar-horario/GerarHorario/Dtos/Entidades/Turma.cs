namespace Sisgea.GerarHorario.Core.Dtos.Entidades;

public record Turma
{
    public string Id { get; init; }
    public string? Nome { get; init; }

    public Diario[] DiariosDaTurma { get; init; }

    public DisponibilidadeDia[] Disponibilidades { get; set; }

    public Turma(string id, string? nome, Diario[] diariosDaTurma, DisponibilidadeDia[] disponibilidades)
    {
        Id = id;
        Nome = nome;
        DiariosDaTurma = diariosDaTurma;
        Disponibilidades = disponibilidades;
    }
}
