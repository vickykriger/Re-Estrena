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
        using(SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT username FROM Usuarios WHERE username = @pUusername";
            validar = connection.QueryFirstOrDefault<Usuario>(query, new { pUusername = user.username});
        }
        if (validar== null)
        {
            string query = "INSERT INTO Usuarios (nombre, apellido, foto, username, ultimoLogin, password) VALUES (@pnombre, @papellido, @pfoto, @pusername, @pultimoLogin, @ppassword)";
            using(SqlConnection connection = new SqlConnection(_connectionString))
            {
                if (user.ultimoLogin < new DateTime(1753, 1, 1))
                    user.ultimoLogin = DateTime.Now;
                connection.Execute(query, new {pnombre = user.nombre, papellido = user.apellido, pfoto = user.foto, pusername = user.username, pultimoLogin = user.ultimoLogin, ppassword = user.password});
            }
            registrado = true;
        }
        return registrado;
    }
    public static Usuario login(string username, string password)
    {
        Usuario logeado = new Usuario();
        using(SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT * FROM Usuarios WHERE username = @pUsername AND password = @pPassword";
            logeado = connection.QueryFirstOrDefault<Usuario>(query, new { pUsername = username, pPassword = password});
        }
        return logeado;
    }
    public static void actualizarLogin(int idUsuario)
    {
        string query = "UPDATE Usuarios SET ultimoLogin = @pLogin WHERE id = @pId";
        using(SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(query, new {pLogin = DateTime.Now, pId = idUsuario});
        }
    }

     public static void subirPublicacion(Publicacion publicacion)
    {
        string query = "INSERT INTO PublicacionLista (IdPublicacion, IdUsuario, IdEtiqueta, Descripcion, Foto, Precio) VALUES (@pIdPublicacion, @pIdUsuario, @pIdEtiqueta, @pDescripcion, @pFoto, @pPrecio)";

    using (SqlConnection connection = new SqlConnection(_connectionString))

        {
            connection.Execute(query, new {pIdPublicacion = publicacion.idPublicacion,
            pIdUsuario = publicacion.idUsuario,
            pIdEtiqueta = publicacion.idEtiqueta,
            pDescripcion = publicacion.descripcion,
            pFoto = publicacion.foto,
            pPrecio = publicacion.precio});
        }
    }
public static Publicacion devolverPublicacion(int idPublicacion)
{
    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
        string query = "SELECT * FROM PublicacionLista WHERE IdPublicacion = @pIdPublicacion";
        Publicacion publicacion = connection.QueryFirstOrDefault<Publicacion>(
            query,
            new { pIdPublicacion = idPublicacion });
        return publicacion;
    }
}

public static List<Publicacion> devolverPublicacionesEtiquetas(int idEtiqueta)
{
    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
        string query = "SELECT * FROM PublicacionLista WHERE IdEtiqueta = @pIdEtiqueta";
 
        List<Publicacion> publicaciones = connection.Query<Publicacion>(query, new { pIdEtiqueta = idEtiqueta }).ToList(); return publicaciones;
    }
} 
}