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
    public class TipoController : ControllerBase
    {

        private TipoService service;
        public TipoController()
        {
            service = new TipoService();
        }



        [HttpGet("ConsultarTodos")]
        public IActionResult ConsultarTodos()
        {

            return Ok(service.ObtenerTodos());
        }


        [HttpPost("ConsultarFiltro")]
        public IActionResult ConsultarFiltro(Dictionary<string, object> lst)
        {
            //if (lst == null )
            //    return BadRequest("Se requiere una descripcion");
            return Ok(service.ConsultarConFiltro(lst));
        }

      
        [HttpPost("Crear")]
        public IActionResult Crear(Tipos obj)
        {
            if (obj == null)
            {
                return BadRequest("Ingrese Tipo");
            }

            var resultado = service.Crear(obj);
            if (resultado == false) return BadRequest("Tipo no Creado");
            return Ok(resultado);

        }

        [HttpPost("Actualizar")]
        public IActionResult Actualizar(Tipos obj)
        {
            if (obj == null)
            {
                return BadRequest("Ingrese Tipo");
            }

            var resultado = service.Actualizar(obj);
            if (resultado == false) return BadRequest("Tipo no Actualizado");
            return Ok(resultado);

        }

        [HttpPost("Eliminar")]
        public IActionResult Eliminar(Tipos obj)
        {
            if (obj == null)
            {
                return BadRequest("Ingrese Tipo");
            }

            var resultado = service.Eliminar(obj);
            if (resultado == false) return BadRequest("Tipo no Eliminado");
            return Ok(resultado);
        }

        [HttpPost("ObtenerUno")]
        public IActionResult Obtener(string obj)
        {
            if (obj ==null)
            {
                return BadRequest("Ingrese Tipo");
            }

            var resultado = service.Obtener(obj);
            if (resultado == null) return BadRequest("Tipo no Encontrado");
            return Ok(resultado);
        }





    }
}
