using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeterinariaBack.Dominio;

namespace VeterinariaBack.AccesoDatos
{
    class ClienteDao 
    {
       

        private Clientes ObjectMapping(System.Data.DataRow row)
        {
            Clientes oCliente = new Clientes();
            oCliente.id_cliente = Convert.ToInt32(row[0].ToString());
            oCliente.c_nombre = row[1].ToString();
            oCliente.sexo= Convert.ToBoolean(row[2].ToString());
            oCliente.borrado = Convert.ToBoolean(row[3].ToString());      

            return oCliente;
        }

        public IList<Clientes> GetAll()
        {
            List<Clientes> listado = new List<Clientes>();

            String strSql = string.Concat(" select id_cliente as id, c_nombre as nombre ,sexo, borrado  from Clientes order by borrado,nombre ");



            var resultadoConsulta = ClienteSingleton.GetInstance().ConsultaSQL(strSql);

            foreach (DataRow row in resultadoConsulta.Rows)
            {
                listado.Add(ObjectMapping(row));
            }

            return listado;
        }


        public IList<Clientes> GetByFilters(Dictionary<string, object> parametros)
        {
            List<Clientes> lst = new List<Clientes>();
            String strSql = string.Concat(" select id_cliente as id, c_nombre as nombre ,sexo, borrado  from Clientes  ");           

            if (parametros.ContainsKey("cliente"))
                strSql += " where (c_nombre LIKE '%' + @cliente + '%') ";

            strSql += " order by borrado, nombre  ";

            var resultado = ClienteSingleton.GetInstance().ConsultaSQL(strSql, parametros);

            foreach (DataRow row in resultado.Rows)
                lst.Add(ObjectMapping(row));

            return lst;
        }


       public bool Create(Clientes oCliente)
        {
            string str_sql = "     INSERT INTO Clientes (c_nombre, sexo, borrado)" +
                             "     VALUES (@nombre, @sexo, 0)";

            var parametros = new Dictionary<string, object>();
            parametros.Add("nombre", oCliente.c_nombre);
            parametros.Add("sexo", oCliente.sexo);

            // Si una fila es afectada por la inserción retorna TRUE. Caso contrario FALSE
            return (ClienteSingleton.GetInstance().EjecutarSQL(str_sql, parametros) == 1);
        }

        public bool Update(Clientes oCliente)
        {
            string str_sql = " 	UPDATE Clientes" +
                "  	SET c_nombre = @nombre," +
                "  	sexo=@sexo " +
                "	WHERE id_Cliente =@id_Cliente";

            var parametros = new Dictionary<string, object>();
            parametros.Add("nombre", oCliente.c_nombre);
            parametros.Add("sexo", oCliente.sexo);
            parametros.Add("id_Cliente", oCliente.id_cliente);

            return (ClienteSingleton.GetInstance().EjecutarSQL(str_sql, parametros) == 1);

        }

        public bool Delete(Clientes oCliente)
        {
            string str_sql = " UPDATE Clientes" +
                "  SET borrado = 1" +
                "  WHERE id_cliente = @id_cliente";
            var parametros = new Dictionary<string, object>();
            parametros.Add("id_cliente", oCliente.id_cliente);


            return (ClienteSingleton.GetInstance().EjecutarSQL(str_sql, parametros) == 1);

        }

        public Clientes GetCliente(string u)
        {
            String strSql = string.Concat(" select id_cliente as id, c_nombre as nombre ,sexo, borrado  from Clientes where borrado=0 and c_nombre=@Cliente ");

            var parametros = new Dictionary<string, object>();
            parametros.Add("Cliente", u);
            //obtenemos la instancia unica de (Patrón Singleton) y ejecutamos el método ConsultaSQL()
            var resultado = ClienteSingleton.GetInstance().ConsultaSQL(strSql, parametros);

            // Validamos que el resultado tenga al menos una fila.
            if (resultado.Rows.Count > 0)
            {
                return ObjectMapping(resultado.Rows[0]);
            }

            return null;
        }
        public Clientes GetUser(string nombreCliente)
        {
            //Construimos la consulta sql para buscar el Cliente en la base de datos.
            String strSql = string.Concat(" select id_cliente as id, c_nombre as nombre ,sexo, borrado   from Clientes where c_nombre=@Cliente ");
          

            var parametros = new Dictionary<string, object>();
            parametros.Add("Cliente", nombreCliente);
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
