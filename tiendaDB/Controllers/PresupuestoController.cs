using Microsoft.AspNetCore.Mvc;
namespace tienda;

[ApiController]
[Route("[controller]")]
public class PresupuestoController : ControllerBase
{
    List<Presupuesto> presupuestos;
    public PresupuestoController()
    {
    }

    [HttpGet("GetPresupuestos")]
    public IActionResult GetProductos()
    {
        return Ok(presupuestos);
    }
}