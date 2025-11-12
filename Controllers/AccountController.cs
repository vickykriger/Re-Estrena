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
        public IActionResult VerNotificaciones()
        {
            return View("Notificaciones");
        }
        public IActionResult VerMensajes()
        {
            return View("Mensajes");
        }
        public IActionResult VerUsuarioC()
        {
            return View("UsuarioComprador");
        }
        public IActionResult VerPaginaPrincipalV()
        {
            return View("PaginaPrincipalVendedor");
        }
        public IActionResult VerLista(int idLista)
        {
            ViewBag.Publicaciones = BD.devolverPublicacionesPorLista(idLista);
            ViewBag.NombreLista = BD.devolverLista(idLista).NombreLista;
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
        public IActionResult hacerLista(string NombreLista)
        {
            int id = Objeto.StringToObject<Usuario>(HttpContext.Session.GetString("user")).id;
            Lista lista = new Lista(id, NombreLista);
            BD.hacerLista(lista);
            return View();
        }
        public IActionResult eliminarDeLista(int idPublicacion, int idLista)
        {
            BD.eliminarDeLista(idPublicacion, idLista);
            ViewBag.Publicaciones = BD.devolverPublicacionesPorLista(idLista);
            ViewBag.NombreLista = BD.devolverLista(idLista).NombreLista;
            return View("VerLista");
        }
        public IActionResult eliminarLista(int idLista)
        {
            BD.eliminarLista(idLista);
            return View("UsuarioComprador");
        }
    }
}