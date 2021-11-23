using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using veterNetFram.AccesoDatos;
using veterNetFram.Dominio;
using veterNetFram.AccesoDatos;

namespace veterNetFram.Servicios
{
    class TipoService
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

       public IList<Tipos> ConsultarConFiltro(Dictionary<string, object> filtros)
        {
            return dao.GetByFilters(filtros);
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
