using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ReEstrena.Models;
using Microsoft.Data.SqlClient;
using Dapper;
using Newtonsoft.Json;

namespace ReEstrena.Controllers;

public class OnBoardingController : Controller
{
    private readonly ILogger<OnBoardingController> _logger;

    public OnBoardingController(ILogger<OnBoardingController> logger)
    {
        _logger = logger;
    }

    public IActionResult index()
    {
        return View("LandingPage");
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
            HttpContext.Session.SetString("user", Objeto.ObjectToString(user));
            return RedirectToAction("VerPaginaPrincipal", "Account");
        }
    }
    public IActionResult registro()
    {
        return View("Registro");
    }
    public IActionResult registroGuardar(string Email, string Contrasenia, string Usuario, string NombreCompleto, string Pais, int Telefono, string Foto)
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
