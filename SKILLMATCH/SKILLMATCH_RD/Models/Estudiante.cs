namespace SKILLMATCH_RD.Models;

public class Estudiante
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Matricula { get; set; } = string.Empty;
    public string Carrera { get; set; } = string.Empty;
    public int SemestreActual { get; set; }
    public bool BuscaPasantia { get; set; }
    public bool BuscaEmpleo { get; set; }
    public string? CvUrl { get; set; }
    public string? FotoUrl { get; set; }
    public string? Descripcion { get; set; }
    public DateTime FechaRegistro { get; set; } = DateTime.Now;

    public int UniversidadId { get; set; }
    public Universidad? Universidad { get; set; }

    public List<Aptitud> Aptitudes { get; set; } = new();
    public List<Solicitud> Solicitudes { get; set; } = new();

    public string NombreCompleto => $"{Nombre} {Apellido}";
}
