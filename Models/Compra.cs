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
        public int IdCompra { get; private set; }        
        public int IdVendedor { get; private set; }
        public int IdComprador { get; private set; }
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
