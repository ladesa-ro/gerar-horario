namespace Core.Dtos.Entidades;

public class Turma
{
    public string Id { get; set; }
    public string? Nome { get; set; }
    public Diario[] DiariosDaTurma { get; set; }
    public DisponibilidadeDia[] Disponibilidades { get; set; }
}