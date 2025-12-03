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
            List<Publicacion> Publicaciones = new List<Publicacion>();
            try
            {
                Publicaciones = BD.devolverTodasLasPublicaciones();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error general: {ex.Message}");
                TempData["Error"] = "Ocurrió un error inesperado.";
            }

            ViewBag.Publicaciones = Publicaciones;
            return View("PaginaPrincipalComprador");
        }
        public IActionResult VerNotificaciones()
        {
            return View("Notificaciones");
        }
        public IActionResult VerNotificacionesV()
        {
            return View("NotificacionesV");
        }
        public IActionResult VerMensajes()
        {
            return View("Mensajes");
        }
        public IActionResult VerMensajesV()
        {
            return View("MensajesV");
        }
        public IActionResult VerUsuarioC()
        {
            string idString = HttpContext.Session.GetString("user");
            int id = -1;
            if (!string.IsNullOrEmpty(idString) && int.TryParse(idString, out id))
            {
                Usuario user = BD.devolverUsuario(id);
                if (user == null)
                {
                    HttpContext.Session.Clear();
                    return RedirectToAction("login", "OnBoarding");
                }
                else
                {
                    ViewBag.Usuario = user;
                    ViewBag.Favoritas = BD.devolverPublicacionesPorLista(BD.devolverIdListaUsuario(id));
                    return View("UsuarioComprador");
                }
            }
            else
            {
                HttpContext.Session.Clear();
                return RedirectToAction("login", "OnBoarding");
            }
        }
        public IActionResult VerPaginaPrincipalV()
        {
            string idString = HttpContext.Session.GetString("user");
            int id = 0;
            if (!string.IsNullOrEmpty(idString))
            {
                int.TryParse(idString, out id);
            }
            ViewBag.PublicacionesPropias = BD.devolverPublicacionesVendedor(id);
            ViewBag.Usuario = BD.devolverUsuario(id);
            return View("PaginaPrincipalVendedor");
        }
        public IActionResult VerLista(int idLista)
        {
            if (idLista <= 0)
            {
                TempData["Error"] = "ID de lista no válido.";
                return RedirectToAction("VerPaginaPrincipalC");
            }

            ViewBag.Publicaciones = BD.devolverPublicacionesPorLista(idLista);
            ViewBag.NombreLista = BD.devolverNombreLista(idLista);

            if (ViewBag.Publicaciones == null)
            {
                TempData["Info"] = "No se encontraron publicaciones en esa lista.";
            }

            return View("VerLista");
        }
        public IActionResult seleccionarEtiqueta(int idEtiqueta)
        {
            List<Publicacion> publicaciones = BD.DevolverPublicacionesEtiquetas(idEtiqueta);
            ViewBag.Publicaciones = publicaciones;
            return View("Buscador");
        }
        public IActionResult buscarProducto(string descripcion)
        {
            if (string.IsNullOrWhiteSpace(descripcion) || descripcion.Length > 100)
            {

                TempData["Error"] = "El término de búsqueda es inválido o demasiado largo.";
                return View("Buscador", new List<Publicacion>());
            }

            List<Publicacion> publicaciones = BD.DevolverPublicacionesBuscador(descripcion);
            ViewBag.Publicaciones = publicaciones;
            ViewBag.Buscado = descripcion;
            return View("Buscador");
        }
        public IActionResult agregarLista(int idPublicacion, int idLista)
        {
            BD.agregarLista(idPublicacion, idLista);
            string urlAnterior = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(urlAnterior))
            {
                return Redirect(urlAnterior);
            }

            return View("VerPaginaPrincipalC");
        }
        public IActionResult likear(int idPublicacion)
        {
            string idString = HttpContext.Session.GetString("user");
            int idUsuario = 0;
            if (string.IsNullOrEmpty(idString) || !int.TryParse(idString, out idUsuario) || idUsuario <= 0)
            {
                TempData["Info"] = "Debes iniciar sesión para gestionar Favoritos.";
                return RedirectToAction("login", "OnBoarding");
            }
            int idListaFavoritos = BD.devolverIdListaUsuario(idUsuario);
            if (idListaFavoritos == 0)
            {
                TempData["Error"] = "Error: No se encontró la lista de favoritos para tu usuario.";
                return RedirectToAction("VerPaginaPrincipalC");
            }
            if (BD.EstaEnLista(idPublicacion, idListaFavoritos))
            {
                BD.eliminarDeLista(idPublicacion, idListaFavoritos);
            }
            else
            {
                BD.agregarLista(idPublicacion, idListaFavoritos);
            }
            string urlAnterior = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(urlAnterior) && Url.IsLocalUrl(urlAnterior))
            {
                return Redirect(urlAnterior);
            }
            return RedirectToAction("VerPaginaPrincipalC");
        }
        public IActionResult verPublicacion(int idPublicacion)
        {
            return RedirectToAction("verPublicacion", "Post", new { idPublicacion = idPublicacion });
        }
        public IActionResult eliminarDeLista(int idPublicacion, int idLista)
        {
            BD.eliminarDeLista(idPublicacion, idLista);
            ViewBag.Publicaciones = BD.devolverPublicacionesPorLista(idLista);
            ViewBag.NombreLista = BD.devolverNombreLista(idLista);
            return View("VerLista");
        }
        public IActionResult DevolverEtiquetasPorPublicacion(int idPublicacion)
        {
            List<Etiqueta> etiquetas = BD.devolverEtiquetasPorPublicacion(idPublicacion);
            if (etiquetas == null || etiquetas.Count == 0)
            {
                return Json(new List<string>());
            }
            else
            {
                List<string> etiquetasBien = new List<string>();
                foreach (var etiqueta in etiquetas)
                {
                    etiquetasBien.Add("#" + etiqueta.Nombre);
                }
                return Json(etiquetasBien);
            }
        }
    }
}