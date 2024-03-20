namespace Core;

public class EntidadeIdentificacao : IEntidadeIdentificacao
{
  private string uniqueTag;

  public string UniqueTag()
  {
    return uniqueTag;
  }

  public EntidadeIdentificacao(string uniqueTag)
  {
    this.uniqueTag = uniqueTag;
  }

  public static EntidadeIdentificacao Id(int id)
  {
    return new EntidadeIdentificacao($"id_{id}");
  }


  public static EntidadeIdentificacao Uuid(int id)
  {
    return new EntidadeIdentificacao($"uuid_{id}");
  }


  public static bool operator ==(EntidadeIdentificacao b1, EntidadeIdentificacao b2)
  {
    if (b1 is null)
    {
      return b2 is null;
    }

    return b1.Equals(b2);
  }

  public static bool operator !=(EntidadeIdentificacao b1, EntidadeIdentificacao b2)
  {
    return !(b1 == b2);
  }

  public override bool Equals(object? obj)
  {
    if (obj == null)
    {
      return false;
    }

    return obj is EntidadeIdentificacao b2 ? (b2.UniqueTag() == uniqueTag) : false;
  }

  public override int GetHashCode()
  {
    return uniqueTag.GetHashCode();
  }
}
