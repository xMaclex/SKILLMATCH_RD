using Microsoft.AspNetCore.Mvc;
using SKILLMATCH_RD.Models;

namespace SKILLMATCH_RD.Controllers;

public class EstudiantesController : Controller
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
    public IActionResult Registro(Estudiante estudiante)
    {
        if (!ModelState.IsValid)
            return View(estudiante);

        return RedirectToAction("Perfil", new { id = 1 });
    }

    public IActionResult Perfil(int id)
    {
        return View();
    }

    public IActionResult MisOfertas()
    {
        return View();
    }

    public IActionResult MisSolicitudes()
    {
        return View();
    }
}
