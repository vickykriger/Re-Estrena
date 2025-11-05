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
            string query = "SELECT username FROM Usuarios WHERE username = @pUusername";
            validar = connection.QueryFirstOrDefault<Usuario>(query, new { pUusername = user.username });
        }
        if (validar == null)
        {
            string query = "INSERT INTO Usuarios (nombre, apellido, foto, username, ultimoLogin, password) VALUES (@pnombre, @papellido, @pfoto, @pusername, @pultimoLogin, @ppassword)";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                if (user.ultimoLogin < new DateTime(1753, 1, 1))
                    user.ultimoLogin = DateTime.Now;
                connection.Execute(query, new { pnombre = user.nombre, papellido = user.apellido, pfoto = user.foto, pusername = user.username, pultimoLogin = user.ultimoLogin, ppassword = user.password });
            }
            registrado = true;
        }
        return registrado;
    }
    public static Usuario login(string username, string password)
    {
        Usuario logeado = new Usuario();
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT * FROM Usuarios WHERE username = @pUsername AND password = @pPassword";
            logeado = connection.QueryFirstOrDefault<Usuario>(query, new { pUsername = username, pPassword = password });
        }
        return logeado;
    }
    public static void actualizarLogin(int idUsuario)
    {
        string query = "UPDATE Usuarios SET ultimoLogin = @pLogin WHERE id = @pId";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(query, new { pLogin = DateTime.Now, pId = idUsuario });
        }
    }
    public static bool subirPublicacion(Publicacion publicacion)
    {
        string query = "INSERT INTO Publicaciones (IdUsuario, Descripcion, Foto, Precio) VALUES (@pIdUsuario, @pIdEtiqueta, @pDescripcion, @pFoto, @pPrecio)";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(query, new
            {
                pIdUsuario = publicacion.idUsuario,
                pDescripcion = publicacion.descripcion,
                pFoto = publicacion.foto,
                pPrecio = publicacion.precio
            });
        }
        Publicacion verificar = devolverPublicacion(publicacion.idPublicacion);
        bool existe = false;
        if (verficiar != null)
        {
            existe = true;
        }
        return verificar;
    }

    public static Publicacion devolverPublicacion(int idPublicacion)
    {
        Publicacion publicacion;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT * FROM Publicaciones WHERE IdPublicacion = @pIdPublicacion";
            publicacion = connection.QueryFirstOrDefault<Publicacion>(query, new { pIdPublicacion = idPublicacion });
        }
        return publicacion;
    }

    public static List<Publicacion> devolverPublicacionesEtiquetas(int idEtiqueta)
    {
        List<Publicacion> publicaciones;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT * FROM Publicaciones INNER JOIN PublicacionEtiqueta ON Publicaciones.IdPublicacion = PublicacionEtiqueta.IdPublicacion WHERE PublicacionEtiqueta.IdEtiqueta = @pIdEtiqueta";
            publicaciones = connection.Query<Publicacion>(query, new { pIdEtiqueta = idEtiqueta }).ToList();
        }
        return publicaciones;
    }
    public static List<Publicacion> devolverPublicacionesBuscador(string texto)
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
                pIdPublicacion = idPublicacion;
                pIdLista = idLista;
            });
        }
    }
    public static void editarPublicacion(int idPublicacion, int IdUsuario, string Descripcion, string Foto, decimal Precio)
    {
        string query = "UPDATE Publicaciones SET IdUsuario = @pIdUsuario, Descripcion = @pDescripcion, Foto = @pFoto, Precio = @pPrecio WHERE IdPublicacion = @pIdPublicacion";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(query, new { pIdUsuario = IdUsuario, pDescripcion = Descripcion, pFoto = Foto, pPrecio = Precio, pIdPublicacion = IdPublicacion });
        }
    }
    public static void editarUsuario(int idUsuario, string email, string contrasenia, string nombreUsuario, string nombreCompleto, string pais, int telefono)
    {
        string query = "UPDATE Usuarios SET Email = @pEmail, Contrasenia = @pContrasenia, NombreUsuario = @pNombreUsuario, NombreCompleto = @pNombreCompleto, Pais = @pPais, Telefono = @pTelefono WHERE IdUsuario = @pIdUsuario";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(query, new { pEmail = email, pContrasenia = contrasenia, pNombreUsuario = nombreUsuario, pNombreCompleto = nombreCompleto, pPais = pais, pTelefono = telefono, pIdUsuario = idUsuario });
        }
    }
}