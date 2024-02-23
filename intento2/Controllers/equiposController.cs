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

        // Método para leer todos los registros
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

        //Método para buscar por Id
        [HttpGet]
        [Route("GetById/{id}")]
        public IActionResult Get(int id)
        {
            try
            {

                Equipos? equipo = (from e in _equiposContext.Equipos where e.id_equipos == id select e).FirstOrDefault();
                if (equipo == null)
                {
                    return NotFound();
                }
                return Ok(equipo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Método para buscar por descripción
        [HttpGet]
        [Route("Find/{filtro}")]
        public IActionResult FindByDescription(string filtro)
        {
            try
            {

                Equipos? equipo = (from e in _equiposContext.Equipos
                                   where e.descripcion.Contains(filtro)
                                   select e).FirstOrDefault();

                if (equipo == null)
                {
                    return NotFound();
                }
                return Ok(equipo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Método para crear registros

        [HttpPost]
        [Route("Add")]

        public IActionResult GuardarEquipo([FromBody] Equipos equipo)
        {
            try
            {
                _equiposContext.Equipos.Add(equipo);
                _equiposContext.SaveChanges();
                return Ok(equipo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Método para actualizar registros

        [HttpPut]
        [Route("actualizar/{id}")]

        public IActionResult ActualizarEquipo(int id, [FromBody] Equipos equipoModificar) 
        {
            try
            {
                /*Para actualizar un registro, se obtiene el registro original de la base de datos 
             al cual alteramos alguna propiedad*/

                Equipos? equipoActual = (from e in _equiposContext.Equipos
                                         where e.id_equipos == id
                                         select e).FirstOrDefault();

                /*Verificamos que el registro exista según ID*/
                if (equipoActual == null)
                {
                    return NotFound();
                }

                /*Si se  encuentra el registro, se alteran los campos modificables*/

                equipoActual.nombre = equipoModificar.nombre;
                equipoActual.descripcion = equipoModificar.descripcion;
                equipoActual.marca_id = equipoModificar.marca_id;
                equipoActual.tipo_equipo_id = equipoModificar.tipo_equipo_id;
                equipoActual.anio_compra = equipoModificar.anio_compra;
                equipoActual.costo = equipoModificar.costo;

                /*Se marca el registro como modificado en el contexto y se envía la modificación a la base de datos*/

                _equiposContext.Entry(equipoActual).State = EntityState.Modified;
                _equiposContext.SaveChanges();

                return Ok(equipoModificar);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);

            }
        }

        //Método para eliminar

        [HttpDelete]
        [Route("eliminar/{id}")]

        public IActionResult EliminarEquipo(int id)
        {
            try
            {
                /*Para actualizar un registro, se obtiene el registro original de la base de datos el cual eliminaremos*/
                Equipos? equipo = (from e in _equiposContext.Equipos
                                   where e.id_equipos == id
                                   select e).FirstOrDefault();

                //Verificamos que exista el registro según su id
                if (equipo == null)
                {
                    return NotFound();
                }

                //Ejecutamos la acción de eliminar el registro 
                _equiposContext.Equipos.Attach(equipo);
                _equiposContext.Equipos.Remove(equipo);
                _equiposContext.SaveChanges();

                return Ok(equipo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); 
            }
        }
    }
}
