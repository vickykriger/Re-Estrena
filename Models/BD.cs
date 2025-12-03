namespace ReEstrena.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Dapper;
using System.Collections.Generic;
using System.Linq;

public static class BD
{
    private static string _connectionString = @"Server=localhost;
    DataBase=BD;Integrated Security =True;TrustServerCertificate=True;";

    public static bool registrarse(Usuario user)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string checkQuery = "SELECT * FROM Usuarios WHERE NombreUsuario = @pUsuario";
            Usuario validar = connection.QueryFirstOrDefault<Usuario>(
                checkQuery,
                new { pUsuario = user.NombreUsuario }
            );

            if (validar != null)
            {
                return false;
            }
        }
        int nuevoIdUsuario;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string insertUser = @"
                INSERT INTO Usuarios (Email, Contrasenia, NombreUsuario, NombreCompleto, Telefono, Foto, Descripcion)
                OUTPUT INSERTED.IdUsuario
                VALUES (@pEmail, @pContrasenia, @pUsuario, @pNombreCompleto, @pTelefono, @pFoto, @pDescripcion);
            ";

            nuevoIdUsuario = connection.QuerySingle<int>(
                insertUser,
                new
                {
                    pEmail = user.Email,
                    pContrasenia = user.Contrasenia,
                    pUsuario = user.NombreUsuario,
                    pNombreCompleto = user.NombreCompleto,
                    pTelefono = user.Telefono,
                    pFoto = user.Foto,
                    pDescripcion = user.Descripcion
                }
            );
        }
        int nuevaListaId;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string insertLista = @"
                INSERT INTO Listas (IdUsuario, NombreLista)
                OUTPUT INSERTED.IdLista
                VALUES (@pIdUsuario, @pNombreLista);
            ";

            nuevaListaId = connection.QuerySingle<int>(
                insertLista,
                new { pIdUsuario = nuevoIdUsuario, pNombreLista = "Favoritos" }
            );
        }
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string insertRelacion =
                "INSERT INTO UsuariosLista (IdLista, IdUsuario) VALUES (@pIdLista, @pIdUsuario)";

            connection.Execute(
                insertRelacion,
                new { pIdLista = nuevaListaId, pIdUsuario = nuevoIdUsuario }
            );
        }

        return true;
    }

    public static Usuario login(string usuario, string password)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT * FROM Usuarios WHERE NombreUsuario = @pUsuario AND Contrasenia = @pContrasenia";
            return connection.QueryFirstOrDefault<Usuario>(query, new { pUsuario = usuario, pContrasenia = password });
        }
    }

    public static int subirPublicacion(Publicacion publicacion)
    {
        int idPub = 0;
        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Publicaciones (IdUsuario, Descripcion, Foto, Precio, NombreProducto) OUTPUT INSERTED.IdPublicacion VALUES (@pIdUsuario, @pDescripcion, @pFoto, @pPrecio, @pNombreProducto)";
                idPub = connection.QuerySingle<int>(query, new
                {
                    pIdUsuario = publicacion.IdUsuario,
                    pDescripcion = publicacion.Descripcion,
                    pFoto = publicacion.Foto,
                    pPrecio = publicacion.Precio,
                    pNombreProducto = publicacion.NombreProducto
                });
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error de SQL en subirPublicacion: {ex.Message}");
            Console.WriteLine($"Error de SQL en subirPublicacion: {ex.Message}");
            throw;
        }

        return idPub;
    }
    public static Publicacion DevolverPublicacion(int idPublicacion)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT * FROM Publicaciones WHERE IdPublicacion = @pIdPublicacion";
            return connection.QueryFirstOrDefault<Publicacion>(query, new { pIdPublicacion = idPublicacion });
        }
    }

    public static List<Publicacion> DevolverPublicacionesEtiquetas(int idEtiqueta)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT Publicaciones.* FROM Publicaciones INNER JOIN PublicacionEtiqueta ON Publicaciones.IdPublicacion = PublicacionEtiqueta.IdPublicacion WHERE PublicacionEtiqueta.IdEtiqueta = @pIdEtiqueta";
            return connection.Query<Publicacion>(query, new { pIdEtiqueta = idEtiqueta }).ToList();
        }
    }

    public static List<Publicacion> DevolverPublicacionesBuscador(string texto)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Publicaciones WHERE Descripcion LIKE '%' + @ptexto + '%' OR NombreProducto LIKE '%' + @ptexto + '%'";
                return connection.Query<Publicacion>(query, new { ptexto = texto }).ToList();
            }
        }
        catch (Exception ex)
        {
            return new List<Publicacion>();
        }
    }

    public static bool eliminarPublicacion(int idPublicacion)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();
            try
            {
                var parametros = new { pIdPublicacion = idPublicacion };
                connection.Execute("DELETE FROM PublicacionLista WHERE IdPublicacion = @pIdPublicacion", parametros, transaction: transaction);
                connection.Execute("DELETE FROM PublicacionEtiqueta WHERE IdPublicacion = @pIdPublicacion", parametros, transaction: transaction);

                string query = "DELETE FROM Publicaciones WHERE IdPublicacion = @pIdPublicacion";
                int filasAfectadas = connection.Execute(query, parametros, transaction: transaction);

                transaction.Commit();
                return filasAfectadas > 0;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
            }
        }
    }

    public static void editarPublicacion(Publicacion publicacion, int idPublicacion)
    {
        string query = "UPDATE Publicaciones SET Descripcion = @pDescripcion, Foto = @pFoto, Precio = @pPrecio, NombreProducto = @pNombreProducto WHERE IdPublicacion = @pIdPublicacion AND IdUsuario = @pIdUsuario";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(query, new { pDescripcion = publicacion.Descripcion, pFoto = publicacion.Foto, pPrecio = publicacion.Precio, pNombreProducto = publicacion.NombreProducto, pIdPublicacion = idPublicacion, pIdUsuario = publicacion.IdUsuario });
        }
    }


    public static List<Publicacion> devolverPublicacionesVendedor(int idUsuario)
    {
        string query = "SELECT * FROM Publicaciones WHERE IdUsuario = @pidUsuario";
        List<Publicacion> pubs;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            pubs = connection.Query<Publicacion>(query, new { pidUsuario = idUsuario }).ToList();
        }
        return pubs;
    }

    public static Usuario devolverUsuario(int idUsuario)
    {
        string query = "SELECT * FROM Usuarios WHERE IdUsuario = @pIdUsuario";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            return connection.QuerySingleOrDefault<Usuario>(query, new { pIdUsuario = idUsuario });
        }
    }



    public static List<Lista> devolverListasPorUsuario(int idUsuario)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT Listas.* FROM Listas INNER JOIN UsuariosLista ON Listas.IdLista = UsuariosLista.IdLista WHERE UsuariosLista.IdUsuario = @pIdUsuario";
            return connection.Query<Lista>(query, new { pIdUsuario = idUsuario }).ToList();
        }
    }

    public static void limpiarEtiquetasDePublicacion(int idPublicacion)
    {
        string query = "DELETE FROM PublicacionEtiqueta WHERE IdPublicacion = @pIdPublicacion";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(query, new { pIdPublicacion = idPublicacion });
        }
    }

    public static List<Etiqueta> devolverEtiquetasPorPublicacion(int idPublicacion)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT Etiquetas.* FROM Etiquetas INNER JOIN PublicacionEtiqueta ON Etiquetas.IdEtiqueta = PublicacionEtiqueta.IdEtiqueta WHERE PublicacionEtiqueta.IdPublicacion = @pIdPublicacion";
                return connection.Query<Etiqueta>(query, new { pIdPublicacion = idPublicacion }).ToList();
            }
        }
        catch (Exception ex)
        {
            return new List<Etiqueta>();
        }
    }

    public static string devolverNombreLista(int idLista)
    {
        string query = "SELECT NombreLista FROM Listas WHERE IdLista = @pIdLista";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            return connection.QuerySingleOrDefault<string>(query, new { pIdLista = idLista });
        }
    }
    public static string[] SepararPorEspacio(string texto)
    {
        char[] delimitador = new char[] { ' ' };
        string[] partes = texto.Split(delimitador, StringSplitOptions.RemoveEmptyEntries);
        return partes;
    }
    public static int agregarEtiqueta(string etiqueta)
    {
        int idEtiqueta = 0;
        string nombreNormalizado = etiqueta.ToLower().Trim();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string selectQuery = "SELECT IdEtiqueta FROM Etiquetas WHERE Nombre = @pNombre";
            idEtiqueta = connection.QuerySingleOrDefault<int>(selectQuery, new { pNombre = nombreNormalizado });

            if (idEtiqueta > 0)
            {
                return idEtiqueta;
            }

            string insertQuery = "INSERT INTO Etiquetas (Nombre) OUTPUT INSERTED.IdEtiqueta VALUES (@pNombre)";
            idEtiqueta = connection.QuerySingle<int>(insertQuery, new { pNombre = nombreNormalizado });
        }

        return idEtiqueta;
    }
    public static void agregarEtiquetaPublicacion(int? idPublicacion, int idEtiqueta)
    {
        string query = "INSERT INTO PublicacionEtiqueta (IdPublicacion, IdEtiqueta) VALUES (@pIdPublicacion, @pIdEtiqueta)";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(query, new { pIdPublicacion = idPublicacion, pIdEtiqueta = idEtiqueta });
        }
    }
    public static List<Publicacion> devolverTodasLasPublicaciones()
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT * FROM Publicaciones";
            return connection.Query<Publicacion>(query).ToList();
        }
    }

    public static bool EstaEnLista(int idPublicacion, int idLista)
    {
        string query = @"
        SELECT COUNT(IdPublicacion) 
        FROM PublicacionLista
        WHERE IdPublicacion = @pIdPublicacion 
        AND IdLista = @pIdLista";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            int count = connection.QuerySingle<int>(query, new
            {
                pIdPublicacion = idPublicacion,
                pIdLista = idLista
            });
            return count > 0;
        }
    }
    public static void eliminarDeLista(int idPublicacion, int idLista)
    {
        string query = "DELETE FROM PublicacionLista WHERE IdPublicacion = @pIdPublicacion AND IdLista = @pIdLista";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(query, new { pIdPublicacion = idPublicacion, pIdLista = idLista });
        }
    }
    public static int devolverIdListaUsuario(int idUser)
    {
        string query = "SELECT TOP 1 IdLista FROM Listas WHERE IdUsuario = @pIdUsuario AND NombreLista = 'Favoritos';";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            return connection.QuerySingleOrDefault<int>(query, new
            {
                pIdUsuario = idUser,
            });
        }
    }
    public static void agregarLista(int idPublicacion, int idListaUsuario)
    {
        string query = @"
        IF NOT EXISTS (SELECT 1 FROM PublicacionLista 
                      WHERE IdPublicacion = @pIdPublicacion 
                      AND IdLista = @pIdLista)
        BEGIN
            INSERT INTO PublicacionLista (IdPublicacion, IdLista) 
            VALUES (@pIdPublicacion, @pIdLista)
        END";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(query, new
            {
                pIdPublicacion = idPublicacion,
                pIdLista = idListaUsuario,
            });
        }
    }
    public static List<Publicacion> devolverPublicacionesPorLista(int idLista)
    {
        List<Publicacion> publicaciones = new List<Publicacion>();
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT Publicaciones.* FROM Publicaciones INNER JOIN PublicacionLista ON Publicaciones.IdPublicacion = PublicacionLista.IdPublicacion WHERE PublicacionLista.IdLista = @pIdLista";
            publicaciones = connection.Query<Publicacion>(query, new { pIdLista = idLista }).ToList();
        }
        return publicaciones;
    }
}
