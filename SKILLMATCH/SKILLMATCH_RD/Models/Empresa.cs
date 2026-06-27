namespace SKILLMATCH_RD.Models;

public class Empresa
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Sector { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Ciudad { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? LogoUrl { get; set; }
    public string? SitioWeb { get; set; }
    public DateTime FechaRegistro { get; set; } = DateTime.Now;

    public List<Oferta> Ofertas { get; set; } = new();
}
