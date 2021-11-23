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
    class MascotaService
    {
        private MascotaDao dao;

        public MascotaService()
        {
            dao = new MascotaDao();
        }

  
        public IList<Mascotas> ObtenerTodos()
        {
            return dao.GetAll();
        }

       public IList<Mascotas> ConsultarConFiltro(Dictionary<string, object> filtros)
        {
            return dao.GetByFilters(filtros);
        }

        public bool Crear(Mascotas obj)
        {
            return dao.Create(obj);
        }

        public bool Actualizar(Mascotas obj)
        {
            return dao.Update(obj);
        }

        public bool Eliminar(Mascotas obj)
        {
            return dao.Delete(obj);
        }

        public object Obtener(string filtro)
        {
            return dao.GetOne(filtro);
        }

        public IList<Mascotas> MdeCli(string id)
        {
            return dao.GetMdC(id);
        }


    }
}
