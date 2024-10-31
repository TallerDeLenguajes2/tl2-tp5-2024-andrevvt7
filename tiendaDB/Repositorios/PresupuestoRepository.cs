using Microsoft.Data.Sqlite;
namespace tienda;

/*
● --------Crear un nuevo Presupuesto. (recibe un objeto Presupuesto)
● --------Listar todos los Presupuestos registrados. (devuelve un List de Presupuestos)
● --------Obtener detalles de un Presupuesto por su ID. (recibe un Id y devuelve un
Producto)
● --------Eliminar un Presupuesto por ID
*/

public class PresupuestoRepository
{
    public string cadenaConexion = "Data Source=./../Tienda.db";
    public List<Presupuesto> presupuestos = new List<Presupuesto>();

    public void CrearPresupuesto(Presupuesto presupuestoNuevo)
    {
        var queryString = $"INSERT INTO Presupuestos (NombreDestinatario,FechaCreacion) VALUES (@nombreDestinatario,@fechaCreacion);";
        using (SqliteConnection conexion = new SqliteConnection(cadenaConexion))
        {
            conexion.Open();
            SqliteCommand comando = new SqliteCommand(queryString, conexion);

            comando.Parameters.Add(new SqliteParameter("@nombreDestinatario", presupuestoNuevo.NombreDestinatario));
            comando.Parameters.Add(new SqliteParameter("@fechaCreacion", DateTime.Now));

            comando.ExecuteNonQuery();
            conexion.Close();
        }

        presupuestos.Add(presupuestoNuevo);
    }

    public List<Presupuesto> GetPresupuestos()
    {
        var queryString = @"SELECT * FROM Presupuestos;";
        using (SqliteConnection conexion = new SqliteConnection(cadenaConexion))
        {
            conexion.Open();
            SqliteCommand comando = new SqliteCommand(queryString, conexion);

            using (SqliteDataReader reader = comando.ExecuteReader())
            {
                while (reader.Read())
                {
                    presupuestos.Add(ObtenerPresupuesto(Convert.ToInt32(reader["idPresupuesto"])));
                }
            }
            conexion.Close();
        }

        return presupuestos;
    }

    public Presupuesto ObtenerPresupuesto(int idPresupuesto)
    {
        Presupuesto presupuesto = new Presupuesto();
        var queryString = $"SELECT * FROM Presupuestos WHERE idPresupuesto=@idP;";
        var queryString2 = @"SELECT * FROM PresupuestosDetalle WHERE idPresupuesto = @idPresupuesto;;";

        using (SqliteConnection conexion = new SqliteConnection(cadenaConexion))
        {
            conexion.Open();
            SqliteCommand comando = new SqliteCommand(queryString, conexion);

            comando.Parameters.Add(new SqliteParameter("@idP", idPresupuesto));

            using (SqliteDataReader reader = comando.ExecuteReader())
            {
                while (reader.Read())
                {
                    presupuesto.IdPresupuesto = Convert.ToInt32(reader["idPresupuesto"]);
                    presupuesto.NombreDestinatario = reader["NombreDestinatario"].ToString();

                    SqliteCommand comando2 = new SqliteCommand(queryString2, conexion);
                    comando2.Parameters.Add(new SqliteParameter("@idPresupuesto", presupuesto.IdPresupuesto));

                    using (SqliteDataReader reader2 = comando2.ExecuteReader())
                    {
                        ProductoRepository repoProductos = new ProductoRepository();
                        List<Producto> productos = repoProductos.GetProductos();
                        while (reader2.Read())
                        {
                            presupuesto.AgregarProducto(productos.FirstOrDefault(p => p.IdProducto == Convert.ToInt32(reader2["idProducto"])), Convert.ToInt32(reader2["cantidad"]));
                        }
                    }
                }

                conexion.Close();
            }

            return presupuesto;
        }
    }

    public void AgregarDetalle(int idPresupuesto, PresupuestoDetalle presupuestoDetalle)
    {
        var queryString = $"INSERT INTO PresupuestosDetalle (idPresupuesto,idProducto,Cantidad) VALUES (@idPresupuesto,@idProducto,@cantidad);";
        using (SqliteConnection conexion = new SqliteConnection(cadenaConexion))
        {
            conexion.Open();
            SqliteCommand comando = new SqliteCommand(queryString, conexion);

            comando.Parameters.Add(new SqliteParameter("@idPresupuesto", idPresupuesto));
            comando.Parameters.Add(new SqliteParameter("@idProducto", presupuestoDetalle.Producto.IdProducto));
            comando.Parameters.Add(new SqliteParameter("@cantidad", presupuestoDetalle.Cantidad));

            comando.ExecuteNonQuery();
            conexion.Close();
        }
    }


    //______________________________________________________________________________________________
    public void EliminarPresupuesto(int idPresupuesto)
    {
        var queryString = $"DELETE FROM PresupuestosDetalle WHERE idPresupuesto=@idP;";
        var queryString2 = $"DELETE FROM Presupuestos WHERE idPresupuesto=@idP;";

        using (SqliteConnection conexion = new SqliteConnection(cadenaConexion))
        {
            conexion.Open();
            SqliteCommand comando = new SqliteCommand(queryString, conexion);
            SqliteCommand comando2 = new SqliteCommand(queryString2, conexion);

            comando.Parameters.Add(new SqliteParameter("@idP", idPresupuesto));
            comando2.Parameters.Add(new SqliteParameter("@idP", idPresupuesto));

            comando.ExecuteNonQuery();
            comando2.ExecuteNonQuery();
            conexion.Close();
        }
    }

}