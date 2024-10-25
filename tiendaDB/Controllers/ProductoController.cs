using Microsoft.AspNetCore.Mvc;
namespace tienda;

[ApiController]
[Route("[controller]")]
public class ProductoController : ControllerBase
{
    List<Producto> productos;
    ProductoRepository productoRepositorio = new ProductoRepository();
    public ProductoController()
    {
        productos = productoRepositorio.GetProductos();
    }

    [HttpGet("GetProductos")]
    public IActionResult GetProductos()
    {
        return Ok(productos);
    }
}