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
        return Ok();
    }

    [HttpGet("GetProductos")]
    public IActionResult GetProductos()
    {
        return Ok(productos);
    }
}