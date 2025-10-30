using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ReEstrena.Models;
using Microsoft.Data.SqlClient;
using Dapper;
namespace ReEstrena.Controllers
{
    public class PostController : Controller
    {
        // Subir una publicación
        [HttpPost]
        public IActionResult SubirPublicacion(string descripcion, string etiquetas, decimal precio, IFormFile foto)
        {
            // Procesar la foto (opcional: guardar en disco/BD)
            string fotoUrl = null;
            if (foto != null && foto.Length > 0)
            {
                var fileName = Path.GetFileName(foto.FileName);
                var filePath = Path.Combine("wwwroot/images/", fileName);
               
                fotoUrl = "/images/" + fileName;
            }

            // Crear el objeto Publicacion
            Publicacion pub = new Publicacion
            {
                Descripcion = descripcion,
                Etiquetas = etiquetas,
                Precio = precio,
                FotoUrl = fotoUrl,
                // Agrega otros campos necesarios, como UsuarioId...
            };

            bool resultado = BD.SubirPublicacion(pub);

            if (resultado)
                return RedirectToAction("ExitoSubida");
            else
                return RedirectToAction("ErrorSubida");
        }

        // Eliminar una publicación
        [HttpPost]
        public IActionResult EliminarPublicacion(int id)
        {
            bool resultado = BD.EliminarPublicacion(id);

            if (resultado)
                return RedirectToAction("ExitoEliminacion");
            else
                return RedirectToAction("ErrorEliminacion");
        }

        // Editar una publicación
        [HttpPost]
        public IActionResult EditarPublicacion(int id, string descripcion, string etiquetas, decimal precio, IFormFile foto)
        {
            // Procesar foto si llega una nueva
            string fotoUrl = null;
            if (foto != null && foto.Length > 0)
            {
                var fileName = Path.GetFileName(foto.FileName);
                var filePath = Path.Combine("wwwroot/images/", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    foto.CopyTo(stream);
                }
                fotoUrl = "/images/" + fileName;
            }

            Publicacion pub = new Publicacion
            {
                Id = id,
                Descripcion = descripcion,
                Etiquetas = etiquetas,
                Precio = precio,
                FotoUrl = fotoUrl // Si no hay foto nueva, puedes mantener la anterior
            };

            bool resultado = BD.EditarPublicacion(pub);

            if (resultado)
                return RedirectToAction("ExitoEdicion");
            else
                return RedirectToAction("ErrorEdicion");
        }
    }
}