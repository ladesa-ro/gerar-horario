using Core.Dtos.Entidades;

namespace Core.Dtos.Configuracoes;

public class GerarHorarioOptions
{

    public int DiaSemanaInicio { get; set; }
    public int DiaSemanaFim { get; set; }

    public Turma[] Turmas { get; set; }
    public Professor[] Professores { get; set; }
    public Intervalo[] IntervalosDeAula { get; set; }

    public override string ToString()
    {
        return "GerarHorarioOptions { nenhuma configuração }";
    }
}