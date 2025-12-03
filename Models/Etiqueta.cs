using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ReEstrena.Models;
using Microsoft.Data.SqlClient;
using Dapper;
using Newtonsoft.Json;

namespace ReEstrena.Models
{
    public class Etiqueta
    {
        [JsonProperty]
        public int IdEtiqueta { get; private set; }
        [JsonProperty]
        public string Nombre { get; private set; }

        public Etiqueta() { }

        public Etiqueta(string nombre)
        {
            Nombre = nombre;
        }
    }
}
