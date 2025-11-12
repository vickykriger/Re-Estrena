using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ReEstrena.Models;
using Microsoft.Data.SqlClient;
using Dapper;
using Newtonsoft.Json;

namespace ReEstrena.Models
{
    public class Lista
    {
        public int IdLista { get; private set; }
        public int IdUsuario { get; private set; }
        public string NombreLista { get; private set; }

        public Lista() { }

        public Lista(int idUsuario, string nombreLista)
        {
            IdUsuario = idUsuario;
            NombreLista = nombreLista;
        }
    }
}
