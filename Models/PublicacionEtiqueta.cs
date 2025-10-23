using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ReEstrena.Models;
using Microsoft.Data.SqlClient;
using Dapper;

namespace ReEstrena.Models
{
    public class PublicacionEtiqueta
    {
        public int IdPublicacionEtiqueta { get; set; }
        public int IdPublicacion { get; set; }
        public int IdEtiqueta { get; set; }

        public PublicacionEtiqueta() { }

        public PublicacionEtiqueta(int idPublicacionEtiqueta, int idPublicacion, int idEtiqueta)
        {
            IdPublicacionEtiqueta = idPublicacionEtiqueta;
            IdPublicacion = idPublicacion;
            IdEtiqueta = idEtiqueta;
        }
    }
}
