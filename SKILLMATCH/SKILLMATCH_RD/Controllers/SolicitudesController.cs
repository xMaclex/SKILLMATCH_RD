using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SKILLMATCH_RD.Data;
using SKILLMATCH_RD.Models;
using SKILLMATCH_RD.Models.Enums;

namespace SKILLMATCH_RD.Controllers;

// Avance 3: gestion completa de postulaciones persistidas en base de datos.
public class SolicitudesController : Controller
{
    private readonly AppDbContext _db;

    public SolicitudesController(AppDbContext db) => _db = db;

    // READ - listado de todas las postulaciones
    public IActionResult Index()
    {
        var solicitudes = _db.Solicitudes
            .Include(s => s.Estudiante)
            .Include(s => s.Oferta).ThenInclude(o => o!.Empresa)
            .OrderByDescending(s => s.FechaSolicitud)
            .ToList();

        return View(solicitudes);
    }

    // READ - detalle
    public IActionResult Detalle(int id)
    {
        var solicitud = _db.Solicitudes
            .Include(s => s.Estudiante)
            .Include(s => s.Oferta).ThenInclude(o => o!.Empresa)
            .FirstOrDefault(s => s.Id == id);

        if (solicitud == null) return NotFound();
        return View(solicitud);
    }

    // CREATE - aplicar a una oferta
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Aplicar(int ofertaId, int estudianteId)
    {
        var oferta = _db.Ofertas.Find(ofertaId);
        var estudiante = _db.Estudiantes.Find(estudianteId);

        if (oferta == null || estudiante == null)
            return RedirectToAction("Index", "Ofertas");

        var yaExiste = _db.Solicitudes.Any(s => s.OfertaId == ofertaId && s.EstudianteId == estudianteId);
        if (!yaExiste)
        {
            _db.Solicitudes.Add(new Solicitud
            {
                OfertaId       = ofertaId,
                EstudianteId   = estudianteId,
                Estado         = EstadoSolicitud.Pendiente,
                PorcentajeMatch = new Random(ofertaId * 100 + estudianteId).Next(60, 95),
                FechaSolicitud = DateTime.Now
            });
            _db.SaveChanges();
            TempData["Mensaje"] = "Postulacion enviada correctamente.";
        }
        else
        {
            TempData["Mensaje"] = "Ya habias postulado a esta oferta.";
        }

        return RedirectToAction("MisSolicitudes", "Estudiantes");
    }

    // UPDATE - cambiar estado de una postulacion
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ActualizarEstado(int id, string estado)
    {
        var solicitud = _db.Solicitudes.Find(id);
        if (solicitud != null && Enum.TryParse<EstadoSolicitud>(estado, out var nuevo))
        {
            solicitud.Estado = nuevo;
            _db.SaveChanges();
            TempData["Mensaje"] = "Estado de la postulacion actualizado.";
        }
        return RedirectToAction("Index");
    }

    // DELETE
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Eliminar(int id)
    {
        var solicitud = _db.Solicitudes.Find(id);
        if (solicitud != null)
        {
            _db.Solicitudes.Remove(solicitud);
            _db.SaveChanges();
            TempData["Mensaje"] = "Postulacion eliminada.";
        }
        return RedirectToAction("Index");
    }
}
