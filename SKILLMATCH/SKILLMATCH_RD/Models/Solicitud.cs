using SKILLMATCH_RD.Models.Enums;

namespace SKILLMATCH_RD.Models;

public class Solicitud
{
    public int Id { get; set; }
    public DateTime FechaSolicitud { get; set; } = DateTime.Now;
    public EstadoSolicitud Estado { get; set; } = EstadoSolicitud.Pendiente;
    public double PorcentajeMatch { get; set; }
    public string? Notas { get; set; }

    public int EstudianteId { get; set; }
    public Estudiante? Estudiante { get; set; }

    public int OfertaId { get; set; }
    public Oferta? Oferta { get; set; }

    public string EstadoTexto => Estado switch
    {
        EstadoSolicitud.Pendiente   => "Pendiente",
        EstadoSolicitud.EnRevision  => "En Revisión",
        EstadoSolicitud.Aceptada    => "Aceptada",
        EstadoSolicitud.Rechazada   => "Rechazada",
        _                           => "Desconocido"
    };
}
