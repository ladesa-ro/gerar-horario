namespace Core;

public interface IDiario
{
  public IEntidadeIdentificacao Id { get; }

  //

  public IEntidadeIdentificacao TurmaId { get; }
  public IEntidadeIdentificacao ProfessorId { get; }

  public IEntidadeIdentificacao DisciplinaId { get; }


  //

  public int QuantidadeMaximaSemana { get; }
}
