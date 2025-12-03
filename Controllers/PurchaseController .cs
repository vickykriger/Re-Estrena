using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ReEstrena.Models;
using Microsoft.Data.SqlClient;
using Dapper;
using Newtonsoft.Json;

namespace ReEstrena.Controllers
{
    public class PurchaseController : Controller
    {
        public IActionResult VerCarrito()
        {
            string jsonCarrito = HttpContext.Session.GetString("carrito");
            List<Publicacion> carrito;
            if (string.IsNullOrEmpty(jsonCarrito))
            {
                carrito = new List<Publicacion>();
            }
            else
            {
                carrito = Objeto.StringToList<Publicacion>(jsonCarrito);
            }
            ViewBag.Carrito = carrito;
            return View("CarritoCompras");
        }

        public IActionResult AgregarAlCarrito(int idProducto)
        {
            string jsonCarrito = HttpContext.Session.GetString("carrito");
            List<Publicacion> carrito = Objeto.StringToList<Publicacion>(jsonCarrito);
            if(BD.DevolverPublicacion(idProducto) != null)
            {
                carrito.Add(BD.DevolverPublicacion(idProducto));
            }
            HttpContext.Session.SetString("carrito", Objeto.ListToString<Publicacion>(carrito));
            return RedirectToAction("VerPublicacion", "Post", new { idPublicacion = idProducto });
        }

        public IActionResult EliminarDelCarrito(int idPublicacion)
        {
            var jsonCarrito = HttpContext.Session.GetString("carrito");
            List<Publicacion> carrito;
            if (string.IsNullOrEmpty(jsonCarrito))
            {
                carrito = new List<Publicacion>();
            }
            else
            {
                carrito = Objeto.StringToList<Publicacion>(jsonCarrito);
            }
            int indexToRemove = carrito.FindIndex(p => p.IdPublicacion == idPublicacion);
            if (indexToRemove >= 0)
            {
                carrito.RemoveAt(indexToRemove);
            }
            HttpContext.Session.SetString("carrito", Objeto.ListToString<Publicacion>(carrito));
            return RedirectToAction("VerCarrito");
        }
    }
}