using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ReEstrena.Models;
using Microsoft.Data.SqlClient;
using Dapper;
using Newtonsoft.Json;

namespace ReEstrena.Models
{
    public class Usuario
    {
        public int IdUsuario { get; private set; }
        public string Email { get; private set; }
        public string Contrasenia { get; private set; }
        public string NombreUsuario { get; private set; }
        public string NombreCompleto { get; private set; }
        public string Pais { get; private set; }
        public int Telefono { get; private set; }
        public string Foto {get; private set; }

        public Usuario()
        {

        }

        public Usuario(int idUsuario, string email, string contrasenia, string nombreUsuario, string nombreCompleto, string pais, int telefono)
        {
            IdUsuario = idUsuario;
            Email = email;
            Contrasenia = contrasenia;
            NombreUsuario = nombreUsuario;
            NombreCompleto = nombreCompleto;
            Pais = pais;
            Telefono = telefono;
        }
    }
}
