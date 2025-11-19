namespace ReEstrena.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Dapper;

public static class BD
{
    private static string _connectionString = @"Server=localhost;
    DataBase=BD;Integrated Security =True;TrustServerCertificate=True;";
    public static bool registrarse(Usuario user)
    {
        bool registrado = false;
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
        INSERT INTO Usuarios (Email, Contrasenia, NombreUsuario, NombreCompleto, Pais, Telefono, Foto)
        OUTPUT INSERTED.IdUsuario
        VALUES (@pEmail, @pContrasenia, @pUsuario, @pNombreCompleto, @pPais, @pTelefono, @pFoto);
        ";

            nuevoIdUsuario = connection.QuerySingle<int>(
                insertUser,
                new
                {
                    pEmail = user.Email,
                    pContrasenia = user.Contrasenia,
                    pUsuario = user.NombreUsuario,
                    pNombreCompleto = user.NombreCompleto,
                    pPais = user.Pais,
                    pTelefono = user.Telefono,
                    pFoto = user.Foto
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
        Usuario logeado = new Usuario();
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT * FROM Usuarios WHERE NombreUsuario = @pUsuario AND Contrasenia = @pContrasenia";
            logeado = connection.QueryFirstOrDefault<Usuario>(query, new { pUsuario = usuario, pContrasenia = password });
        }
        return logeado;
    }
    public static bool subirPublicacion(Publicacion publicacion)
    {
        string query = "INSERT INTO Publicaciones (IdUsuario, Descripcion, Foto, Precio, NombreProducto) VALUES (@pIdUsuario, @pDescripcion, @pFoto, @pPrecio, @pNombreProducto)";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(query, new
            {
                pIdUsuario = publicacion.IdUsuario,
                pDescripcion = publicacion.Descripcion,
                pFoto = publicacion.Foto,
                pPrecio = publicacion.Precio,
                pNombreProducto = publicacion.NombreProducto
            });
        }
        Publicacion verificar = DevolverPublicacion(publicacion.IdPublicacion);
        bool existe = false;
        if (verificar != null)
        {
            existe = true;
        }
        return existe;
    }

    public static Publicacion DevolverPublicacion(int idPublicacion)
    {
        Publicacion publicacion;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT * FROM Publicaciones WHERE IdPublicacion = @pIdPublicacion";
            publicacion = connection.QueryFirstOrDefault<Publicacion>(query, new { pIdPublicacion = idPublicacion });
        }
        return publicacion;
    }

    public static Publicacion RandomPublicacion()
    {
        Publicacion publicacion;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT * FROM Publicaciones ORDER BY NEWID()";
            publicacion = connection.QueryFirstOrDefault<Publicacion>(query);
        }
        return publicacion;
    }

    public static List<Publicacion> DevolverPublicacionesEtiquetas(int idEtiqueta)
    {
        List<Publicacion> publicaciones;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT * FROM Publicaciones INNER JOIN PublicacionEtiqueta ON Publicaciones.IdPublicacion = PublicacionEtiqueta.IdPublicacion WHERE PublicacionEtiqueta.IdEtiqueta = @pIdEtiqueta";
            publicaciones = connection.Query<Publicacion>(query, new { pIdEtiqueta = idEtiqueta }).ToList();
        }
        return publicaciones;
    }
    public static List<Publicacion> DevolverPublicacionesBuscador(string texto)
    {
        List<Publicacion> publicaciones;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT * FROM Publicaciones WHERE Descripcion LIKE '%' + @ptexto + '%'";
            publicaciones = connection.Query<Publicacion>(query, new { ptexto = texto }).ToList();
        }
        return publicaciones;
    }
    public static bool eliminarPublicacion(int idPublicacion)
    {
        string query = "DELETE FROM Publicaciones WHERE IdPublicacion = @pIdPublicacion";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(query, new
            {
                pIdPublicacion = idPublicacion
            });
        }
        Publicacion verificar = DevolverPublicacion(idPublicacion);
        bool eliminado = false;
        if (verificar == null)
        {
            eliminado = true;
        }
        return eliminado;
    }
    public static void agregarLista(int idPublicacion, int idLista)
    {
        string query = "INSERT INTO PublicacionLista (IdPublicacion, IdLista) VALUES (@pIdPublicacion, @pIdLista)";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(query, new
            {
                pIdPublicacion = idPublicacion,
                pIdLista = idLista,
            });
        }
    }

    public static void editarPublicacion(Publicacion publicacion, int id)
    {
        string query = "UPDATE Publicaciones SET IdUsuario = @pIdUsuario, Descripcion = @pDescripcion, Foto = @pFoto, Precio = @pPrecio, NombreProducto = @pNombreProducto WHERE IdPublicacion = @pIdPublicacion";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(query, new { pIdUsuario = publicacion.IdUsuario, pDescripcion = publicacion.Descripcion, pFoto = publicacion.Foto, pPrecio = publicacion.Precio, pNombreProducto = publicacion.NombreProducto, pIdPublicacion = id });
        }
    }
    public static void editarUsuario(Usuario user, int id)
    {
        string query = "UPDATE Usuarios SET Email = @pEmail, Contrasenia = @pContrasenia, NombreUsuario = @pNombreUsuario, NombreCompleto = @pNombreCompleto, Pais = @pPais, Telefono = @pTelefono WHERE IdUsuario = @pIdUsuario";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(query, new { pEmail = user.Email, pContrasenia = user.Contrasenia, pNombreUsuario = user.NombreUsuario, pNombreCompleto = user.NombreCompleto, pPais = user.Pais, pTelefono = user.Telefono, pIdUsuario = id });
        }
    }
    public static List<Publicacion> devolverPublicacionesVendedor(int idUsuario)
    {
        string query = "SELECT * FROM Publicaciones WHERE IdUsuario = @pidUsuario";
        List<Publicacion> publicaciones;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            publicaciones = connection.Query<Publicacion>(query, new { pidUsuario = idUsuario }).ToList();
        }
        return publicaciones;
    }
    public static Usuario devolverUsuario(int idUsuario)
    {
        string query = "SELECT * FROM Usuarios WHERE IdUsuario = @pIdUsuario";
        Usuario user;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            user = connection.QuerySingleOrDefault<Usuario>(query, new { pIdUsuario = idUsuario });
        }
        return user;
    }
    public static List<Publicacion> devolverPublicacionesPorLista(int idLista)
    {
        string query = "SELECT * FROM Publicaciones INNER JOIN PublicacionLista ON Publicaciones.IdPublicacion = PublicacionLista.IdPublicacion WHERE PublicacionLista.IdLista = @pIdLista";
        List<Publicacion> publicaciones;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            publicaciones = connection.Query<Publicacion>(query, new { pIdLista = idLista }).ToList();
        }
        return publicaciones;
    }
    public static List<Lista> devolverListasPorUsuario(int idUsuario)
    {
        string query = "SELECT * FROM Listas INNER JOIN UsuariosLista ON Listas.IdLista = UsuariosLista.IdLista WHERE UsuariosLista.IdUsuario = @pIdUsuario";
        List<Lista> listas;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            listas = connection.Query<Lista>(query, new { pIdUsuario = idUsuario }).ToList();
        }
        return listas;
    }
    public static List<Etiqueta> devolverEtiquetasPorPublicacion(int idPublicacion)
    {
        string query = "SELECT * FROM Etiquetas INNER JOIN PublicacionEtiqueta ON Etiquetas.IdEtiqueta = PublicacionEtiqueta.IdEtiqueta WHERE PublicacionEtiqueta.IdPublicacion = @pIdPublicacion";
        List<Etiqueta> etiquetas;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            etiquetas = connection.Query<Etiqueta>(query, new { pIdPublicacion = idPublicacion }).ToList();
        }
        return etiquetas;
    }
    public static void hacerLista(Lista lista)
    {
        string query = "INSERT INTO Listas (IdUsuario, NombreLista) VALUES (@pIdUsuario, @pNombreLista)";
        string query1 = "INSERT INTO UsuariosLista (IdLista, IdUsuario) VALUES (@pIdLista, @pIdUsuario)";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(query, new { pIdUsuario = lista.IdUsuario, pNombreLista = lista.NombreLista });
            connection.Execute(query1, new { pIdLista = lista.IdLista, pIdUsuario = lista.IdUsuario });
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
    public static void eliminarLista(int idLista)
    {
        string query = "DELETE FROM Listas WHERE IdLista = @pIdLista";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(query, new { pIdLista = idLista });
        }
    }
    public static string devolverNombreLista(int idLista)
    {
        string query = "SELECT NombreLista FROM Listas WHERE IdLista = @pIdLista";
        string nombre = "";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            nombre = connection.QuerySingleOrDefault<string>(query, new { pIdLista = idLista });
        }
        return nombre;
    }
}
