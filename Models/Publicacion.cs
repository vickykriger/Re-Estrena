using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ReEstrena.Models;
using Microsoft.Data.SqlClient;
using Dapper;

namespace ReEstrena.Models
{
    public class Publicacion
    {
        public int IdPublicacion { get; set; }
        public int IdUsuario { get; set; }
        public string Descripcion { get; set; }
        public string Foto { get; set; }
        public decimal Precio { get; set; }

        public Publicacion() { }

        public Publicacion(int idPublicacion, int idUsuario, string descripcion, string foto, decimal precio)
        {
            IdPublicacion = idPublicacion;
            IdUsuario = idUsuario;
            Descripcion = descripcion;
            Foto = foto;
            Precio = precio;
        }
    }
}
