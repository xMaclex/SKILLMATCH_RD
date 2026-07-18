using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SKILLMATCH_RD.Data;
using SKILLMATCH_RD.Models;

namespace SKILLMATCH_RD.Controllers;

// Avance 3: registro y listado de estudiantes persistidos en base de datos.
public class EstudiantesController : Controller
{
    private readonly AppDbContext _db;
    private readonly IWebHostEnvironment _env;

    public EstudiantesController(AppDbContext db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

    private static readonly string[] ExtensionesCv = { ".pdf", ".doc", ".docx" };
    private static readonly string[] ExtensionesPortafolio = { ".pdf", ".zip", ".png", ".jpg", ".jpeg" };
    private const long TamanoMaximoBytes = 5 * 1024 * 1024; // 5 MB

    // Guarda un archivo en wwwroot/uploads/{carpeta} y devuelve la ruta relativa (o null si no se subió nada).
    private string? GuardarArchivo(IFormFile? archivo, string carpeta, string[] extensionesPermitidas)
    {
        if (archivo == null || archivo.Length == 0)
            return null;

        var extension = Path.GetExtension(archivo.FileName).ToLowerInvariant();

        if (!extensionesPermitidas.Contains(extension))
            throw new InvalidOperationException($"Formato no permitido ({extension}). Formatos válidos: {string.Join(", ", extensionesPermitidas)}");

        if (archivo.Length > TamanoMaximoBytes)
            throw new InvalidOperationException("El archivo supera el tamaño máximo permitido (5 MB).");

        var carpetaDestino = Path.Combine(_env.WebRootPath, "uploads", carpeta);
        Directory.CreateDirectory(carpetaDestino);

        var nombreArchivo = $"{Guid.NewGuid()}{extension}";
        var rutaCompleta = Path.Combine(carpetaDestino, nombreArchivo);

        using (var stream = new FileStream(rutaCompleta, FileMode.Create))
        {
            archivo.CopyTo(stream);
        }

        return $"/uploads/{carpeta}/{nombreArchivo}";
    }

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
    public IActionResult Registro(Estudiante estudiante, IFormFile? cvArchivo, IFormFile? portafolioArchivo)
    {
        if (!ModelState.IsValid)
            return View(estudiante);

        try
        {
            estudiante.CvUrl = GuardarArchivo(cvArchivo, "cv", ExtensionesCv) ?? estudiante.CvUrl;
            estudiante.PortafolioUrl = GuardarArchivo(portafolioArchivo, "portafolio", ExtensionesPortafolio) ?? estudiante.PortafolioUrl;
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(estudiante);
        }

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

    // NUEVO: actualizar CV/Portafolio desde la página de Perfil
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ActualizarDocumentos(int id, IFormFile? cvArchivo, IFormFile? portafolioArchivo)
    {
        var estudiante = _db.Estudiantes.Find(id);
        if (estudiante == null) return NotFound();

        try
        {
            var nuevoCv = GuardarArchivo(cvArchivo, "cv", ExtensionesCv);
            var nuevoPortafolio = GuardarArchivo(portafolioArchivo, "portafolio", ExtensionesPortafolio);

            if (nuevoCv != null) estudiante.CvUrl = nuevoCv;
            if (nuevoPortafolio != null) estudiante.PortafolioUrl = nuevoPortafolio;

            _db.SaveChanges();
            TempData["Mensaje"] = "Documentos actualizados correctamente.";
        }
        catch (InvalidOperationException ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction("Perfil", new { id });
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
