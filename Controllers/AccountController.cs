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
            List<Publicacion> Publicaciones = BD.devolverTodasLasPublicaciones();
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
            string idString = HttpContext.Session.GetString("IdUsuario");
            int id = 0;
            if (!string.IsNullOrEmpty(idString))
            {
                int.TryParse(idString, out id);
            }
            ViewBag.Usuario = BD.devolverUsuario(id);
            ViewBag.Favoritas = BD.devolverPublicacionesPorLista(1);
            return View("UsuarioComprador");
        }
        public IActionResult VerPaginaPrincipalV()
        {
            string idString = HttpContext.Session.GetString("IdUsuario");
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
            ViewBag.Publicaciones = BD.devolverPublicacionesPorLista(idLista);
            ViewBag.NombreLista = BD.devolverNombreLista(idLista);
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
            BD.agregarLista(idPublicacion, 1);
            string urlAnterior = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(urlAnterior))
            {
                return Redirect(urlAnterior);
            }
            return View("VerPaginaPrincipalC");
        }
        public IActionResult verPublicacion(int idPublicacion)
        {
            return RedirectToAction("verPublicacion", "Post", new { idPublicacion = idPublicacion });
        }
        public IActionResult editarUsuarioGuardar(string email, string contrasenia, string usuario, string nombreCompleto, string pais, int telefono)
        {
            string idString = HttpContext.Session.GetString("IdUsuario");
            int id = 0;
            if (!string.IsNullOrEmpty(idString))
            {
                int.TryParse(idString, out id);
            }
            Usuario user = new Usuario(email, contrasenia, usuario, nombreCompleto, pais, telefono);
            BD.editarUsuario(user, id);
            return View("UsuarioComprador");
        }
        public IActionResult hacerLista(string NombreLista)
        {
            string idString = HttpContext.Session.GetString("IdUsuario");
            int id = 0;
            if (!string.IsNullOrEmpty(idString))
            {
                int.TryParse(idString, out id);
            }
            Lista lista = new Lista(id, NombreLista);
            BD.hacerLista(lista);
            string urlAnterior = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(urlAnterior))
            {
                return Redirect(urlAnterior);
            }

            return View("VerPaginaPrincipalC");
        }
        public IActionResult eliminarDeLista(int idPublicacion, int idLista)
        {
            BD.eliminarDeLista(idPublicacion, idLista);
            ViewBag.Publicaciones = BD.devolverPublicacionesPorLista(idLista);
            ViewBag.NombreLista = BD.devolverNombreLista(idLista);
            return View("VerLista");
        }
        public IActionResult eliminarLista(int idLista)
        {
            BD.eliminarLista(idLista);
            return View("UsuarioComprador");
        }
        public IActionResult DevolverEtiquetasPorPublicacion(int idPublicacion)
        {
            List<Etiqueta> etiquetas = BD.devolverEtiquetasPorPublicacion(idPublicacion);
            if (etiquetas == null || etiquetas.Count == 0)
            {
                return Json(new List<string>());
            }else
            {
                List<string> etiquetasBien = new List<string>();
                foreach(var etiqueta in etiquetas)
                {
                    etiquetasBien.Add("#" + etiqueta.Nombre);
                }
                return Json(etiquetasBien);
            }
        }
    }
}