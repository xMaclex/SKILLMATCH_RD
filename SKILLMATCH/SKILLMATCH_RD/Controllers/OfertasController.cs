using Microsoft.AspNetCore.Mvc;
using SKILLMATCH_RD.Models;
using SKILLMATCH_RD.Models.Enums;

namespace SKILLMATCH_RD.Controllers;

public class OfertasController : Controller
{
    private static readonly List<Oferta> _ofertas = new()
    {
        new Oferta
        {
            Id = 1, Titulo = "Desarrollador .NET Junior",
            Descripcion = "Buscamos estudiante con conocimientos basicos de C# y SQL Server para unirse a nuestro equipo de desarrollo interno.",
            Tipo = TipoOferta.Pasantia, CarreraRequerida = "Desarrollo de Software",
            SemestreMinimo = 3, Modalidad = "Hibrido", Activa = true,
            FechaPublicacion = DateTime.Now.AddDays(-3),
            Empresa = new Empresa { Nombre = "TechCorp RD", Ciudad = "Santo Domingo" },
            AptitudesRequeridas = new List<Aptitud>
            {
                new() { Nombre = "C#" }, new() { Nombre = "SQL Server" }, new() { Nombre = "HTML/CSS" }
            }
        },
        new Oferta
        {
            Id = 2, Titulo = "Tecnico de Soporte IT",
            Descripcion = "Apoyo en soporte tecnico nivel 1 y 2, gestion de incidencias y mantenimiento de equipos.",
            Tipo = TipoOferta.Empleo, CarreraRequerida = "Redes y Telecomunicaciones",
            SemestreMinimo = 4, Modalidad = "Presencial", Activa = true,
            FechaPublicacion = DateTime.Now.AddDays(-7),
            Empresa = new Empresa { Nombre = "Banco Digital RD", Ciudad = "Santiago" },
            AptitudesRequeridas = new List<Aptitud>
            {
                new() { Nombre = "Windows Server" }, new() { Nombre = "Redes" }, new() { Nombre = "Active Directory" }
            }
        },
        new Oferta
        {
            Id = 3, Titulo = "Analista de Seguridad Informatica",
            Descripcion = "Monitoreo de sistemas, analisis de vulnerabilidades y apoyo en implementacion de politicas de seguridad.",
            Tipo = TipoOferta.Pasantia, CarreraRequerida = "Seguridad Informatica",
            SemestreMinimo = 5, Modalidad = "Remoto", Activa = true,
            FechaPublicacion = DateTime.Now.AddDays(-1),
            Empresa = new Empresa { Nombre = "CyberShield RD", Ciudad = "Santo Domingo" },
            AptitudesRequeridas = new List<Aptitud>
            {
                new() { Nombre = "Kali Linux" }, new() { Nombre = "Firewall" }, new() { Nombre = "SIEM" }
            }
        },
        new Oferta
        {
            Id = 4, Titulo = "Disenador UI/UX Junior",
            Descripcion = "Creacion de wireframes, prototipos y diseno de interfaces responsivas para aplicaciones web.",
            Tipo = TipoOferta.Empleo, CarreraRequerida = "Diseno Grafico",
            SemestreMinimo = 3, Modalidad = "Remoto", Activa = true,
            FechaPublicacion = DateTime.Now.AddDays(-5),
            Empresa = new Empresa { Nombre = "CreativaStudio", Ciudad = "Santo Domingo" },
            AptitudesRequeridas = new List<Aptitud>
            {
                new() { Nombre = "Figma" }, new() { Nombre = "Adobe XD" }, new() { Nombre = "CSS" }
            }
        },
        new Oferta
        {
            Id = 5, Titulo = "Administrador de Bases de Datos",
            Descripcion = "Gestion, optimizacion y respaldo de bases de datos SQL Server y PostgreSQL en ambiente productivo.",
            Tipo = TipoOferta.Empleo, CarreraRequerida = "Desarrollo de Software",
            SemestreMinimo = 6, Modalidad = "Presencial", Activa = true,
            FechaPublicacion = DateTime.Now.AddDays(-2),
            Empresa = new Empresa { Nombre = "DataCenter RD", Ciudad = "Santiago" },
            AptitudesRequeridas = new List<Aptitud>
            {
                new() { Nombre = "SQL Server" }, new() { Nombre = "PostgreSQL" }, new() { Nombre = "T-SQL" }
            }
        },
        new Oferta
        {
            Id = 6, Titulo = "Pasante de Soporte Tecnico",
            Descripcion = "Apoyo al departamento de IT en instalacion de software, configuracion de equipos y atencion a usuarios.",
            Tipo = TipoOferta.Pasantia, CarreraRequerida = "Soporte Tecnico",
            SemestreMinimo = 2, Modalidad = "Presencial", Activa = true,
            FechaPublicacion = DateTime.Now.AddDays(-10),
            Empresa = new Empresa { Nombre = "RetailMax RD", Ciudad = "San Pedro de Macoris" },
            AptitudesRequeridas = new List<Aptitud>
            {
                new() { Nombre = "Windows 11" }, new() { Nombre = "Office 365" }, new() { Nombre = "Comunicacion" }
            }
        }
    };

    public IActionResult Index(string? tipo = null, string? carrera = null)
    {
        var resultado = _ofertas.Where(o => o.Activa).ToList();

        if (!string.IsNullOrEmpty(tipo))
            resultado = resultado.Where(o => o.Tipo.ToString() == tipo).ToList();

        if (!string.IsNullOrEmpty(carrera))
            resultado = resultado.Where(o => o.CarreraRequerida == carrera).ToList();

        ViewBag.TipoFiltro    = tipo;
        ViewBag.CarreraFiltro = carrera;
        ViewBag.TotalOfertas  = _ofertas.Count(o => o.Activa);

        return View(resultado);
    }

    public IActionResult Detalle(int id)
    {
        var oferta = _ofertas.FirstOrDefault(o => o.Id == id);
        if (oferta == null) return NotFound();
        return View(oferta);
    }

    public IActionResult Crear()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Crear(Oferta oferta)
    {
        oferta.Id = _ofertas.Max(o => o.Id) + 1;
        oferta.FechaPublicacion = DateTime.Now;
        oferta.Activa = true;
        oferta.Empresa = new Empresa { Nombre = "Mi Empresa", Ciudad = "Santo Domingo" };
        _ofertas.Add(oferta);

        TempData["Mensaje"] = "Oferta publicada exitosamente.";
        return RedirectToAction("Index");
    }
}
