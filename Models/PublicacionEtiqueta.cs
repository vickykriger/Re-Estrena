using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ReEstrena.Models;
using Microsoft.Data.SqlClient;
using Dapper;
using Newtonsoft.Json;

namespace ReEstrena.Models
{
    public class PublicacionEtiqueta
    {
        [JsonProperty]
        public int IdPublicacionEtiqueta { get; private set; }
        [JsonProperty]
        public int IdPublicacion { get; private set; }
        [JsonProperty]
        public int IdEtiqueta { get; private set; }

        public PublicacionEtiqueta() { }

        public PublicacionEtiqueta(int idPublicacion, int idEtiqueta)
        {
            IdPublicacion = idPublicacion;
            IdEtiqueta = idEtiqueta;
        }
    }
}
