using Microsoft.AspNetCore.Mvc;
using SKILLMATCH_RD.Models;

namespace SKILLMATCH_RD.Controllers;

public class EmpresasController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Registro()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Registro(Empresa empresa)
    {
        if (!ModelState.IsValid)
            return View(empresa);

        HttpContext.Session.SetString("UsuarioNombre", empresa.Nombre);
        HttpContext.Session.SetString("UsuarioTipo",   "Empresa");
        HttpContext.Session.SetString("UsuarioEmail",  empresa.Email);

        TempData["Mensaje"] = "Empresa registrada exitosamente.";
        return RedirectToAction("Dashboard");
    }

    public IActionResult Dashboard()
    {
        var nombre = HttpContext.Session.GetString("UsuarioNombre") ?? "Mi Empresa";

        ViewBag.NombreEmpresa    = nombre;
        ViewBag.OfertasActivas   = 3;
        ViewBag.SolicitudesTotal = 12;
        ViewBag.EnRevision       = 5;
        ViewBag.Colocaciones     = 2;

        ViewBag.Candidatos = new List<object>
        {
            new { Nombre = "Carlos Mejia",   Carrera = "Desarrollo de Software", Semestre = 6, Match = 92, Oferta = "Desarrollador .NET Junior" },
            new { Nombre = "Maria Torres",   Carrera = "Desarrollo de Software", Semestre = 5, Match = 87, Oferta = "Desarrollador .NET Junior" },
            new { Nombre = "Jose Ramirez",   Carrera = "Redes y Telecomunicaciones", Semestre = 7, Match = 81, Oferta = "Tecnico de Soporte IT" },
            new { Nombre = "Ana Rodriguez",  Carrera = "Seguridad Informatica",  Semestre = 6, Match = 78, Oferta = "Analista de Seguridad" },
            new { Nombre = "Luis Fernandez", Carrera = "Desarrollo de Software", Semestre = 4, Match = 74, Oferta = "Desarrollador .NET Junior" }
        };

        return View();
    }

    public IActionResult Candidatos(int ofertaId)
    {
        return View();
    }
}
