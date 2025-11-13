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
        // Mostrar el carrito
        public IActionResult VerCarrito()
        {
            var carrito = ObtenerCarritoDeSesion();
            ViewBag.Carrito = carrito;
            ViewBag.Total = CalcularTotal(carrito);
            return View("VerCarrito");
        }

        // Agregar una publicación al carrito 
        [HttpPost]
        public IActionResult AgregarACarrito(int idPublicacion)
        {
            var carrito = ObtenerCarritoDeSesion();

            // Simulación: busca la publicación por id (modifica por tu acceso real a BD)
            Publicacion publicacion = BD.DevolverPublicacion(idPublicacion);
            if (publicacion != null)
            {
                carrito.Add(publicacion);
                GuardarCarritoEnSesion(carrito);            
            }
            return RedirectToAction("VerCarrito");
        }

        // Eliminar una publicación del carrito
        [HttpPost]
        public IActionResult EliminarDeCarrito(int id)
        {
            var carrito = ObtenerCarritoDeSesion();
            carrito.RemoveAll(p => p.IdPublicacion == id);
            GuardarCarritoEnSesion(carrito);

            return RedirectToAction("VerCarrito");
        }

        // Métodos auxiliares para manejar el carrito en sesión
        private List<Publicacion> ObtenerCarritoDeSesion()
        {
            var carritoJson = HttpContext.Session.GetString("carrito");
            if (string.IsNullOrEmpty(carritoJson))
                return new List<Publicacion>();
            return JsonConvert.DeserializeObject<List<Publicacion>>(carritoJson);
        }

        private void GuardarCarritoEnSesion(List<Publicacion> carrito)
        {
            var carritoJson = JsonConvert.SerializeObject(carrito);
            HttpContext.Session.SetString("carrito", carritoJson);
        }

        private decimal CalcularTotal(List<Publicacion> carrito)
        {
            decimal total = 0;
            foreach (var pub in carrito)
                total += pub.Precio;
            // Ejemplo: agregar costo de envío fijo
            total += 11;
            return total;
        }
    }
}