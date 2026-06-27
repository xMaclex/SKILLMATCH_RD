using Microsoft.AspNetCore.Mvc;
using SKILLMATCH_RD.Models;

namespace SKILLMATCH_RD.Controllers;

public class EstudiantesController : Controller
{
    public IActionResult Index()
    {
        var nombre = HttpContext.Session.GetString("UsuarioNombre");
        if (nombre == null) return RedirectToAction("Login", "Auth");
        return View();
    }

    public IActionResult Registro()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Registro(Estudiante estudiante)
    {
        if (!ModelState.IsValid)
            return View(estudiante);

        HttpContext.Session.SetString("UsuarioNombre", estudiante.NombreCompleto);
        HttpContext.Session.SetString("UsuarioTipo",   "Estudiante");
        HttpContext.Session.SetString("UsuarioEmail",  estudiante.Email);

        TempData["Mensaje"] = "Perfil creado exitosamente.";
        return RedirectToAction("MisOfertas");
    }

    public IActionResult Perfil(int id)
    {
        return View();
    }

    public IActionResult MisOfertas()
    {
        var nombre = HttpContext.Session.GetString("UsuarioNombre") ?? "Estudiante";
        ViewBag.NombreEstudiante = nombre;

        ViewBag.Recomendaciones = new List<object>
        {
            new { Titulo = "Desarrollador .NET Junior",      Empresa = "TechCorp RD",       Tipo = "Pasantia",  Match = 92, Ciudad = "Santo Domingo", Modalidad = "Hibrido",    Aptitudes = new[] { "C#", "SQL Server", "HTML/CSS" } },
            new { Titulo = "Analista de Sistemas",           Empresa = "DataCenter RD",     Tipo = "Empleo",    Match = 85, Ciudad = "Santiago",       Modalidad = "Presencial", Aptitudes = new[] { "C#", "SQL", "POO" } },
            new { Titulo = "Pasante Backend .NET",           Empresa = "FinTech RD",        Tipo = "Pasantia",  Match = 79, Ciudad = "Santo Domingo", Modalidad = "Remoto",     Aptitudes = new[] { "ASP.NET", "EF Core", "Git" } },
            new { Titulo = "Desarrollador Full Stack",       Empresa = "CreativaStudio",    Tipo = "Empleo",    Match = 73, Ciudad = "Santo Domingo", Modalidad = "Hibrido",    Aptitudes = new[] { "JavaScript", "React", "C#" } }
        };

        return View();
    }

    public IActionResult MisSolicitudes()
    {
        return View();
    }
}
