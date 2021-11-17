using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeterinariaBack.AccesoDatos;
using VeterinariaBack.Dominio;

namespace VeterinariaBack.Servicios
{
    public class ClienteService
    {
        private ClienteDao dao;

        public ClienteService()
        {
            dao = new ClienteDao();
        }

  
        public IList<Clientes> ObtenerTodos()
        {
            return dao.GetAll();
        }

       public IList<Clientes> ConsultarConFiltro(Dictionary<string, object> filtros)
        {
            return dao.GetByFilters(filtros);
        }

        public bool CrearCliente(Clientes oCliente)
        {
            return dao.Create(oCliente);
        }

        public bool ActualizarCliente(Clientes oClienteSelected)
        {
            return dao.Update(oClienteSelected);
        }

        public bool EliminarCliente(Clientes oClienteSelected)
        {
            return dao.Delete(oClienteSelected);
        }

        public object ObtenerCliente(string Cliente)
        {
            return dao.GetUser(Cliente);
        }


    }
}
