using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VeterinariaBack.Dominio;
using VeterinariaBack.Servicios;

namespace VeterinariaWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AtencionController : ControllerBase
    {

        private AtencionService service;
        public AtencionController()
        {
            service = new AtencionService();
        }



        [HttpGet("ConsultarTodos")]
        public IActionResult ConsultarTodos()
        {

            return Ok(service.ObtenerTodos());
        }


        [HttpPost("ConsultarFiltro")]
        public IActionResult ConsultarFiltro(Dictionary<string, object> lst)
        {
            if (lst == null || lst.Count == 0)
                return BadRequest("Se requiere una lista de parámetros!");
            return Ok(service.ConsultarConFiltro(lst));
        }

        [HttpPost("CrearTrans")]
        public IActionResult CrearTrans(Atenciones obj)
        {
            if (obj == null)
            {
                return BadRequest("Ingrese Atencion");
            }

            var resultado = service.CrearTrans(obj);
            if (resultado == false) return BadRequest("Atencion no Creado");
            return Ok(resultado);

        }

        [HttpPost("Crear")]
        public IActionResult Crear(Atenciones obj)
        {
            if (obj == null)
            {
                return BadRequest("Ingrese Atencion");
            }

            var resultado = service.Crear(obj);
            if (resultado == false) return BadRequest("Atencion no Creado");
            return Ok(resultado);

        }

        [HttpPost("Actualizar")]
        public IActionResult Actualizar(Atenciones obj)
        {
            if (obj == null)
            {
                return BadRequest("Ingrese Atencion");
            }

            var resultado = service.Actualizar(obj);
            if (resultado == false) return BadRequest("Atencion no Actualizado");
            return Ok(resultado);

        }

        [HttpPost("Eliminar")]
        public IActionResult Eliminar(Atenciones obj)
        {
            if (obj == null)
            {
                return BadRequest("Ingrese Atencion");
            }

            var resultado = service.Eliminar(obj);
            if (resultado == false) return BadRequest("Atencion no Eliminado");
            return Ok(resultado);
        }

        [HttpPost("ObtenerUno")]
        public IActionResult Obtener(int obj)
        {
            if (obj <=0)
            {
                return BadRequest("Ingrese ID");
            }

            var resultado = service.Obtener(obj);
            if (resultado == null) return BadRequest("Atencion no Encontrado");
            return Ok(resultado);
        }





    }
}
