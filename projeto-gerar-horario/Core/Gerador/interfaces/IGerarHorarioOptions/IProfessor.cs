namespace Core;

public interface IProfessor
{
  public IEntidadeIdentificacao Id { get; }
  public string? Nome { get; }
  public IDisponibilidadeDia[] Disponibilidades { get; }
}
