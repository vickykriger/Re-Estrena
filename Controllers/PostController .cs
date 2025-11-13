using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ReEstrena.Models;
using Microsoft.Data.SqlClient;
using Dapper;
using Newtonsoft.Json;
namespace ReEstrena.Controllers
{
    public class PostController : Controller
    {
        public IActionResult subirPublicacion(string descripcion, string foto, decimal precio)
        {
            int id = Objeto.StringToObject<Usuario>(HttpContext.Session.GetString("user")).IdUsuario;
            Publicacion publicacion = new Publicacion(id, descripcion, foto, precio);
            ViewBag.subido = BD.subirPublicacion(publicacion);
            ViewBag.Publicacion = publicacion;
            return View("VerPublicacion");
        }
        public IActionResult eliminarPublicacion(int idProducto)
        {
            ViewBag.eliminado = BD.eliminarPublicacion(idProducto);
            return RedirectToAction("VerPaginaPrincipalV", "Account");
        }
        public IActionResult editarPublicacion(string descripcion, string foto, decimal precio)
        {
            int id = Objeto.StringToObject<Usuario>(HttpContext.Session.GetString("user")).IdUsuario;
            Publicacion publicacion = new Publicacion(id, descripcion, foto, precio);
            BD.editarPublicacion(publicacion);
            ViewBag.Publicacion = publicacion;
            return View("VerPublicacion");
        }
        public IActionResult verPublicacion(int idPublicacion)
        {
            ViewBag.Publicacion = BD.DevolverPublicacion(idPublicacion);
            return View("VerPublicacion");
        }
    }
}