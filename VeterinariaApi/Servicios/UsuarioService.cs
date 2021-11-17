using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using veterNetFram.AccesoDatos;
using veterNetFram.Dominio;

namespace veterNetFram.Servicios
{
    class UsuarioService : IService
    {
        private IUsuarioDao dao;

        public UsuarioService()
        {
            dao = new UsuarioDao();
        }

        public Usuarios ValidarUsuario(string usuario, string password)
        {
            var usr = dao.GetUser(usuario);
            if (usr != null && usr.password.Equals(password))
            {
                return usr;
            }
            return null;
                       
        }

        public IList<Usuarios> ObtenerTodos()
        {
            return dao.GetAll();
        }

       public IList<Usuarios> ConsultarConFiltro(Dictionary<string, object> filtros)
        {
            return dao.GetByFilters(filtros);
        }

        public bool CrearUsuario(Usuarios oUsuario)
        {
            return dao.Create(oUsuario);
        }

        public bool ActualizarUsuario(Usuarios oUsuarioSelected)
        {
            return dao.Update(oUsuarioSelected);
        }

        public bool EliminarUsuario(Usuarios oUsuarioSelected)
        {
            return dao.Delete(oUsuarioSelected);
        }

        public object ObtenerUsuario(string usuario)
        {
            return dao.GetUser(usuario);
        }


    }
}
