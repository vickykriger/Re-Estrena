using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ReEstrena.Models;
using Microsoft.Data.SqlClient;
using Dapper;

namespace ReEstrena.Models
{
    public class UsuariosLista
    {
        public int IdUsuariosLista { get; set; }
        public int IdLista { get; set; }
        public int IdUsuario { get; set; }

        public UsuariosLista() { }

        public UsuariosLista(int idUsuariosLista, int idLista, int idUsuario)
        {
            IdUsuariosLista = idUsuariosLista;
            IdLista = idLista;
            IdUsuario = idUsuario;
        }
    }
}
