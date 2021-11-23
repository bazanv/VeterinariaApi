using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using veterNetFram.Dominio;

namespace veterNetFram.AccesoDatos
{
    class TipoDao 
    {
       

        private Tipos ObjectMapping(System.Data.DataRow row)
        {
            Tipos obj = new Tipos();
            obj.id_tipo = Convert.ToInt32(row[0].ToString());
            obj.t_descrip = row[1].ToString();        
            obj.borrado = Convert.ToBoolean(row[2].ToString());      

            return obj;
        }

        public IList<Tipos> GetAll()
        {
            List<Tipos> listado = new List<Tipos>();

            String strSql = string.Concat(" select id_tipo as id, t_descrip as descripcion, borrado  from Tipos order by borrado, descripcion ");


            var resultadoConsulta = ClienteSingleton.GetInstance().ConsultaSQL(strSql);

            foreach (DataRow row in resultadoConsulta.Rows)
            {
                listado.Add(ObjectMapping(row));
            }

            return listado;
        }


        public IList<Tipos> GetByFilters(Dictionary<string, object> parametros)
        {
            List<Tipos> lst = new List<Tipos>();
            String strSql = string.Concat(" select id_tipo as id, t_descrip as descripcion, borrado  from Tipos  ");           

            if (parametros.ContainsKey("descripcion"))
                strSql += " where (t_descrip LIKE '%' + @descripcion + '%') ";

            strSql += " order by borrado, t_descrip  ";

            var resultado = ClienteSingleton.GetInstance().ConsultaSQL(strSql, parametros);

            
            foreach (DataRow row in resultado.Rows)
                lst.Add(ObjectMapping(row));

            return lst;
        }


       public bool Create(Tipos obj)
        {
            string str_sql = "     INSERT INTO Tipos (t_descrip, borrado)" +
                             "     VALUES (@descripcion, 0)";

            var parametros = new Dictionary<string, object>();
            parametros.Add("descripcion", obj.t_descrip);

            // Si una fila es afectada por la inserción retorna TRUE. Caso contrario FALSE
            return (ClienteSingleton.GetInstance().EjecutarSQL(str_sql, parametros) == 1);
        }

        public bool Update(Tipos obj)
        {
            string str_sql = " 	UPDATE Tipos" +
                "  	SET t_descrip = @descripcion " +
                "	WHERE id_tipo =@id_tipo ";

            var parametros = new Dictionary<string, object>();
            parametros.Add("descripcion", obj.t_descrip);
            parametros.Add("id_tipo", obj.id_tipo);

            return (ClienteSingleton.GetInstance().EjecutarSQL(str_sql, parametros) == 1);

        }

        public bool Delete(Tipos obj)
        {
            string str_sql = " UPDATE Tipos" +
                "  SET borrado = 1" +
                "  WHERE  id_tipo =@id_tipo ";
            var parametros = new Dictionary<string, object>();
            parametros.Add("id_tipo", obj.id_tipo);


            return (ClienteSingleton.GetInstance().EjecutarSQL(str_sql, parametros) == 1);

        }

        public Tipos GetOne(string nombre)
        {
            //Construimos la consulta sql para buscar el Cliente en la base de datos.
            String strSql = string.Concat(" select id_tipo as id, t_descrip as descripcion, borrado   from Tipos where t_descrip=@descripcion ");
          

            var parametros = new Dictionary<string, object>();
            parametros.Add("descripcion", nombre);
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
