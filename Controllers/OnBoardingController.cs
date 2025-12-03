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
        List<Publicacion> publicaciones = new List<Publicacion>();
        int i = 0;
        while (publicaciones.Count < 3)
        {
            i++;
            if (BD.DevolverPublicacion(i) != null)
            {
                publicaciones.Add(BD.DevolverPublicacion(i));
            }
        }
        ViewBag.Publicaciones = publicaciones;
        return View("LandingPage");
    }
    public IActionResult login()
    {
        return View("InicioSesion");
    }
    public IActionResult loginGuardar(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            ViewBag.MensajeError = "El nombre de usuario y la contraseña no pueden estar vacíos.";
            return View("InicioSesion");
        }
        else
        {
            Usuario user = BD.login(username, password);
            if (user == null)
            {
                ViewBag.MensajeError = "Credenciales incorrectas. Intenta de nuevo.";
                return View("InicioSesion");
            }
            else
            {
                HttpContext.Session.SetString("user", user.IdUsuario.ToString());
                List<Publicacion> carritoVacio = new List<Publicacion>();
                string jsonVacio = Objeto.ListToString<Publicacion>(carritoVacio);
                HttpContext.Session.SetString("carrito", jsonVacio);
                return RedirectToAction("VerPaginaPrincipalC", "Account");
            }
        }
    }
    public IActionResult registro()
    {
        return View("Registro");
    }
    public IActionResult registroGuardar(string Email, string Contrasenia, string Usuario, string NombreCompleto, int Telefono)
    {
        string errores = "";

        if (string.IsNullOrWhiteSpace(Email) || Email.Length > 100)
        {
            errores += "El email es requerido y no debe exceder 100 caracteres. ";
        }
        if (string.IsNullOrWhiteSpace(Contrasenia) || Contrasenia.Length < 8)
        {
            errores += "La contraseña es requerida y debe tener mínimo 8 caracteres. ";
        }
        if (string.IsNullOrWhiteSpace(Usuario) || Usuario.Length > 50)
        {
            errores += "El nombre de usuario es requerido y no debe exceder 50 caracteres. ";
        }
        if (string.IsNullOrWhiteSpace(NombreCompleto) || NombreCompleto.Length > 150)
        {
            errores += "El nombre completo es requerido y no debe exceder 150 caracteres. ";
        }
       
        if (Telefono <= 0)
        {
            errores += "El teléfono es requerido. ";
        }

        if (!string.IsNullOrEmpty(errores))
        {
            ViewBag.MensajeError = errores;
            return View("Registro");
        }
        Usuario user = new Usuario(Email, Contrasenia, Usuario, NombreCompleto, Telefono);
        bool registrado = BD.registrarse(user);
        if (registrado)
        {
            HttpContext.Session.SetString("user", user.IdUsuario.ToString());
            return RedirectToAction("VerPaginaPrincipalC", "Account");
        }
        else
        {
            ViewBag.MensajeError = "El email o nombre de usuario ya se encuentran registrados.";
            return View("Registro");
        }
    }
    public IActionResult cerrarSesion()
    {
        HttpContext.Session.Remove("user");
        return View("LandingPage");
    }
}
