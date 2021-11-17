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
    public class ClienteController : ControllerBase
    {

        private ClienteService service;
        public ClienteController()
        {
            service = new ClienteService();
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

      
        [HttpPost("Crear")]
        public IActionResult Crear(Clientes obj)
        {
            if (obj == null)
            {
                return BadRequest("Ingrese Cliente");
            }

            var resultado = service.CrearCliente(obj);
            if (resultado == false) return BadRequest("Cliente no Creado");
            return Ok(resultado);

        }

        [HttpPost("Actualizar")]
        public IActionResult Actualizar(Clientes obj)
        {
            if (obj == null)
            {
                return BadRequest("Ingrese Cliente");
            }

            var resultado = service.ActualizarCliente(obj);
            if (resultado == false) return BadRequest("Cliente no Actualizado");
            return Ok(resultado);

        }

        [HttpPost("Eliminar")]
        public IActionResult Eliminar(Clientes obj)
        {
            if (obj == null)
            {
                return BadRequest("Ingrese Cliente");
            }

            var resultado = service.EliminarCliente(obj);
            if (resultado == false) return BadRequest("Cliente no Eliminado");
            return Ok(resultado);
        }

        [HttpPost("ObtenerUno")]
        public IActionResult Obtener(string obj)
        {
            if (obj ==null)
            {
                return BadRequest("Ingrese Cliente");
            }

            var resultado = service.ObtenerCliente(obj);
            if (resultado == null) return BadRequest("Cliente no Encontrado");
            return Ok(resultado);
        }





    }
}
