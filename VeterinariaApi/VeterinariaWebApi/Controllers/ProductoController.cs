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
    public class ProductoController : ControllerBase
    {

        private ProductoService service;
        public ProductoController()
        {
            service = new ProductoService();
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
        public IActionResult Crear(Productos obj)
        {
            if (obj == null)
            {
                return BadRequest("Ingrese Producto");
            }

            var resultado = service.Crear(obj);
            if (resultado == false) return BadRequest("Producto no Creado");
            return Ok(resultado);

        }

        [HttpPost("Actualizar")]
        public IActionResult Actualizar(Productos obj)
        {
            if (obj == null)
            {
                return BadRequest("Ingrese Producto");
            }

            var resultado = service.Actualizar(obj);
            if (resultado == false) return BadRequest("Producto no Actualizado");
            return Ok(resultado);

        }

        [HttpPost("Eliminar")]
        public IActionResult Eliminar(Productos obj)
        {
            if (obj == null)
            {
                return BadRequest("Ingrese Producto");
            }

            var resultado = service.Eliminar(obj);
            if (resultado == false) return BadRequest("Producto no Eliminado");
            return Ok(resultado);
        }

        [HttpPost("ObtenerUno")]
        public IActionResult Obtener(string obj)
        {
            if (obj ==null)
            {
                return BadRequest("Ingrese Producto");
            }

            var resultado = service.Obtener(obj);
            if (resultado == null) return BadRequest("Producto no Encontrado");
            return Ok(resultado);
        }





    }
}
