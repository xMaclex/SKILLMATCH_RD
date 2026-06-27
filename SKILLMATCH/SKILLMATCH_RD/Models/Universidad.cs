namespace SKILLMATCH_RD.Models;

public class Universidad
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public string Ciudad { get; set; } = string.Empty;
    public string? LogoUrl { get; set; }
    public string? SitioWeb { get; set; }

    public List<Estudiante> Estudiantes { get; set; } = new();
}
