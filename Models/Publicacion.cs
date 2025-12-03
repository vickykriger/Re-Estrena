using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ReEstrena.Models;
using Microsoft.Data.SqlClient;
using Dapper;
using Newtonsoft.Json;

namespace ReEstrena.Models
{
    public class Publicacion
    {
        [JsonProperty]
        public int IdPublicacion { get; private set; }
        [JsonProperty]
        public int IdUsuario { get; private set; }
        [JsonProperty]
        public string Descripcion { get; private set; }
        [JsonProperty]
        public string Foto { get; private set; }
        [JsonProperty]
        public decimal Precio { get; private set; }
        [JsonProperty]
        public string NombreProducto { get; private set; }
        public Publicacion() { }

        public Publicacion(int idUsuario, string descripcion, string foto, decimal precio, string nombreProducto)
        {
            IdUsuario = idUsuario;
            Descripcion = descripcion;
            Foto = foto;
            Precio = precio;
            NombreProducto = nombreProducto;
        }
    }
}
