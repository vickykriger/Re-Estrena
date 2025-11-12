using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ReEstrena.Models;
using Microsoft.Data.SqlClient;
using Dapper;
using Newtonsoft.Json;

namespace ReEstrena.Models
{
    public class UsuariosLista
    {
        public int IdUsuariosLista { get; private set; }
        public int IdLista { get; private set; }
        public int IdUsuario { get; private set; }

        public UsuariosLista() { }

        public UsuariosLista(int idLista, int idUsuario)
        {
            IdLista = idLista;
            IdUsuario = idUsuario;
        }
    }
}
