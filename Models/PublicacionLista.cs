using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ReEstrena.Models;
using Microsoft.Data.SqlClient;
using Dapper;
using Newtonsoft.Json;

namespace ReEstrena.Models
{
    public class PublicacionLista
    {
        [JsonProperty]
        public int IdPublicacionLista { get; private set; }
        [JsonProperty]
        public int IdPublicacion { get; private set; }
        [JsonProperty]
        public int IdLista { get; private set; }

        public PublicacionLista() { }

        public PublicacionLista(int idPublicacion, int idLista)
        {
            IdPublicacion = idPublicacion;
            IdLista = idLista;
        }
    }
}
