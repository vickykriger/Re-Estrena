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
        public IActionResult subirPublicacionGuardar(string nombreProducto, string descripcion, string foto, string etiquetas, decimal precio)
        {
            string errores = "";
            string idString = HttpContext.Session.GetString("user");
            int idUsuario = 0;

            if (string.IsNullOrEmpty(idString) || !int.TryParse(idString, out idUsuario) || idUsuario <= 0)
            {
                return RedirectToAction("login", "OnBoarding");
            }

            if (string.IsNullOrWhiteSpace(nombreProducto)) errores += "El nombre del producto es obligatorio. ";
            if (string.IsNullOrWhiteSpace(descripcion)) errores += "La descripción es obligatoria. ";
            if (string.IsNullOrWhiteSpace(foto)) errores += "La foto (ruta/URL) es obligatoria. ";
            if (precio <= 0) errores += "El precio debe ser mayor a cero. ";

            if (!string.IsNullOrEmpty(errores))
            {
                ViewBag.ErrorMessage = errores;
                return View("SubirPublicacion");
            }

            Publicacion publicacion = new Publicacion(idUsuario, descripcion, foto, precio, nombreProducto);

            int idPub = BD.subirPublicacion(publicacion);

            if (idPub > 0)
            {
                if (!string.IsNullOrWhiteSpace(etiquetas))
                {
                    string[] etiquetasArray = BD.SepararPorEspacio(etiquetas);
                    foreach (string etiqueta in etiquetasArray)
                    {
                        if (!string.IsNullOrWhiteSpace(etiqueta))
                        {
                            int idEtiqueta = BD.agregarEtiqueta(etiqueta);
                            if (idEtiqueta > 0)
                            {
                                BD.agregarEtiquetaPublicacion(idPub, idEtiqueta);
                            }
                        }
                    }
                }

                return RedirectToAction("VerPaginaPrincipalV", "Account");
            }
            else
            {
                ViewBag.ErrorMessage = "Error al guardar la publicación en la base de datos.";
                return View("SubirPublicacion");
            }
        }
        public IActionResult subirPublicacion()
        {
            return View("SubirPublicacion");
        }
        [HttpPost]
        public IActionResult eliminarPublicacion(int id)
        {
            bool eliminado = BD.eliminarPublicacion(id);
            return RedirectToAction("VerPaginaPrincipalV", "Account");
        }
        public IActionResult editarPublicacionGuardar(string nombreProducto, string descripcion, string foto, decimal precio, int idPub)
        {
            string idString = HttpContext.Session.GetString("user");
            int id = 0;

            if (string.IsNullOrEmpty(idString) || !int.TryParse(idString, out id) || id <= 0)
            {
                TempData["Error"] = "Sesión inválida. Inicia sesión de nuevo.";
                return RedirectToAction("login", "OnBoarding");
            }

            Publicacion publicacionExistente = BD.DevolverPublicacion(idPub);

            if (publicacionExistente == null || publicacionExistente.IdUsuario != id)
            {
                TempData["Error"] = "Acceso denegado: No puedes editar esta publicación.";
                return RedirectToAction("VerPaginaPrincipalV", "Account");
            }

            if (string.IsNullOrWhiteSpace(nombreProducto) || precio <= 0)
            {
                TempData["Error"] = "El nombre y el precio son obligatorios.";
                return RedirectToAction("editarPublicacion", new { id = idPub });
            }

            Publicacion publicacion = new Publicacion(id, descripcion, foto, precio, nombreProducto);

            BD.editarPublicacion(publicacion, idPub);

            TempData["SuccessMessage"] = "¡Publicación editada con éxito!";
            return RedirectToAction("VerPaginaPrincipalV", "Account");
        }
        public IActionResult verPublicacion(int idPublicacion)
        {
            ViewBag.Publicacion = BD.DevolverPublicacion(idPublicacion);
            ViewBag.Etiquetas = BD.devolverEtiquetasPorPublicacion(idPublicacion);
            return View("VerPublicacion");
        }
        public IActionResult editarPublicacion(int id)
        {
            ViewBag.Publicacion = BD.DevolverPublicacion(id);
            ViewBag.Etiquetas = BD.devolverEtiquetasPorPublicacion(id);
            return View("EditarPublicacion");
        }
        [HttpGet]
        public IActionResult EtiquetasPorPublicacion(int id)
        {
            List<Etiqueta> lista = BD.devolverEtiquetasPorPublicacion(id);
            return Json(lista);
        }
    }
}