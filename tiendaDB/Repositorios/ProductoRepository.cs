using Microsoft.Data.Sqlite;
namespace tienda;

public class ProductoRepository
{
    public string cadenaConexion = "Data Source=./../Tienda.db";
    public List<Producto> productos = new List<Producto>();
    public List<Producto> GetProductos()
    {
        var queryString = @"SELECT * FROM Productos;";
        using (SqliteConnection conexion = new SqliteConnection(cadenaConexion))
        {
            SqliteCommand comando = new SqliteCommand(queryString, conexion);
            conexion.Open();
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

}