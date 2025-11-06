using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ReEstrena.Models;
using Microsoft.Data.SqlClient;
using Dapper;
using Newtonsoft.Json;
namespace ReEstrena.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult VerPaginaPrincipalC()
        {
            return View("PaginaPrincipalComprador");
        }
        public IActionResult VerNotificacionesC()
        {
            return View("NotificacionesComprador");
        }
        public IActionResult VerMensajesC()
        {
            return View("MensajesComprador");
        }
        public IActionResult VerUsuarioC()
        {
            return View("UsuarioComprador");
        }
        public IActionResult VerPaginaPrincipalV()
        {
            return View("PaginaPrincipalVendedor");
        }
        public IActionResult VerNotificacionesV()
        {
            return View("NotificacionesVendedor");
        }
        public IActionResult VerMensajesV()
        {
            return View("MensajesVendedor");
        }
    }
}