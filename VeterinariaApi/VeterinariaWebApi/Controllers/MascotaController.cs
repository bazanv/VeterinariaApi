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
    public class MascotaController : ControllerBase
    {

        private MascotaService service;
        public MascotaController()
        {
            service = new MascotaService();
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
        public IActionResult Crear(Mascotas obj)
        {
            if (obj == null)
            {
                return BadRequest("Ingrese Mascota");
            }

            var resultado = service.Crear(obj);
            if (resultado == false) return BadRequest("Mascota no Creado");
            return Ok(resultado);

        }

        [HttpPost("Actualizar")]
        public IActionResult Actualizar(Mascotas obj)
        {
            if (obj == null)
            {
                return BadRequest("Ingrese Mascota");
            }

            var resultado = service.Actualizar(obj);
            if (resultado == false) return BadRequest("Mascota no Actualizado");
            return Ok(resultado);

        }

        [HttpPost("Eliminar")]
        public IActionResult Eliminar(Mascotas obj)
        {
            if (obj == null)
            {
                return BadRequest("Ingrese Mascota");
            }

            var resultado = service.Eliminar(obj);
            if (resultado == false) return BadRequest("Mascota no Eliminado");
            return Ok(resultado);
        }

        [HttpPost("ObtenerUno")]
        public IActionResult Obtener(string obj)
        {
            if (obj ==null)
            {
                return BadRequest("Ingrese Mascota");
            }

            var resultado = service.Obtener(obj);
            if (resultado == null) return BadRequest("Mascota no Encontrado");
            return Ok(resultado);
        }





    }
}
