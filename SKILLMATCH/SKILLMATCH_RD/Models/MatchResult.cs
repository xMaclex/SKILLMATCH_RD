namespace SKILLMATCH_RD.Models;

public class MatchResult
{
    public Estudiante Estudiante { get; set; } = null!;
    public Oferta Oferta { get; set; } = null!;
    public double PorcentajeCompatibilidad { get; set; }
    public List<Aptitud> AptitudesCoincidentes { get; set; } = new();
    public List<Aptitud> AptitudesFaltantes { get; set; } = new();

    public string NivelMatch => PorcentajeCompatibilidad switch
    {
        >= 80 => "Excelente",
        >= 60 => "Bueno",
        >= 40 => "Regular",
        _     => "Bajo"
    };

    public string ClaseMatch => PorcentajeCompatibilidad switch
    {
        >= 80 => "success",
        >= 60 => "primary",
        >= 40 => "warning",
        _     => "danger"
    };
}
