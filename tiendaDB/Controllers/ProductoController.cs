using Microsoft.AspNetCore.Mvc;
namespace tienda;

[ApiController]
[Route("[controller]")]

/*
Crear un Controlador de Producto(ProductoController) que incluya los endpoints para:
● POST /api/Producto: Permite crear un nuevo Producto.
● GET /api/Producto: Permite listar los Productos existentes.
● PUT /api/Producto/{Id}: Permite modificar un nombre de un Producto.
*/
public class ProductoController : ControllerBase
{
    List<Producto> productos;
    ProductoRepository productoRepositorio = new ProductoRepository();
    public ProductoController()
    {
        productos = productoRepositorio.GetProductos();
    }

    [HttpPost("CrearProducto")]
    public IActionResult CrearProducto([FromBody] Producto producto)
    {
        productoRepositorio.CrearProducto(producto);

        return Ok($"Producto creado con ID {producto.IdProducto}");
    }

    [HttpPut("ModificarProducto/{id}")]
    public IActionResult ModificarProducto(int id, [FromBody]Producto producto)
    {
        if (productos.FirstOrDefault(p => p.IdProducto == id) == null)
        {
            return NotFound($"No se encontró el producto con ID {id}");
        }

        productoRepositorio.ModificarProducto(id, producto);
        
        return Ok($"Producto {id} modificado.");
    }

    [HttpGet("GetProductos")]
    public IActionResult GetProductos()
    {
        if (productos.Count() == 0)
        {
            return NotFound("No hay productos");
        }

        return Ok(productos);
    }
    
    [HttpGet("ObtenerProducto/{id}")]
    public IActionResult ObtenerProducto(int id)
    {
        if (productos.FirstOrDefault(p => p.IdProducto == id) == null)
        {
            return NotFound($"No se encontró el producto con ID {id}");
        }

        Producto producto = productoRepositorio.ObtenerProducto(id);

        return Ok(producto);
    }
    
    [HttpDelete("EliminarProducto/{id}")]
    public IActionResult EliminarProducto(int id)
    {
        if (productos.FirstOrDefault(p => p.IdProducto == id) == null)
        {
            return NotFound($"El producto con ID {id} no existe");
        }
        
        productoRepositorio.EliminarProducto(id);
        
        return Ok($"Producto eliminado: ID {id}");
    }
}