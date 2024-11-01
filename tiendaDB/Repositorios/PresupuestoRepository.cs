using Microsoft.Data.Sqlite;
namespace tienda;

/*
● --------Crear un nuevo Presupuesto. (recibe un objeto Presupuesto)
● --------Listar todos los Presupuestos registrados. (devuelve un List de Presupuestos)
● --------Obtener detalles de un Presupuesto por su ID. (recibe un Id y devuelve un Producto)
● --------Eliminar un Presupuesto por ID
*/

public class PresupuestoRepository
{
    public string cadenaConexion = "Data Source=./../Tienda.db";
    public List<Presupuesto> presupuestosCompleto = new List<Presupuesto>();
    public List<Presupuesto> presupuestosSinDetalles = new List<Presupuesto>();

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

        presupuestosCompleto.Add(presupuestoNuevo);
        presupuestosSinDetalles.Add(presupuestoNuevo);
    }

public List<Presupuesto> GetPresupuestosSinDetalles()
    {
        var consultaGeneral = @"SELECT * FROM Presupuestos;";
        using (SqliteConnection conexion = new SqliteConnection(cadenaConexion))
        {
            conexion.Open();
            SqliteCommand comandoGeneral = new SqliteCommand(consultaGeneral, conexion);

            using (SqliteDataReader reader = comandoGeneral.ExecuteReader())
            {
                while (reader.Read())
                {
                    Presupuesto presupuesto = new Presupuesto();
                    presupuesto.IdPresupuesto = Convert.ToInt32(reader["idPresupuesto"]);
                    presupuesto.NombreDestinatario = reader["NombreDestinatario"].ToString();
                    presupuestosSinDetalles.Add(presupuesto);
                }
            }

            conexion.Close();
        }

        return presupuestosSinDetalles;
    }

    public List<Presupuesto> GetPresupuestosCompleto()
    {
        var consultaGeneral = @"SELECT * FROM Presupuestos;";
        var consultaConProductos = @"SELECT * FROM Presupuestos LEFT JOIN PresupuestosDetalle ON Presupuestos.idPresupuesto = PresupuestosDetalle.idPresupuesto INNER JOIN Productos ON PresupuestosDetalle.idProducto = Productos.idProducto ORDER BY Presupuestos.idPresupuesto;";
        using (SqliteConnection conexion = new SqliteConnection(cadenaConexion))
        {
            conexion.Open();
            SqliteCommand comandoGeneral = new SqliteCommand(consultaGeneral, conexion);
            SqliteCommand comandoConProductos = new SqliteCommand(consultaConProductos, conexion);

            using (SqliteDataReader reader = comandoGeneral.ExecuteReader())
            {
                while (reader.Read())
                {
                    Presupuesto presupuesto = new Presupuesto();
                    presupuesto.IdPresupuesto = Convert.ToInt32(reader["idPresupuesto"]);
                    presupuesto.NombreDestinatario = reader["NombreDestinatario"].ToString();
                    presupuestosCompleto.Add(presupuesto);
                }
            }

            using (SqliteDataReader reader = comandoConProductos.ExecuteReader())
            {
                while (reader.Read())
                {
                    Presupuesto? presupuesto = presupuestosCompleto.FirstOrDefault(p => p.IdPresupuesto == Convert.ToInt32(reader["idPresupuesto"]));
                    Producto producto = new Producto();
                    producto.IdProducto = Convert.ToInt32(reader["idProducto"]);
                    producto.Descripcion = reader["Descripcion"].ToString();
                    producto.Precio = Convert.ToInt32(reader["Precio"]);
                    presupuesto.AgregarProducto(producto, Convert.ToInt32(reader["Cantidad"]));
                }

            }
            conexion.Close();
        }

        return presupuestosCompleto;
    }

    public Presupuesto ObtenerPresupuesto(int idPresupuesto){
        Presupuesto? presupuesto = presupuestosCompleto.FirstOrDefault(p => p.IdPresupuesto == idPresupuesto);

        return presupuesto;
    }
    
    public List<PresupuestoDetalle> DetallesPresupuesto(int idPresupuesto){
        List<PresupuestoDetalle> detalle = ObtenerPresupuesto(idPresupuesto).Detalle;

        return detalle;
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