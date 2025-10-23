using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ReEstrena.Models;
using Microsoft.Data.SqlClient;
using Dapper;

namespace ReEstrena.Models
{
    public class Compra
    {
        public int IdCompra { get; set; }
        public int IdVendedor { get; set; }
        public int IdComprador { get; set; }
        public DateTime FechaCompra { get; set; }

        public Compra() { }

        public Compra(int idCompra, int idVendedor, int idComprador, DateTime fechaCompra)
        {
            IdCompra = idCompra;
            IdVendedor = idVendedor;
            IdComprador = idComprador;
            FechaCompra = fechaCompra;
        }
    }
}
