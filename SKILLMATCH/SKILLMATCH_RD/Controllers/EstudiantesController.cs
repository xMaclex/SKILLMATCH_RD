using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SKILLMATCH_RD.Data;
using SKILLMATCH_RD.Models;

namespace SKILLMATCH_RD.Controllers;

// Avance 3: registro y listado de estudiantes persistidos en base de datos.
public class EstudiantesController : Controller
{
    private readonly AppDbContext _db;

    public EstudiantesController(AppDbContext db) => _db = db;

    // READ - listado de estudiantes registrados (CRUD)
    public IActionResult Index()
    {
        var estudiantes = _db.Estudiantes
            .Include(e => e.Universidad)
            .Include(e => e.Aptitudes)
            .OrderBy(e => e.Nombre)
            .ToList();

        return View(estudiantes);
    }

    // CREATE - formulario
    public IActionResult Registro()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Registro(Estudiante estudiante)
    {
        if (!ModelState.IsValid)
            return View(estudiante);

        // Asocia al estudiante con la universidad ITLA sembrada por defecto.
        var itla = _db.Universidades.OrderBy(u => u.Id).FirstOrDefault();
        if (itla != null)
            estudiante.UniversidadId = itla.Id;

        estudiante.FechaRegistro = DateTime.Now;
        _db.Estudiantes.Add(estudiante);
        _db.SaveChanges();

        HttpContext.Session.SetString("UsuarioNombre", estudiante.NombreCompleto);
        HttpContext.Session.SetString("UsuarioTipo",   "Estudiante");
        HttpContext.Session.SetString("UsuarioEmail",  estudiante.Email);

        TempData["Mensaje"] = "Perfil creado exitosamente.";
        return RedirectToAction("MisOfertas");
    }

    // READ - detalle de un estudiante
    public IActionResult Perfil(int id)
    {
        var estudiante = _db.Estudiantes
            .Include(e => e.Universidad)
            .Include(e => e.Aptitudes)
            .FirstOrDefault(e => e.Id == id);

        if (estudiante == null) return NotFound();
        return View(estudiante);
    }

    // DELETE
    public IActionResult Eliminar(int id)
    {
        var estudiante = _db.Estudiantes
            .Include(e => e.Universidad)
            .FirstOrDefault(e => e.Id == id);

        if (estudiante == null) return NotFound();
        return View(estudiante);
    }

    [HttpPost, ActionName("Eliminar")]
    [ValidateAntiForgeryToken]
    public IActionResult EliminarConfirmado(int id)
    {
        var estudiante = _db.Estudiantes.Find(id);
        if (estudiante != null)
        {
            _db.Estudiantes.Remove(estudiante);
            _db.SaveChanges();
            TempData["Mensaje"] = "Estudiante eliminado correctamente.";
        }
        return RedirectToAction("Index");
    }

    // Recomendaciones personalizadas: ofertas reales ordenadas por compatibilidad.
    public IActionResult MisOfertas()
    {
        var nombre = HttpContext.Session.GetString("UsuarioNombre") ?? "Estudiante";
        ViewBag.NombreEstudiante = nombre;

        var email = HttpContext.Session.GetString("UsuarioEmail");
        var estudiante = string.IsNullOrEmpty(email)
            ? null
            : _db.Estudiantes.Include(e => e.Aptitudes).FirstOrDefault(e => e.Email == email);

        var ofertas = _db.Ofertas
            .Include(o => o.Empresa)
            .Include(o => o.AptitudesRequeridas)
            .Where(o => o.Activa)
            .ToList();

        var recomendaciones = ofertas
            .Select(o => new
            {
                Titulo    = o.Titulo,
                Empresa   = o.Empresa?.Nombre ?? "Empresa",
                Tipo      = o.Tipo.ToString(),
                Ciudad    = o.Empresa?.Ciudad ?? "",
                Modalidad = o.Modalidad ?? "",
                Aptitudes = o.AptitudesRequeridas.Select(a => a.Nombre).ToArray(),
                Match     = CalcularMatch(estudiante, o)
            })
            .OrderByDescending(r => r.Match)
            .Take(4)
            .ToList<object>();

        ViewBag.Recomendaciones = recomendaciones;
        return View();
    }

    // Calculo simple de compatibilidad basado en aptitudes coincidentes.
    private static int CalcularMatch(Estudiante? estudiante, Oferta oferta)
    {
        if (estudiante == null || !oferta.AptitudesRequeridas.Any())
            return new Random(oferta.Id).Next(70, 93); // valor base cuando no hay perfil

        var requeridas = oferta.AptitudesRequeridas.Select(a => a.Nombre).ToHashSet();
        var coincidentes = estudiante.Aptitudes.Count(a => requeridas.Contains(a.Nombre));
        var porcentaje = 55 + (int)(45.0 * coincidentes / requeridas.Count);
        return Math.Min(porcentaje, 99);
    }

    public IActionResult MisSolicitudes()
    {
        var email = HttpContext.Session.GetString("UsuarioEmail");
        var solicitudes = _db.Solicitudes
            .Include(s => s.Oferta).ThenInclude(o => o!.Empresa)
            .Include(s => s.Estudiante)
            .Where(s => s.Estudiante!.Email == email)
            .ToList();

        return View(solicitudes);
    }
}
