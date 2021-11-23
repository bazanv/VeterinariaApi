using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using veterNetFram.Dominio;

namespace veterNetFram.AccesoDatos
{
    class UsuarioDao : IUsuarioDao
    {
       

        private Usuarios ObjectMapping(System.Data.DataRow row)
        {
            Usuarios oUsuario = new Usuarios();
            oUsuario.id_usuario = Convert.ToInt32(row[0].ToString());
            oUsuario.usuario = row[1].ToString();
            oUsuario.password= row[2].ToString();
            oUsuario.email = row[3].ToString();

            oUsuario.borrado=  Convert.ToBoolean(row[4].ToString());
           // oUsuario.borrado = (Boolean)row[4];
            //oUsuario.borrado = row.Table.Columns.Contains("borrado") ? row[3].ToString() : null;

            return oUsuario;
        }

        public IList<Usuarios> GetAll()
        {
            List<Usuarios> listado = new List<Usuarios>();

            String strSql = string.Concat(" select id_usuario as id, usuario,[password],email, borrado  from Usuarios order by borrado,usuario ");
            

            var resultadoConsulta = ClienteSingleton.GetInstance().ConsultaSQL(strSql);

            foreach (DataRow row in resultadoConsulta.Rows)
            {
                listado.Add(ObjectMapping(row));
            }

            return listado;
        }

        public IList<Usuarios> GetByFilters(Dictionary<string, object> parametros)
        {
            List<Usuarios> lst = new List<Usuarios>();
            String strSql = string.Concat(" select id_usuario as id, usuario,[password],email, borrado  from Usuarios  ");           

            if (parametros.ContainsKey("usuario"))
                strSql += " where (usuario LIKE '%' + @usuario + '%') ";

            strSql += " order by borrado, usuario  ";

            var resultado = ClienteSingleton.GetInstance().ConsultaSQL(strSql, parametros);

            foreach (DataRow row in resultado.Rows)
                lst.Add(ObjectMapping(row));

            return lst;
        }


       public bool Create(Usuarios oUsuario)
        {
            string str_sql = "     INSERT INTO Usuarios (usuario, password, email, borrado)" +
                             "     VALUES (@usuario, @password, @email, 0)";

            var parametros = new Dictionary<string, object>();
            parametros.Add("usuario", oUsuario.usuario);
            parametros.Add("password", oUsuario.password);
            parametros.Add("email", oUsuario.email);



            // Si una fila es afectada por la inserción retorna TRUE. Caso contrario FALSE
            return (ClienteSingleton.GetInstance().EjecutarSQL(str_sql, parametros) == 1);
        }

        public bool Update(Usuarios oUsuario)
        {
            string str_sql = " 	UPDATE Usuarios" +
                "  	SET usuario = @usuario," +
                "  	password= @password," +
                "  	email=@email " +
                "	WHERE id_usuario =@id_usuario";

            var parametros = new Dictionary<string, object>();
            parametros.Add("usuario", oUsuario.usuario);
            parametros.Add("password", oUsuario.password);
            parametros.Add("email", oUsuario.email);
            parametros.Add("id_usuario", oUsuario.id_usuario);

            return (ClienteSingleton.GetInstance().EjecutarSQL(str_sql, parametros) == 1);

        }

        public bool Delete(Usuarios oUsuario)
        {
            string str_sql = " UPDATE Usuarios" +
                "  SET borrado = 1" +
                "  WHERE id_usuario = @id_Usuario";
            var parametros = new Dictionary<string, object>();
            parametros.Add("id_Usuario", oUsuario.id_usuario);


            return (ClienteSingleton.GetInstance().EjecutarSQL(str_sql, parametros) == 1);

        }

        public Usuarios GetUsuario(string u)
        {
            String strSql = string.Concat(" select id_usuario as id, usuario,[password],email, borrado  from Usuarios where borrado=0 and usuario=@usuario ");

            var parametros = new Dictionary<string, object>();
            parametros.Add("usuario", u);
            //obtenemos la instancia unica de (Patrón Singleton) y ejecutamos el método ConsultaSQL()
            var resultado = ClienteSingleton.GetInstance().ConsultaSQL(strSql, parametros);

            // Validamos que el resultado tenga al menos una fila.
            if (resultado.Rows.Count > 0)
            {
                return ObjectMapping(resultado.Rows[0]);
            }

            return null;
        }
        public Usuarios GetUser(string nombreUsuario)
        {
            //Construimos la consulta sql para buscar el usuario en la base de datos.
            String strSql = string.Concat(" select id_usuario as id, usuario,[password],email, borrado  from Usuarios where usuario=@usuario ");
          

            var parametros = new Dictionary<string, object>();
            parametros.Add("usuario", nombreUsuario);
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
