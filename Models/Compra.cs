using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ReEstrena.Models;
using Microsoft.Data.SqlClient;
using Dapper;
using Newtonsoft.Json;

namespace ReEstrena.Models
{
    public class Compra
    {
        [JsonProperty]
        public int IdCompra { get; private set; }        
        [JsonProperty]
        public int IdVendedor { get; private set; }
        [JsonProperty]
        public int IdComprador { get; private set; }
        [JsonProperty]
        public DateTime FechaCompra { get; private set; }

        public Compra() { }

        public Compra(int idVendedor, int idComprador, DateTime fechaCompra)
        {
            IdVendedor = idVendedor;
            IdComprador = idComprador;
            FechaCompra = fechaCompra;
        }
    }
}
