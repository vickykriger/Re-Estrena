using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ReEstrena.Models;
using Microsoft.Data.SqlClient;
using Dapper;

namespace ReEstrena.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Email { get; set; }
        public string Contrasenia { get; set; }
        public string NombreUsuario { get; set; }
        public string NombreCompleto { get; set; }
        public string Pais { get; set; }
        public int Telefono { get; set; }

        public Usuario() { }

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
