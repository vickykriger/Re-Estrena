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
            return View("Buscador");
        }
        public IActionResult agregarLista(int idPublicacion, int idLista)
        {
            BD.agregarLista(idPublicacion, idLista);
            return View();
        }
        public IActionResult likear(int idPublicacion)
        {
            BD.agregarLista(idPublicacion, 1);
            return View();
        }
        public IActionResult verPublicacion(int idPublicacion)
        {
            ViewBag.Publicacion = BD.DevolverPublicacion(idPublicacion);
            return View("VerPublicacion");
        }
        public IActionResult editarUsuarioGuardar(string email, string contrasenia, string nombreUsuario, string nombreCompleto, string pais, int telefono, string foto)
        {
            int id = Objeto.StringToObject<Usuario>(HttpContext.Session.GetString("user")).id;
            BD.editarUsuario(id, email, contrasenia, nombreUsuario, nombreCompleto, pais, telefono, foto);
            return View("UsuarioComprador");
        }
    }
}