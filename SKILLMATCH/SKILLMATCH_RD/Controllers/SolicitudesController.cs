using Microsoft.AspNetCore.Mvc;

namespace SKILLMATCH_RD.Controllers;

public class SolicitudesController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Detalle(int id)
    {
        return View();
    }

    [HttpPost]
    public IActionResult Aplicar(int ofertaId, int estudianteId)
    {
        return RedirectToAction("Index", "Estudiantes");
    }

    [HttpPost]
    public IActionResult ActualizarEstado(int id, string estado)
    {
        return RedirectToAction("Dashboard", "Empresas");
    }
}
