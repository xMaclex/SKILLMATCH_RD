using Microsoft.AspNetCore.Mvc;

namespace SKILLMATCH_RD.Controllers;

public class AuthController : Controller
{
    public IActionResult Login(string? returnUrl = null)
    {
        if (HttpContext.Session.GetString("UsuarioNombre") != null)
            return RedirectToAction("Index", "Home");

        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    [HttpPost]
    public IActionResult Login(string email, string password, string tipo, string? returnUrl = null)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            ViewBag.Error = "Por favor completa todos los campos.";
            return View();
        }

        var nombre = email.Contains('@')
            ? email.Split('@')[0]
            : email;

        nombre = char.ToUpper(nombre[0]) + nombre[1..].Replace(".", " ");

        HttpContext.Session.SetString("UsuarioEmail",  email);
        HttpContext.Session.SetString("UsuarioTipo",   tipo ?? "Estudiante");
        HttpContext.Session.SetString("UsuarioNombre", nombre);

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return tipo == "Empresa"
            ? RedirectToAction("Dashboard", "Empresas")
            : RedirectToAction("MisOfertas", "Estudiantes");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
}
