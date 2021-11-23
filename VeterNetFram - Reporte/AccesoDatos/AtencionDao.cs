using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using veterNetFram.Dominio;

namespace veterNetFram.AccesoDatos
{
    class AtencionDao 
    {
        DetalleDao daoDet = new DetalleDao();

        private Atenciones ObjectMapping(System.Data.DataRow row)
        {
            Atenciones obj = new Atenciones();
            obj.id_atencion = Convert.ToInt32(row[0].ToString());
            obj.fecha = Convert.ToDateTime(row[1]);
            obj.a_descrip= row[2].ToString();
            obj.importe= Convert.ToDouble(row[3].ToString());
            obj.borrado = Convert.ToBoolean(row[4].ToString());

            Mascotas ax1 = new Mascotas();
            ax1.id_mascota= Convert.ToInt32(row[5].ToString());
            ax1.m_nombre = row[6].ToString();
            obj.mascota = ax1;

            Usuarios ax2 = new Usuarios();
            ax2.id_usuario= Convert.ToInt32(row[7].ToString());
            ax2.usuario= row[8].ToString();
            obj.usuario = ax2;

            Clientes ax3 = new Clientes();
            ax3.id_cliente = Convert.ToInt32(row[9].ToString());
            ax3.c_nombre= row[10].ToString();
            obj.mascota.cliente = ax3;


            return obj;
        }

        public IList<Atenciones> GetAll()
        {
            List<Atenciones> listado = new List<Atenciones>();

            String strSql = string.Concat(" select a.id_atencion, a.fecha, a.a_descrip, a.importe, a.borrado, m.id_mascota, m.m_nombre, u.id_usuario, u.usuario, c.id_cliente, c.c_nombre from Atencion a join Mascotas m on a.id_mascota = m.id_mascota join Usuarios u on u.id_usuario = a.id_usuario join clientes c on c.id_cliente=m.id_cliente order by a.borrado, a.fecha desc  ");



            var resultadoConsulta = ClienteSingleton.GetInstance().ConsultaSQL(strSql);

            foreach (DataRow row in resultadoConsulta.Rows)
            {
                listado.Add(ObjectMapping(row));
            }

            return listado;
        }


        public IList<Atenciones> GetByFilters(Dictionary<string, object> parametros)
        {
            List<Atenciones> lst = new List<Atenciones>();
            String strSql = string.Concat(" select a.id_atencion, a.fecha, a.a_descrip, a.importe, a.borrado, m.id_mascota, m.m_nombre, u.id_usuario, u.usuario, c.id_cliente, c.c_nombre from Atencion a join Mascotas m on a.id_mascota = m.id_mascota join Usuarios u on u.id_usuario = a.id_usuario join clientes c on c.id_cliente=m.id_cliente  ");          

            if (parametros.ContainsKey("mascota"))
                strSql += " where (m.m_nombre LIKE '%' + @mascota + '%') ";

            strSql += " order by a.borrado, a.fecha desc    ";

            var resultado = ClienteSingleton.GetInstance().ConsultaSQL(strSql, parametros);

            foreach (DataRow row in resultado.Rows)
                lst.Add(ObjectMapping(row));

            return lst;
        }


       public bool Create(Atenciones obj)
        {
            string str_sql = "     INSERT INTO Atencion (fecha, a_descrip, importe, id_mascota, id_usuario, borrado)" +
                             "     VALUES (@fecha, @a_descrip, @importe, @id_mascota, @id_usuario, 0)";

            var parametros = new Dictionary<string, object>();
            parametros.Add("fecha", obj.fecha);
            parametros.Add("a_descrip", obj.a_descrip);
            parametros.Add("importe", obj.importe);
            parametros.Add("id_mascota", obj.mascota.id_mascota);
            parametros.Add("id_usuario", obj.usuario.id_usuario);

            if(ClienteSingleton.GetInstance().EjecutarSQL(str_sql, parametros) == 1)
            {
                string max_id = " select max(id_atencion) from atencion ";
                var idAtencion =(int) ClienteSingleton.GetInstance().ConsultaSQLScalar(max_id);
       
                foreach(Detalles det in obj.detalle)
                {    
                    det.id_atencion = idAtencion;
                    daoDet.Create(det);
                }
                return true;
            }

            return false;
        }

        public bool Update(Atenciones obj)
        {
            string str_sql = " 	UPDATE Atencion" +
                "  	SET fecha = @fecha," +
                "  	a_descrip=@a_descrip, " +
                "   importe=@importe, " +
                "   id_mascota=@id_mascota, " +
                "   id_usuario=@id_usuario  " +
                "	WHERE id_atencion =@id_atencion  ";

            var parametros = new Dictionary<string, object>();
            parametros.Add("fecha", obj.fecha);
            parametros.Add("a_descrip", obj.a_descrip);
            parametros.Add("importe", obj.importe);
            parametros.Add("id_mascota", obj.mascota.id_mascota);
            parametros.Add("id_usuario", obj.usuario.id_usuario);
            parametros.Add("id_atencion", obj.id_atencion);

            if (ClienteSingleton.GetInstance().EjecutarSQL(str_sql, parametros) == 1)
            {

                foreach (Detalles det in obj.detalle)
                {
                    daoDet.Update(det);
                }
                return true;
            }

            return false;


        }

        public bool Delete(Atenciones obj)
        {
            string str_sql = " UPDATE Atencion" +
                "  SET borrado = 1" +
                "  WHERE id_atencion =@id_atencion";
            var parametros = new Dictionary<string, object>();
            parametros.Add("id_atencion", obj.id_atencion);

            if (ClienteSingleton.GetInstance().EjecutarSQL(str_sql, parametros) == 1)
            {

                foreach (Detalles det in obj.detalle)
                {
                    daoDet.Create(det);
                }
                return true;
            }

            return false;

        }


        public Atenciones GetOne(int id)
        {
            //Construimos la consulta sql para buscar el Cliente en la base de datos.
            String strSql = string.Concat(" select a.id_atencion, a.fecha, a.a_descrip, a.importe, a.borrado, m.id_mascota, m.m_nombre, u.id_usuario, u.usuario, c.id_cliente, c.c_nombre from Atencion a join Mascotas m on a.id_mascota = m.id_mascota join Usuarios u on u.id_usuario = a.id_usuario join clientes c on c.id_cliente=m.id_cliente  ");


            var parametros = new Dictionary<string, object>();
            parametros.Add("id_atencion", id);
            //Usando el método GetDBHelper obtenemos la instancia unica de DBHelper (Patrón Singleton) y ejecutamos el método ConsultaSQL()
            var resultado = ClienteSingleton.GetInstance().ConsultaSQL(strSql, parametros);

            // Validamos que el resultado tenga al menos una fila.
            if (resultado.Rows.Count > 0)
            {
                Atenciones a = new Atenciones();
                a= ObjectMapping(resultado.Rows[0]);

                //List<Detalles> aux= new List<Detalles>();
                var aux= daoDet.GetByFilters(id);
                foreach (Detalles d in aux)
                {
                    a.detalle.Add(d);
                }

                return a;
            }

            return null;
        }


    }
}
