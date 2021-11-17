using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeterinariaBack.AccesoDatos;
using VeterinariaBack.Dominio;
using VeterinariaBack.AccesoDatos;

namespace VeterinariaBack.Servicios
{
    public class TipoService
    {
        private TipoDao dao;

        public TipoService()
        {
            dao = new TipoDao();
        }

  
        public IList<Tipos> ObtenerTodos()
        {
            return dao.GetAll();
        }

       public IList<Tipos> ConsultarConFiltro(Dictionary<string, object> fil)
        {
          
            return dao.GetByFilters(fil);
        }

        public bool Crear(Tipos obj)
        {
            return dao.Create(obj);
        }

        public bool Actualizar(Tipos obj)
        {
            return dao.Update(obj);
        }

        public bool Eliminar(Tipos obj)
        {
            return dao.Delete(obj);
        }

        public object Obtener(string filtro)
        {
            return dao.GetOne(filtro);
        }

        


    }
}
