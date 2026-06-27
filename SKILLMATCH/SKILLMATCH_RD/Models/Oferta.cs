using SKILLMATCH_RD.Models.Enums;

namespace SKILLMATCH_RD.Models;

public class Oferta
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public TipoOferta Tipo { get; set; }
    public string CarreraRequerida { get; set; } = string.Empty;
    public int SemestreMinimo { get; set; }
    public string? Modalidad { get; set; }
    public string? Salario { get; set; }
    public bool Activa { get; set; } = true;
    public DateTime FechaPublicacion { get; set; } = DateTime.Now;
    public DateTime? FechaVencimiento { get; set; }

    public int EmpresaId { get; set; }
    public Empresa? Empresa { get; set; }

    public List<Aptitud> AptitudesRequeridas { get; set; } = new();
    public List<Solicitud> Solicitudes { get; set; } = new();
}
