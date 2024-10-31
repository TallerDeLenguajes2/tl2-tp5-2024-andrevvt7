namespace tienda;

public class Presupuesto{
    int idPresupuesto;
    string? nombreDestinatario;
    List<PresupuestoDetalle>? detalle;

    public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value; }
    public string? NombreDestinatario { get => nombreDestinatario; set => nombreDestinatario = value; }
    public List<PresupuestoDetalle>? Detalle { get => detalle; set => detalle = value; }

    public Presupuesto(){
        Detalle = new List<PresupuestoDetalle>();
    }
    public void AgregarProducto(Producto producto, int cantidad){
        PresupuestoDetalle presupuestoDetalleNuevo = new PresupuestoDetalle();
        presupuestoDetalleNuevo.CargarProducto(producto);
        presupuestoDetalleNuevo.Cantidad = cantidad;
        Detalle.Add(presupuestoDetalleNuevo);
    }

    public int MontoPresupuesto(){
        int monto = 0;
        foreach (var presupuestoDetalle in Detalle)
        {
            monto += presupuestoDetalle.Cantidad * presupuestoDetalle.Producto.Precio;
        }

        return monto;
    }
    public int MontoPresupuestoConIva(){
        int monto = 0;
        
        return monto;
    }
    public int CantidadProductos(){
        int cantidad = 0;
        foreach (var presupuestoDetalle in Detalle)
        {
            cantidad += presupuestoDetalle.Cantidad;
        }
        
        return cantidad;
    }
}