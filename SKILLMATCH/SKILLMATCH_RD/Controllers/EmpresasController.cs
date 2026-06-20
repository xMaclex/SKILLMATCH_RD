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

        return RedirectToAction("Dashboard");
    }

    public IActionResult Dashboard()
    {
        return View();
    }

    public IActionResult Candidatos(int ofertaId)
    {
        return View();
    }
}
