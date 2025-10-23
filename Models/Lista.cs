using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ReEstrena.Models;
using Microsoft.Data.SqlClient;
using Dapper;

namespace ReEstrena.Models
{
    public class Lista
    {
        public int IdLista { get; set; }
        public int IdUsuario { get; set; }
        public string NombreLista { get; set; }

        public Lista() { }

        public Lista(int idLista, int idUsuario, string nombreLista)
        {
            IdLista = idLista;
            IdUsuario = idUsuario;
            NombreLista = nombreLista;
        }
    }
}
