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
        [JsonProperty]
        public int IdUsuario { get; private set; }
        [JsonProperty]
        public string Email { get; private set; }
        [JsonProperty]
        public string Contrasenia { get; private set; }
        [JsonProperty]
        public string NombreUsuario { get; private set; }
        [JsonProperty]
        public string NombreCompleto { get; private set; }
        [JsonProperty]
        public string Pais { get; private set; }
        [JsonProperty]
        public int Telefono { get; private set; }
        [JsonProperty]
        public string Foto {get; private set; }
        [JsonProperty]
        public string Descripcion {get; private set;}

        public Usuario()
        {

        }

        public Usuario(string email, string contrasenia, string nombreUsuario, string nombreCompleto, string pais, int telefono)
        {
            Email = email;
            Contrasenia = contrasenia;
            NombreUsuario = nombreUsuario;
            NombreCompleto = nombreCompleto;
            Pais = pais;
            Telefono = telefono;
        }
    }
}
