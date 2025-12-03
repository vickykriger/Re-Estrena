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
            if (idProducto <= 0)
            {
                TempData["Error"] = "ID de producto inválido.";
                return RedirectToAction("VerCarrito");
            }

            string jsonCarrito = HttpContext.Session.GetString("carrito");
            List<Publicacion> carrito;

            if (string.IsNullOrEmpty(jsonCarrito))
            {
                carrito = new List<Publicacion>();
            }
            else
            {
                try
                {
                    carrito = Objeto.StringToList<Publicacion>(jsonCarrito);
                    if (carrito == null)
                    {
                        carrito = new List<Publicacion>();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error al deserializar el carrito: {ex.Message}");
                    TempData["Error"] = "Hubo un error al leer tu carrito. Se ha reiniciado.";
                    carrito = new List<Publicacion>();
                }
            }
            
            if (carrito.Any(p => p.IdPublicacion == idProducto))
            {
                TempData["Error"] = "¡Ese producto ya está en tu carrito de compras!";
                return RedirectToAction("VerCarrito");
            }

            Publicacion productoAAgregar = BD.DevolverPublicacion(idProducto);

            if (productoAAgregar != null)
            {
                carrito.Add(productoAAgregar);
                HttpContext.Session.SetString("carrito", Objeto.ListToString<Publicacion>(carrito));
                TempData["Info"] = "Producto agregado al carrito con éxito.";
            }
            else
            {
                TempData["Error"] = "El producto que intentas agregar no existe.";
            }

            return RedirectToAction("VerCarrito");
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