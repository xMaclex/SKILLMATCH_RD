namespace SKILLMATCH_RD.Models;

public class Aptitud
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public string? Descripcion { get; set; }

    public List<Estudiante> Estudiantes { get; set; } = new();
    public List<Oferta> Ofertas { get; set; } = new();
}
