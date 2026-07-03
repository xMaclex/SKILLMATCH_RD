using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SKILLMATCH_RD.Data;
using SKILLMATCH_RD.Models;
using SKILLMATCH_RD.Models.Enums;

namespace SKILLMATCH_RD.Controllers;

// Avance 3: el dashboard y el registro de empresas trabajan contra la base de datos.
public class EmpresasController : Controller
{
    private readonly AppDbContext _db;

    public EmpresasController(AppDbContext db) => _db = db;

    // READ - listado de empresas registradas (CRUD)
    public IActionResult Index()
    {
        var empresas = _db.Empresas
            .Include(e => e.Ofertas)
            .OrderBy(e => e.Nombre)
            .ToList();

        return View(empresas);
    }

    // CREATE - formulario
    public IActionResult Registro()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Registro(Empresa empresa)
    {
        if (!ModelState.IsValid)
            return View(empresa);

        empresa.FechaRegistro = DateTime.Now;
        _db.Empresas.Add(empresa);
        _db.SaveChanges();

        HttpContext.Session.SetString("UsuarioNombre", empresa.Nombre);
        HttpContext.Session.SetString("UsuarioTipo",   "Empresa");
        HttpContext.Session.SetString("UsuarioEmail",  empresa.Email);

        TempData["Mensaje"] = "Empresa registrada exitosamente.";
        return RedirectToAction("Dashboard");
    }

    // Panel con estadisticas calculadas en tiempo real desde la base de datos.
    public IActionResult Dashboard()
    {
        var nombre = HttpContext.Session.GetString("UsuarioNombre") ?? "Mi Empresa";
        var empresa = _db.Empresas.FirstOrDefault(e => e.Nombre == nombre);

        IQueryable<Solicitud> solicitudes = _db.Solicitudes
            .Include(s => s.Estudiante)
            .Include(s => s.Oferta);

        IQueryable<Oferta> ofertas = _db.Ofertas;

        if (empresa != null)
        {
            ofertas = ofertas.Where(o => o.EmpresaId == empresa.Id);
            solicitudes = solicitudes.Where(s => s.Oferta!.EmpresaId == empresa.Id);
        }

        ViewBag.NombreEmpresa    = nombre;
        ViewBag.OfertasActivas   = ofertas.Count(o => o.Activa);
        ViewBag.SolicitudesTotal = solicitudes.Count();
        ViewBag.EnRevision       = solicitudes.Count(s => s.Estado == EstadoSolicitud.EnRevision);
        ViewBag.Colocaciones     = solicitudes.Count(s => s.Estado == EstadoSolicitud.Aceptada);

        ViewBag.Candidatos = solicitudes
            .OrderByDescending(s => s.PorcentajeMatch)
            .Select(s => new
            {
                Nombre   = s.Estudiante!.Nombre + " " + s.Estudiante.Apellido,
                Carrera  = s.Estudiante.Carrera,
                Semestre = s.Estudiante.SemestreActual,
                Match    = (int)s.PorcentajeMatch,
                Oferta   = s.Oferta!.Titulo
            })
            .Take(5)
            .ToList<object>();

        return View();
    }

    public IActionResult Candidatos(int ofertaId)
    {
        var candidatos = _db.Solicitudes
            .Include(s => s.Estudiante)
            .Include(s => s.Oferta)
            .Where(s => s.OfertaId == ofertaId)
            .OrderByDescending(s => s.PorcentajeMatch)
            .ToList();

        return View(candidatos);
    }
}
