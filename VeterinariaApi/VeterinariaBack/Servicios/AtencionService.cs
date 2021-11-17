using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeterinariaBack.AccesoDatos;
using VeterinariaBack.Dominio;


namespace VeterinariaBack.Servicios
{
    public class AtencionService
    {
        private AtencionDao dao;

        public AtencionService()
        {
            dao = new AtencionDao();
        }

  
        public IList<Atenciones> ObtenerTodos()
        {
            return dao.GetAll();
        }

       public IList<Atenciones> ConsultarConFiltro(Dictionary<string, object> filtros)
        {
            return dao.GetByFilters(filtros);
        }

        public bool Crear(Atenciones obj)
        {
            return dao.Create(obj);
        }

        public bool CrearTrans(Atenciones obj)
        {
            return dao.CreateTrans(obj);
        }

        public bool Actualizar(Atenciones obj)
        {
            return dao.Update(obj);
        }

        public bool Eliminar(Atenciones obj)
        {
            return dao.Delete(obj);
        }

        public object Obtener(int filtro)
        {
            return dao.GetOne(filtro);
        }


    }
}
