using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeterinariaBack.Dominio;

namespace VeterinariaBack.AccesoDatos
{
    interface IUsuarioDao
    {
        Usuarios GetUser(string usuario);
        IList<Usuarios> GetAll();
        IList<Usuarios> GetByFilters(Dictionary<string, object> parametros);
        bool Delete(Usuarios oUsuario);
        bool Update(Usuarios oUsuario);
        bool Create(Usuarios oUsuario);


    }
}
