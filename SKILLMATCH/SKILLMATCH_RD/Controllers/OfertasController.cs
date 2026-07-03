using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SKILLMATCH_RD.Data;
using SKILLMATCH_RD.Models;
using SKILLMATCH_RD.Models.Enums;

namespace SKILLMATCH_RD.Controllers;

// Avance 3: las ofertas ahora se leen y escriben en la base de datos SQLite
// mediante Entity Framework Core (CRUD completo), en lugar de la lista estatica
// en memoria que se usaba en el Avance 2.
public class OfertasController : Controller
{
    private readonly AppDbContext _db;

    public OfertasController(AppDbContext db) => _db = db;

    // READ - listado con filtros
    public IActionResult Index(string? tipo = null, string? carrera = null)
    {
        IQueryable<Oferta> query = _db.Ofertas
            .Include(o => o.Empresa)
            .Include(o => o.AptitudesRequeridas)
            .Where(o => o.Activa);

        if (!string.IsNullOrEmpty(tipo) && Enum.TryParse<TipoOferta>(tipo, out var t))
            query = query.Where(o => o.Tipo == t);

        if (!string.IsNullOrEmpty(carrera))
            query = query.Where(o => o.CarreraRequerida == carrera);

        var resultado = query.OrderByDescending(o => o.FechaPublicacion).ToList();

        ViewBag.TipoFiltro    = tipo;
        ViewBag.CarreraFiltro = carrera;
        ViewBag.TotalOfertas  = _db.Ofertas.Count(o => o.Activa);

        return View(resultado);
    }

    // READ - detalle
    public IActionResult Detalle(int id)
    {
        var oferta = _db.Ofertas
            .Include(o => o.Empresa)
            .Include(o => o.AptitudesRequeridas)
            .FirstOrDefault(o => o.Id == id);

        if (oferta == null) return NotFound();
        return View(oferta);
    }

    // CREATE
    public IActionResult Crear()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Crear(Oferta oferta)
    {
        oferta.FechaPublicacion = DateTime.Now;
        oferta.Activa = true;

        // Asocia la oferta a la empresa en sesion, o a la primera empresa disponible.
        var nombreEmpresa = HttpContext.Session.GetString("UsuarioNombre");
        var empresa = (!string.IsNullOrEmpty(nombreEmpresa)
                          ? _db.Empresas.FirstOrDefault(e => e.Nombre == nombreEmpresa)
                          : null)
                      ?? _db.Empresas.OrderBy(e => e.Id).FirstOrDefault();

        if (empresa != null)
            oferta.EmpresaId = empresa.Id;

        _db.Ofertas.Add(oferta);
        _db.SaveChanges();

        TempData["Mensaje"] = "Oferta publicada exitosamente.";
        return RedirectToAction("Index");
    }

    // UPDATE
    public IActionResult Editar(int id)
    {
        var oferta = _db.Ofertas.Find(id);
        if (oferta == null) return NotFound();
        return View(oferta);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Editar(Oferta oferta)
    {
        var actual = _db.Ofertas.Find(oferta.Id);
        if (actual == null) return NotFound();

        actual.Titulo           = oferta.Titulo;
        actual.Descripcion      = oferta.Descripcion;
        actual.Tipo             = oferta.Tipo;
        actual.CarreraRequerida = oferta.CarreraRequerida;
        actual.SemestreMinimo   = oferta.SemestreMinimo;
        actual.Modalidad        = oferta.Modalidad;
        actual.Salario          = oferta.Salario;
        actual.FechaVencimiento = oferta.FechaVencimiento;
        actual.Activa           = oferta.Activa;

        _db.SaveChanges();

        TempData["Mensaje"] = "Oferta actualizada correctamente.";
        return RedirectToAction("Index");
    }

    // DELETE
    public IActionResult Eliminar(int id)
    {
        var oferta = _db.Ofertas
            .Include(o => o.Empresa)
            .FirstOrDefault(o => o.Id == id);

        if (oferta == null) return NotFound();
        return View(oferta);
    }

    [HttpPost, ActionName("Eliminar")]
    [ValidateAntiForgeryToken]
    public IActionResult EliminarConfirmado(int id)
    {
        var oferta = _db.Ofertas.Find(id);
        if (oferta != null)
        {
            _db.Ofertas.Remove(oferta);
            _db.SaveChanges();
            TempData["Mensaje"] = "Oferta eliminada correctamente.";
        }
        return RedirectToAction("Index");
    }
}
