using Microsoft.AspNetCore.Mvc;
using SKILLMATCH_RD.Models;

namespace SKILLMATCH_RD.Controllers;

public class OfertasController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Detalle(int id)
    {
        return View();
    }

    public IActionResult Crear()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Crear(Oferta oferta)
    {
        if (!ModelState.IsValid)
            return View(oferta);

        return RedirectToAction("Index");
    }
}
