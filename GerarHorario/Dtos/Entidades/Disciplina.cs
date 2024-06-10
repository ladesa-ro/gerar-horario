using Sisgea.GerarHorario.Core.Dtos.Entidades;

public class Disciplina
{
    public string Id { get; set; }
    public string NomeDisciplina { get; set; }

    public int CargaHoraria {get; set; }

    public Disciplina(string id, string nomeDisciplina)
    {
        Id = id;
        NomeDisciplina = nomeDisciplina;
    }
}