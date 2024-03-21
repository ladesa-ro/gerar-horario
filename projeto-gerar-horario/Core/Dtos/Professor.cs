namespace Core;
public class Professor
{
    public string Id { get; set; }
    public string? Nome { get; set; }
    public DisponibilidadeDia[] Disponibilidades { get; set; }
}