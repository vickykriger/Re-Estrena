using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ReEstrena.Models;
using Microsoft.Data.SqlClient;
using Dapper;

namespace ReEstrena.Models
{
    public class PublicacionLista
    {
        public int IdPublicacionLista { get; set; }
        public int IdPublicacion { get; set; }
        public int IdLista { get; set; }

        public PublicacionLista() { }

        public PublicacionLista(int idPublicacionLista, int idPublicacion, int idLista)
        {
            IdPublicacionLista = idPublicacionLista;
            IdPublicacion = idPublicacion;
            IdLista = idLista;
        }
    }
}
