using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ReEstrena.Models;
using Microsoft.Data.SqlClient;
using Dapper;

namespace ReEstrena.Controllers;

public class OnBoardingController : Controller
{
    private readonly ILogger<OnBoardingController> _logger;

    public OnBoardingController(ILogger<OnBoardingController> logger)
    {
        _logger = logger;
    }

    public IActionResult LandingPage()
    {
        return View();
    }
    public IActionResult login()
    {
        return View("InicioSesion");
    }
    public IActionResult loginGuardar(string username, string password)
    {
        Usuario user = BD.login(username, password);
        if(user==null)
        {
            return View("InicioSesion");
        }
        else 
        {
            BD.actualizarLogin(user.id);
            HttpContext.Session.SetString("user", Objeto.ObjectToString(user));
            return RedirectToAction("VerPaginaPrincipal", "Account");
        }
    }
    public IActionResult registro()
    {
        return View("Registro");
    }
    public IActionResult registroGuardar(string username, string password, string nombre, string apellido, string foto)
    {
        Usuario user = new Usuario(username, password, nombre, apellido, foto);
        bool registrado = BD.registrarse(user);
        if(registrado)
        {
            return View ("InicioSesion");
        }
        else
        {
            return View ("Registro");
        }
    }
    public IActionResult cerrarSesion()
    {
        HttpContext.Session.Remove("user");
        return View("LandingPage");
    }
}
