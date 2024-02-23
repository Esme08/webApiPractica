using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using intento2.Models;
using Microsoft.EntityFrameworkCore;

namespace intento2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class equiposController : ControllerBase
    {
        private readonly equiposContext _equiposContext;
        public equiposController(equiposContext equiposContext)
        {
            _equiposContext = equiposContext;
        }
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            try
            {

                List<Equipos> listadoEquipos = (from e in _equiposContext.Equipos
                                                select e).ToList();
                if (listadoEquipos.Count == 0)
                {
                    return NotFound();
                }
                return Ok(listadoEquipos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
