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
            string idString = HttpContext.Session.GetString("IdUsuario");
            int idUsuario = 0;
            if (!string.IsNullOrEmpty(idString))
            {
                int.TryParse(idString, out idUsuario);
            }
            if (idUsuario <= 0)
            {
                return RedirectToAction("login", "OnBoarding");
            }
            Publicacion publicacion = new Publicacion(idUsuario, descripcion, foto, precio, nombreProducto);
            int idPub = BD.subirPublicacion(publicacion);
            if (idPub > 0)
            {
                ViewBag.Publicacion = publicacion;
                string[] etiquetasArray = BD.SepararPorEspacio(etiquetas);
                foreach (string etiqueta in etiquetasArray)
                {
                    int idEtiqueta = BD.agregarEtiqueta(etiqueta);
                    if (idEtiqueta > 0)
                    {
                        BD.agregarEtiquetaPublicacion(idPub, idEtiqueta);
                        
                    }
                    ViewBag.Etiquetas=BD.devolverEtiquetasPorPublicacion(idPub);
                }
                return View("EditarPublicacion");
            }
            else
            {
                ViewBag.ErrorMessage = "Error al guardar la publicación. Asegúrese de que todos los campos requeridos sean válidos.";
                return View("SubirPublicacion");
            }
        }
        public IActionResult subirPublicacion()
        {
            return View("SubirPublicacion");
        }
        public IActionResult eliminarPublicacion(int id)
        {
            ViewBag.eliminado = BD.eliminarPublicacion(id);
            return RedirectToAction("VerPaginaPrincipalV", "Account");
        }
        public IActionResult editarPublicacionGuardar(string nombreProducto, string descripcion, string foto, decimal precio, int idPub)
        {
            string idString = HttpContext.Session.GetString("IdUsuario");
            int id = 0;
            if (!string.IsNullOrEmpty(idString))
            {
                int.TryParse(idString, out id);
            }
            Publicacion publicacion = new Publicacion(id, descripcion, foto, precio, nombreProducto);
            BD.editarPublicacion(publicacion, idPub);
            ViewBag.Publicacion = BD.DevolverPublicacion(idPub);
            ViewBag.Etiquetas = BD.devolverEtiquetasPorPublicacion(idPub);
            return View("VerPublicacion");
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
            List<Etiqueta>lista = BD.devolverEtiquetasPorPublicacion(id);
            return Json(lista);
        }
    }
}