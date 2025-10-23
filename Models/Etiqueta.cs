using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ReEstrena.Models;
using Microsoft.Data.SqlClient;
using Dapper;

namespace ReEstrena.Models
{
    public class Etiqueta
    {
        public int IdEtiqueta { get; set; }
        public string Nombre { get; set; }

        public Etiqueta() { }

        public Etiqueta(int idEtiqueta, string nombre)
        {
            IdEtiqueta = idEtiqueta;
            Nombre = nombre;
        }
    }
}
