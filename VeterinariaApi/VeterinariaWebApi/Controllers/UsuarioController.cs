using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VeterinariaBack.Dominio;
using VeterinariaBack.Servicios;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VeterinariaWebApi.Controllers
{
    

    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private UsuarioService service;
        public UsuarioController()
        {
            service = new UsuarioService();
        }


        [HttpGet("ValidarUsuario")]
        public IActionResult ValidarUsuario(string usuario, string password)
        {
            var resultado = service.ValidarUsuario(usuario, password);
            if (resultado == null) return BadRequest("Usuario no encontrado");
            return Ok(resultado);
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

        // POST api/<UsuarioController>
        [HttpPost("CrearUsuario")]
        public IActionResult CrearUsuario(Usuarios oUsuario)
        {
            if (oUsuario == null)
            {
                return BadRequest("Ingrese Usuario");
            }

            var resultado = service.CrearUsuario(oUsuario);
            if (resultado == false) return BadRequest("Usuario no Creado");
            return Ok(resultado);
            
        }

        [HttpPost("ActualizarUsuario")]
        public IActionResult ActualizarUsuario(Usuarios oUsuario)
        {
            if (oUsuario == null)
            {
                return BadRequest("Ingrese Usuario");
            }

            var resultado = service.ActualizarUsuario(oUsuario);
            if (resultado == false) return BadRequest("Usuario no Actualizado");
            return Ok(resultado);

        }

        [HttpPost("EliminarUsuario")]
        public IActionResult EliminarUsuario(Usuarios oUsuario)
        {
            if (oUsuario == null)
            {
                return BadRequest("Ingrese Usuario");
            }

            var resultado = service.EliminarUsuario(oUsuario);
            if (resultado == false) return BadRequest("Usuario no Eliminado");
            return Ok(resultado);
        }

        [HttpPost("ObtenerUsuario")]
        public IActionResult ObtenerUsuario(string usuario)
        {
            //if (usuario == null)
            //{
            //    return BadRequest("Ingrese Usuario");
            //}

            var resultado = service.ObtenerUsuario(usuario);
            if (resultado ==null) return BadRequest("Usuario no Encontrado");
            return Ok(resultado);
        }










        //// PUT api/<UsuarioController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<UsuarioController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}

        // GET: api/<UsuarioController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<UsuarioController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

    }
}
