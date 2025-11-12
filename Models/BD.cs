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

        Usuario validar = new Usuario();
        bool registrado = false;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT NombreUsuario FROM Usuarios WHERE NombreUsuario = @pUNombreUsuario";
            validar = connection.QueryFirstOrDefault<Usuario>(query, new { pUNombreUsuario = user.NombreUsuario });
        }
        if (validar == null)
        {
            string query = "INSERT INTO Usuarios (Email, Contrasenia, Usuario, NombreCompleto, Pais, Telefono, Foto) VALUES (@pEmail, @pContrasenia, @pUsuario, @pNombreCompleto, @pPais, @pTelefono, @pFoto)";
            string query1 = "INSERT INTO Listas (IdUsuario, NombreLista) VALUES (@pIdUsuario, @pNombreLista)";
            string query2 = "INSERT INTO UsuariosLista (IdLista, IdUsuario) VALUES (@pIdLista, @pIdUsuario)";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Execute(query, new { pEmail = user.Email, pContrasenia = user.Contrasenia, pUsuario = user.NombreUsuario, pNombreCompleto = user.NombreCompleto, pPais = user.Pais, pTelefono = user.Telefono, pFoto = user.Foto });
                connection.Execute(query1, new { pIdUsuario = user.IdUsuario, pNombreLista = "Favoritos"});
                connection.Execute(query2, new { pIdLista = 1, pIdUsuario = userIdUsuario});
            }
            registrado = true;
        }
        return registrado;
    }
    public static Usuario login(string NombreUsuario, string password)
    {
        Usuario logeado = new Usuario();
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT * FROM Usuarios WHERE NombreUsuario = @pNombreUsuario AND Contrasenia = @pContrasenia";
            logeado = connection.QueryFirstOrDefault<Usuario>(query, new { pNombreUsuario = NombreUsuario, pContrasenia = password });
        }
        return logeado;
    }
    public static bool subirPublicacion(Publicacion publicacion)
    {
        string query = "INSERT INTO Publicaciones (IdUsuario, Descripcion, Foto, Precio, NombreProducto) VALUES (@pIdUsuario, @pIdEtiqueta, @pDescripcion, @pFoto, @pPrecio, @pNombreProducto)";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(query, new
            {
                pIdUsuario = publicacion.idUsuario,
                pDescripcion = publicacion.descripcion,
                pFoto = publicacion.foto,
                pPrecio = publicacion.precio,
                pNombreProducto = publicacion.NombreProducto
            });
        }
        Publicacion verificar = DevolverPublicacion(publicacion.idPublicacion);
        bool existe = false;
        if (verficiar != null)
        {
            existe = true;
        }
        return verificar;
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
            string query = "SELECT * FROM Publicaciones WHERE Descripcion LIKE '%@ptexto%'";
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
        Publicacion verificar = devolverPublicacion(idPublicacion);
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

    public static void editarPublicacion(int idPublicacion, int IdUsuario, string Descripcion, string Foto, decimal Precio, string NombreProducto)
    {
        string query = "UPDATE Publicaciones SET IdUsuario = @pIdUsuario, Descripcion = @pDescripcion, Foto = @pFoto, Precio = @pPrecio, NombreProducto = @pNombreProducto WHERE IdPublicacion = @pIdPublicacion";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(query, new { pIdUsuario = IdUsuario, pDescripcion = Descripcion, pFoto = Foto, pPrecio = Precio, pIdPublicacion = IdPublicacion, pNombreProducto = NombreProducto });
        }
    }
    public static void editarUsuario(int idUsuario, string email, string contrasenia, string nombreUsuario, string nombreCompleto, string pais, int telefono, string foto)
    {
        string query = "UPDATE Usuarios SET Email = @pEmail, Contrasenia = @pContrasenia, NombreUsuario = @pNombreUsuario, NombreCompleto = @pNombreCompleto, Pais = @pPais, Telefono = @pTelefono WHERE IdUsuario = @pIdUsuario, Foto = @pFoto";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(query, new { pEmail = email, pContrasenia = contrasenia, pNombreUsuario = nombreUsuario, pNombreCompleto = nombreCompleto, pPais = pais, pTelefono = telefono, pIdUsuario = idUsuario, pFoto = foto });
        }
    }
    public static List<Publicacion> devolverPublicacionesVendedor(int idUsuario)
    {
        string query = "SELECT * FROM Publicaciones WHERE IdUsuario = @pidUsuario";
        List<Publicacion> publicaciones;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            publicaciones = connection.Execute(query, new { pidUsuario = idUsuario}).ToList();
        }
        return publicaciones;
    }
    public static Usuario devolverUsuario(int idUsuario)
    {
        string query = "SELECT * FROM Usuarios WHERE IdUsuario = @pIdUsuario";
        Usuario user;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            user = connection.Execute(query, new {pIdUsuario = idUsuario});
        }
        return user;
    }
    public static List<Publicacion> devolverPublicacionesPorLista(int idLista)
    {
        string query = "SELECT * FROM Publicaciones INNER JOIN PublicacionLista ON Publicaciones.IdPublicacion = PublicacionLista.IdPublicacion WHERE PublicacionLista.IdLista = @pIdLista";
        List<Publicacion> publicaciones;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            publicaciones = connection.Execute(query, new {pIdLista = idLista}).ToList();
        }
        return publicaciones;
    }
    public static List<Lista> devolverListasPorUsuario(int idUsuario)
    {
        string query = "SELECT * FROM Listas INNER JOIN UsuariosLista ON Listas.IdLista = UsuariosLista.IdLista WHERE UsuariosLista.IdUsuario = @pIdUsuario";
        List<Lista> listas;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            publicaciones = connection.Execute(query, new {pIdUsuario = idUsuario}).ToList();
        }
        return listas;
    }
    public static List<Etiqueta> devolverLista(int idPublicacion)
    {
        string query = "SELECT * FROM Etiquetas INNER JOIN PublicacionEtiqueta ON Etiquetas.IdEtiqueta = PublicacionEtiqueta.IdEtiqueta WHERE PublicacionEtiqueta.IdPublicacion = @pIdPublicacion";
        List<Etiqueta> etiquetas;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            etiquetas = connection.Execute(query, new {pIdPublicacion = idPublicacion}).ToList();
        }
        return etiquetas;
    }
    public static void hacerLista(Lista lista)
    {
        string query = "INSERT INTO Listas (IdUsuario, NombreLista) VALUES (@pIdUsuario, @pNombreLista)";
        string query1 = "INSERT INTO UsuariosLista (IdLista, IdUsuario) VALUES (@pIdLista, @pIdUsuario)";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(query, new { pIdUsuario = lista.IdUsuario, pNombreLista = lista.NombreLista});
            connection.Execute(query1, new { pIdLista = lista.IdLista, pIdUsuario = lista.IdUsuario});
        }
    }
    public static void eliminarDeLista(int idPublicacion, int idLista)
    {
        string query = "DELETE FROM PublicacionLista WHERE IdPublicacion = @pIdPublicacion AND IdLista = @pIdLista";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(query, new { pIdPublicacion = idPublicacion, pIdLista = idLista});
        }
    }
    public static void eliminarLista(Lista lista)
    {
        string query = "DELETE FROM Listas WHERE IdLista = @pIdLista";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(query, new { pIdLista = lista.IdLista});
        }
    }
}
