using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeterinariaBack.Dominio;

namespace VeterinariaBack.AccesoDatos
{
    class MascotaDao 
    {
       

        private Mascotas ObjectMapping(System.Data.DataRow row)
        {
            Mascotas obj = new Mascotas();
            obj.id_mascota = Convert.ToInt32(row[0].ToString());
            obj.m_nombre = row[1].ToString();
            obj.edad= Convert.ToInt32(row[2].ToString());
            obj.borrado = Convert.ToBoolean(row[3].ToString());

            Tipos ax1 = new Tipos();
            ax1.id_tipo= Convert.ToInt32(row[4].ToString());
            ax1.t_descrip = row[5].ToString();
            obj.tipo = ax1;

            Clientes ax2 = new Clientes();
            ax2.id_cliente= Convert.ToInt32(row[6].ToString());
            ax2.sexo= Convert.ToBoolean(row[7].ToString());
            ax2.c_nombre = row[8].ToString();
            obj.cliente = ax2;

            return obj;
        }

        public IList<Mascotas> GetAll()
        {
            List<Mascotas> listado = new List<Mascotas>();

            String strSql = string.Concat(" select m.id_mascota as id, m.m_nombre as mascota, m.edad, m.borrado, t.id_tipo, t_descrip as animal, c.id_cliente, c.sexo, c.c_nombre as dueño from mascotas m join clientes c on m.id_cliente=c.id_cliente join tipos t on t.id_tipo=m.id_tipo order by m.borrado, mascota ");



            var resultadoConsulta = ClienteSingleton.GetInstance().ConsultaSQL(strSql);

            foreach (DataRow row in resultadoConsulta.Rows)
            {
                listado.Add(ObjectMapping(row));
            }

            return listado;
        }


        public IList<Mascotas> GetByFilters(Dictionary<string, object> parametros)
        {
            List<Mascotas> lst = new List<Mascotas>();
            String strSql = string.Concat(" select m.id_mascota as id, m.m_nombre as mascota, m.edad, m.borrado, t.id_tipo, t_descrip as animal, c.id_cliente, c.sexo, c.c_nombre as dueño from mascotas m join clientes c on m.id_cliente=c.id_cliente join tipos t on t.id_tipo=m.id_tipo   ");           

            if (parametros.ContainsKey("mascota"))
                strSql += " where (m.m_nombre LIKE '%' + @mascota + '%') ";

            strSql += " order by m.borrado, mascota  ";

            var resultado = ClienteSingleton.GetInstance().ConsultaSQL(strSql, parametros);

            foreach (DataRow row in resultado.Rows)
                lst.Add(ObjectMapping(row));

            return lst;
        }


       public bool Create(Mascotas obj)
        {
            string str_sql = "     INSERT INTO Mascotas (m_nombre, edad, id_tipo, id_cliente, borrado)" +
                             "     VALUES (@nombre, @edad, @id_tipo, @id_cliente, 0)";

            var parametros = new Dictionary<string, object>();
            parametros.Add("nombre", obj.m_nombre);
            parametros.Add("edad", obj.edad);
            parametros.Add("id_tipo", obj.tipo.id_tipo);
            parametros.Add("id_cliente", obj.cliente.id_cliente);

            // Si una fila es afectada por la inserción retorna TRUE. Caso contrario FALSE
            return (ClienteSingleton.GetInstance().EjecutarSQL(str_sql, parametros) == 1);
        }

        public bool Update(Mascotas obj)
        {
            string str_sql = " 	UPDATE Mascotas" +
                "  	SET m_nombre = @nombre," +
                "  	edad=@edad, " +
                "   id_tipo=@id_tipo, "+
                "   id_cliente= @id_cliente "+
                "	WHERE id_mascota =@id_mascota ";

            var parametros = new Dictionary<string, object>();
            parametros.Add("nombre", obj.m_nombre);
            parametros.Add("edad", obj.edad);
            parametros.Add("id_tipo", obj.tipo.id_tipo);
            parametros.Add("id_cliente", obj.cliente.id_cliente);
            parametros.Add("id_mascota", obj.id_mascota);

            return (ClienteSingleton.GetInstance().EjecutarSQL(str_sql, parametros) == 1);

        }

        public bool Delete(Mascotas obj)
        {
            string str_sql = " UPDATE Mascotas" +
                "  SET borrado = 1" +
                "  WHERE id_mascota =@id_mascota ";
            var parametros = new Dictionary<string, object>();
            parametros.Add("id_mascota", obj.id_mascota);


            return (ClienteSingleton.GetInstance().EjecutarSQL(str_sql, parametros) == 1);

        }

      
        public Mascotas GetOne(string cadena)
        {
            //Construimos la consulta sql para buscar el Cliente en la base de datos.
            String strSql = string.Concat(" select m.id_mascota as id, m.m_nombre as mascota, m.edad, m.borrado, t.id_tipo, t_descrip as animal, c.id_cliente, c.sexo, c.c_nombre as dueño from mascotas m join clientes c on m.id_cliente=c.id_cliente join tipos t on t.id_tipo=m.id_tipo   where m.m_nombre=@mascota ");
          

            var parametros = new Dictionary<string, object>();
            parametros.Add("mascota", cadena);
            //Usando el método GetDBHelper obtenemos la instancia unica de DBHelper (Patrón Singleton) y ejecutamos el método ConsultaSQL()
            var resultado = ClienteSingleton.GetInstance().ConsultaSQL(strSql, parametros);

            // Validamos que el resultado tenga al menos una fila.
            if (resultado.Rows.Count > 0)
            {
                return ObjectMapping(resultado.Rows[0]);
            }

            return null;
        }

        public IList<Mascotas> GetMdC(string cli)
        {
            List<Mascotas> listado = new List<Mascotas>();
            String strSql = string.Concat(" select m.id_mascota as id, m.m_nombre as mascota, m.edad, m.borrado, t.id_tipo, t_descrip as animal, c.id_cliente, c.sexo, c.c_nombre as dueño from mascotas m join clientes c on m.id_cliente = c.id_cliente join tipos t on t.id_tipo = m.id_tipo   where c.c_nombre = @c_nombre ");


            var parametros = new Dictionary<string, object>();
            parametros.Add("c_nombre", cli);
            //Usando el método GetDBHelper obtenemos la instancia unica de DBHelper (Patrón Singleton) y ejecutamos el método ConsultaSQL()
            var resultado = ClienteSingleton.GetInstance().ConsultaSQL(strSql, parametros);



            foreach (DataRow row in resultado.Rows)
            {
                listado.Add(ObjectMapping(row));
            }

            return listado;
        }



    }
}
