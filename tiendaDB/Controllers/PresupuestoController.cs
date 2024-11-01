using Microsoft.AspNetCore.Mvc;
namespace tienda;

/*
● ----------POST /api/Presupuesto: Permite crear un Presupuesto.
● ----------POST /api/Presupuesto/{id}/ProductoDetalle: Permite agregar un Producto existente
y una cantidad al presupuesto.
● ----------GET /api/presupuesto: Permite listar los presupuestos existentes.
*/

[ApiController]
[Route("[controller]")]
public class PresupuestoController : ControllerBase
{
    List<Presupuesto> presupuestos;
    List<Presupuesto> presupuestosSinDetalles;
    List<Producto> productos;
    PresupuestoRepository presupestoRepositorio = new PresupuestoRepository();
    ProductoRepository productoRepositorio = new ProductoRepository();

    public PresupuestoController()
    {
        presupuestos = presupestoRepositorio.GetPresupuestosCompleto();
        presupuestosSinDetalles = presupestoRepositorio.GetPresupuestosSinDetalles();
        productos = productoRepositorio.GetProductos();
    }

     [HttpPost("CrearPresupuesto")]
    public IActionResult CrearPresupuesto([FromBody] Presupuesto presupuesto)
    {
        presupestoRepositorio.CrearPresupuesto(presupuesto);

        return Ok($"Presupuesto creado con ID {presupuesto.IdPresupuesto}");
    }
        
    [HttpPost("AgregarDetallePresupuesto/{idPresupuesto}")]
    public IActionResult AgregarDetallePresupuesto(int idPresupuesto, [FromBody] PresupuestoDetalle presupuestoDetalle)
    {
        if (presupuestos.FirstOrDefault(p => p.IdPresupuesto == idPresupuesto) == null)
        {
            return NotFound($"No se encontró el presupuesto con ID {idPresupuesto}");
        }
        
        if (productos.FirstOrDefault(p => p.IdProducto == presupuestoDetalle.Producto.IdProducto) == null)
        {
            return NotFound("El producto que quiere agregar no existe");
        }

        presupestoRepositorio.AgregarDetalle(idPresupuesto, presupuestoDetalle);

        return Ok($"Detalle agregado al presupuesto {idPresupuesto}");
    }

    [HttpGet("GetPresupuestosCompleto")]
    public IActionResult GetPresupuestosCompleto()
    {
        if (presupuestos.Count() == 0)
        {
            return NotFound("No hay presupuestos");
        }

        return Ok(presupuestos);
    }

    [HttpGet("GetPresupuestosSinDetalles")]
    public IActionResult GetPresupuestosSinDetalles()
    {
        if (presupuestosSinDetalles.Count() == 0)
        {
            return NotFound("No hay presupuestos");
        }

        return Ok(presupuestosSinDetalles);
    }
    
    [HttpGet("ObtenerPresupuesto/{idPresupuesto}")]
    public IActionResult ObtenerPresupuesto(int idPresupuesto)
    {
       if (presupuestos.FirstOrDefault(p => p.IdPresupuesto == idPresupuesto) == null)
        {
            return NotFound($"No se encontró el presupuesto con ID {idPresupuesto}");
        }

        Presupuesto presupuesto = presupestoRepositorio.ObtenerPresupuesto(idPresupuesto);

        return Ok(presupuesto);
    }
    
    
    [HttpGet("ObtenerDetallesDePresupuesto/{idPresupuesto}")]
    public IActionResult ObtenerDetallesDePresupuesto(int idPresupuesto)
    {
       if (presupuestos.FirstOrDefault(p => p.IdPresupuesto == idPresupuesto) == null)
        {
            return NotFound($"No se encontró el presupuesto con ID {idPresupuesto}");
        }

        List<PresupuestoDetalle> detalles = presupestoRepositorio.DetallesPresupuesto(idPresupuesto);

        return Ok(detalles);
    }
    

// _______________________________________________________________________________________
    [HttpDelete("EliminarPresupuesto/{id}")]
    public IActionResult EliminarPresupuesto(int id)
    {
        if (presupuestos.FirstOrDefault(p => p.IdPresupuesto == id) == null)
        {
            return NotFound($"El presupuesto con ID {id} no existe");
        }
        
        presupestoRepositorio.EliminarPresupuesto(id);
        
        return Ok($"Presupuesto eliminado: ID {id}");
    }
}