using Microsoft.Data.Sqlite;
namespace tienda;

/*
Crear un repositorio llamado ProductoRepository para gestionar todas las
operaciones relacionadas con Productos. Este repositorio debe incluir métodos para:
● ----------Crear un nuevo Producto. (recibe un objeto Producto)
● ----------Modificar un Producto existente. (recibe un Id y un objeto Producto)
● ----------Listar todos los Productos registrados. (devuelve un List de Producto)
● ----------Obtener detalles de un Productos por su ID. (recibe un Id y devuelve un Producto)
● ----------Eliminar un Producto por ID
*/
public class ProductoRepository
{
    public string cadenaConexion = "Data Source=./../Tienda.db";
    public List<Producto> productos = new List<Producto>();

    public void CrearProducto(Producto productoNuevo)
    {
        var queryString = $"INSERT INTO Productos (Descripcion,Precio) VALUES (@descripcion,@precio);";
        using (SqliteConnection conexion = new SqliteConnection(cadenaConexion))
        {
            conexion.Open();
            SqliteCommand comando = new SqliteCommand(queryString, conexion);

            comando.Parameters.Add(new SqliteParameter("@descripcion", productoNuevo.Descripcion));
            comando.Parameters.Add(new SqliteParameter("@precio", productoNuevo.Precio));

            comando.ExecuteNonQuery();
            conexion.Close();
        }

        productos.Add(productoNuevo);
    }

    public void ModificarProducto(int idProducto, Producto productoNuevo)
    {
        var queryString = $"UPDATE Productos SET Descripcion=@descripcion WHERE idProducto=@idP;";
        // var queryString = $"UPDATE Productos SET Descripcion=@descripcion, Precio=@precio WHERE idProducto=@idP;";
        using (SqliteConnection conexion = new SqliteConnection(cadenaConexion))
        {
            conexion.Open();
            SqliteCommand comando = new SqliteCommand(queryString, conexion);

            comando.Parameters.Add(new SqliteParameter("@descripcion", productoNuevo.Descripcion));
            // comando.Parameters.Add(new SqliteParameter("@precio", productoNuevo.Precio));
            comando.Parameters.Add(new SqliteParameter("@idP", idProducto));

            comando.ExecuteNonQuery();
            conexion.Close();
        }
    }
    public List<Producto> GetProductos()
    {
        var queryString = @"SELECT * FROM Productos;";
        using (SqliteConnection conexion = new SqliteConnection(cadenaConexion))
        {
            conexion.Open();
            SqliteCommand comando = new SqliteCommand(queryString, conexion);
            using (SqliteDataReader reader = comando.ExecuteReader())
            {
                while (reader.Read())
                {
                    Producto producto = new Producto();
                    producto.IdProducto = Convert.ToInt32(reader["idProducto"]);
                    producto.Descripcion = reader["Descripcion"].ToString();
                    producto.Precio = Convert.ToInt32(reader["Precio"]);

                    productos.Add(producto);
                }
            }
            conexion.Close();
        }

        return productos;
    }

    //● Obtener detalles de un Productos por su ID. (recibe un Id y devuelve un Producto)
    public Producto ObtenerProducto(int idProducto)
    {
        Producto producto = new Producto();
        var queryString = $"SELECT * FROM Productos WHERE idProducto=@idP;";
        using (SqliteConnection conexion = new SqliteConnection(cadenaConexion))
        {
            conexion.Open();
            SqliteCommand comando = new SqliteCommand(queryString, conexion);

            comando.Parameters.Add(new SqliteParameter("@idP", idProducto));

            using (SqliteDataReader reader = comando.ExecuteReader())
            {
                 while (reader.Read())
                {
                    producto.IdProducto = Convert.ToInt32(reader["idProducto"]);
                    producto.Descripcion = reader["Descripcion"].ToString();
                    producto.Precio = Convert.ToInt32(reader["Precio"]);
                }
            }

            conexion.Close();
        }

        return producto;
    }

    // // ● Eliminar un Producto por ID
    public void EliminarProducto(int idProducto)
    {
        var queryString = $"DELETE FROM Productos WHERE idProducto=@idP;";
        using (SqliteConnection conexion = new SqliteConnection(cadenaConexion))
        {
            conexion.Open();
            SqliteCommand comando = new SqliteCommand(queryString, conexion);

            comando.Parameters.Add(new SqliteParameter("@idP", idProducto));

            comando.ExecuteNonQuery();
            conexion.Close();
        }
    }

}