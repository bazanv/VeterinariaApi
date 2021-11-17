using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeterinariaBack.Dominio;

namespace VeterinariaBack.AccesoDatos
{
    class ProductoDao
    {
       

        private Productos ObjectMapping(System.Data.DataRow row)
        {
            Productos obj = new Productos();
            obj.id_producto = Convert.ToInt32(row[0].ToString());
            obj.p_nombre = row[1].ToString();   
            obj.precio= Convert.ToInt32(row[2].ToString());
            obj.borrado = Convert.ToBoolean(row[3].ToString());      

            return obj;
        }

        public IList<Productos> GetAll()
        {
            List<Productos> listado = new List<Productos>();

            String strSql = string.Concat(" select id_producto, p_nombre, precio, borrado from Productos order by borrado, p_nombre ");


            var resultadoConsulta = ClienteSingleton.GetInstance().ConsultaSQL(strSql);

            foreach (DataRow row in resultadoConsulta.Rows)
            {
                listado.Add(ObjectMapping(row));
            }

            return listado;
        }


        public IList<Productos> GetByFilters(Dictionary<string, object> parametros)
        {
            List<Productos> lst = new List<Productos>();
            String strSql = string.Concat(" select id_producto, p_nombre, precio, borrado from Productos  ");           

            if (parametros.ContainsKey("nombre"))
                strSql += " where (p_nombre LIKE '%' + @nombre + '%') ";

            strSql += " order by borrado, p_nombre  ";

            var resultado = ClienteSingleton.GetInstance().ConsultaSQL(strSql, parametros);

            
            foreach (DataRow row in resultado.Rows)
                lst.Add(ObjectMapping(row));

            return lst;
        }


       public bool Create(Productos obj)
        {
            string str_sql = "     INSERT INTO Productos (p_nombre, precio, borrado) " +
                             "     VALUES (@p_nombre, @precio, 0)";

            var parametros = new Dictionary<string, object>();
            parametros.Add("p_nombre", obj.p_nombre);
            parametros.Add("precio", obj.precio);

            // Si una fila es afectada por la inserción retorna TRUE. Caso contrario FALSE
            return (ClienteSingleton.GetInstance().EjecutarSQL(str_sql, parametros) == 1);
        }

        public bool Update(Productos obj)
        {
            string str_sql = " 	UPDATE Productos" +
                "  	SET p_nombre = @p_nombre, " +
                "   SET precio=@precio "+
                "	WHERE id_producto =@id_producto ";

            var parametros = new Dictionary<string, object>();
            parametros.Add("p_nombre", obj.p_nombre);
            parametros.Add("precio", obj.precio);
            parametros.Add("id_product", obj.id_producto);

            return (ClienteSingleton.GetInstance().EjecutarSQL(str_sql, parametros) == 1);

        }

        public bool Delete(Productos obj)
        {
            string str_sql = " UPDATE Tipos" +
                "  SET borrado = 1 " +
                "	WHERE id_producto =@id_producto ";
            var parametros = new Dictionary<string, object>();
            parametros.Add("id_product", obj.id_producto);


            return (ClienteSingleton.GetInstance().EjecutarSQL(str_sql, parametros) == 1);

        }

        public Productos GetOne(string nombre)
        {
            //Construimos la consulta sql para buscar el Cliente en la base de datos.
            String strSql = string.Concat(" select id_producto, p_nombre, precio, borrado from Productos  where p_nombre=@nombre ");
          

            var parametros = new Dictionary<string, object>();
            parametros.Add("nombre", nombre);
            //Usando el método GetDBHelper obtenemos la instancia unica de DBHelper (Patrón Singleton) y ejecutamos el método ConsultaSQL()
            var resultado = ClienteSingleton.GetInstance().ConsultaSQL(strSql, parametros);

            // Validamos que el resultado tenga al menos una fila.
            if (resultado.Rows.Count > 0)
            {
                return ObjectMapping(resultado.Rows[0]);
            }

            return null;
        }


    }
}
