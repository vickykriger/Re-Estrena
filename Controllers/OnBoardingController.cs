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
            HttpContext.Session.SetString("IdUsuario", user.IdUsuario.ToString());
            List<Publicacion> carritoVacio = new List<Publicacion>();
            string jsonVacio = Objeto.ListToString<Publicacion>(carritoVacio);
            HttpContext.Session.SetString("carrito", jsonVacio);
            return RedirectToAction("VerPaginaPrincipalC", "Account");
        }
    }
    public IActionResult registro()
    {
        return View("Registro");
    }
    public IActionResult registroGuardar(string Email, string Contrasenia, string Usuario, string NombreCompleto, string Pais, int Telefono)
    {
        Usuario user = new Usuario(Email, Contrasenia, Usuario, NombreCompleto, Pais, Telefono);
        bool registrado = BD.registrarse(user);
        if(registrado)
        {
            HttpContext.Session.SetString("user", Objeto.ObjectToString(user));
            return RedirectToAction("VerPaginaPrincipalC", "Account");
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
