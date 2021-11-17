using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using veterNetFram.AccesoDatos;
using veterNetFram.Dominio;


namespace veterNetFram.Servicios
{
    class ProductoService
    {
        private ProductoDao dao;

        public ProductoService()
        {
            dao = new ProductoDao();
        }

  
        public IList<Productos> ObtenerTodos()
        {
            return dao.GetAll();
        }

       public IList<Productos> ConsultarConFiltro(Dictionary<string, object> filtros)
        {
            return dao.GetByFilters(filtros);
        }

        public bool Crear(Productos obj)
        {
            return dao.Create(obj);
        }

        public bool Actualizar(Productos obj)
        {
            return dao.Update(obj);
        }

        public bool Eliminar(Productos obj)
        {
            return dao.Delete(obj);
        }

        public object Obtener(string filtro)
        {
            return dao.GetOne(filtro);
        }


    }
}
