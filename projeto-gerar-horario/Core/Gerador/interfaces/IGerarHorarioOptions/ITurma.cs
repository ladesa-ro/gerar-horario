namespace Core;

public interface ITurma
{
  public IEntidadeIdentificacao Id { get; }

  public string? Nome { get; }

  public IDiario[] DiariosDaTurma { get; }
  public IDisponibilidadeDia[] Disponibilidades { get; }
}
