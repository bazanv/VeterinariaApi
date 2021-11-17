using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using veterNetFram.Dominio;

namespace veterNetFram.Servicios
{
    interface IService
    {
        Usuarios ValidarUsuario(string usuario, string password);

        IList<Usuarios> ObtenerTodos();

        IList<Usuarios> ConsultarConFiltro(Dictionary<string, object> filtros);

        bool CrearUsuario(Usuarios oUsuario);
        bool ActualizarUsuario(Usuarios oUsuarioSelected);

         bool EliminarUsuario(Usuarios oUsuarioSelected);

        object ObtenerUsuario(string usuario);


    }
}
