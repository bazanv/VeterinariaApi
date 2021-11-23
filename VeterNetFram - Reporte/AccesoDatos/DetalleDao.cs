using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using veterNetFram.Dominio;

namespace veterNetFram.AccesoDatos
{
    class DetalleDao
    {
       

        private Detalles ObjectMapping(System.Data.DataRow row)
        {
            Detalles obj = new Detalles();
            obj.id_detalle = Convert.ToInt32(row[0].ToString());
            obj.cantidad = Convert.ToInt32(row[1]);
            obj.p_facturado= Convert.ToDouble(row[2].ToString());
            obj.borrado = Convert.ToBoolean(row[3].ToString());

            Productos ax1 = new Productos();
            ax1.id_producto= Convert.ToInt32(row[4].ToString());
            ax1.p_nombre = row[5].ToString();
            obj.producto = ax1;

            return obj;
        }

        public IList<Detalles> GetAll()
        {
            List<Detalles> listado = new List<Detalles>();

            String strSql = string.Concat(" select d.id_detalle, d.cantidad, d.p_facturado, d.borrado, p.id_producto, p.p_nombre from Detalles d join Productos p on d.id_producto=p.id_producto order by d.borrado, d.id_producto    ");



            var resultadoConsulta = ClienteSingleton.GetInstance().ConsultaSQL(strSql);

            foreach (DataRow row in resultadoConsulta.Rows)
            {
                listado.Add(ObjectMapping(row));
            }

            return listado;
        }


        public IList<Detalles> GetByFilters(int id)
        {
            List<Detalles> lst = new List<Detalles>();
            String strSql = string.Concat(" select d.id_detalle, d.cantidad, d.p_facturado, d.borrado, p.id_producto, p.p_nombre from Detalles d join Productos p on d.id_producto=p.id_producto where d.id_atencion =@id_atencion order by d.borrado, d.id_producto    ");

            var parametros = new Dictionary<string, object>();
            parametros.Add("id_atencion", id);

            var resultado = ClienteSingleton.GetInstance().ConsultaSQL(strSql, parametros);

            foreach (DataRow row in resultado.Rows)
                lst.Add(ObjectMapping(row));

            return lst;
        }


       public bool Create(Detalles obj)
        {
            string str_sql = "     INSERT INTO Detalles (id_producto, cantidad, p_facturado, id_atencion, borrado)" +
                             "     VALUES (@id_producto, @cantidad, @p_facturado, @id_atencion, 0) ";

            var parametros = new Dictionary<string, object>();
            parametros.Add("id_producto", obj.producto.id_producto);
            parametros.Add("cantidad", obj.cantidad);
            parametros.Add("p_facturado", obj.p_facturado);
            parametros.Add("id_atencion", obj.id_atencion);


            return (ClienteSingleton.GetInstance().EjecutarSQL(str_sql, parametros) == 1);
           
        }

        public bool Update(Detalles obj)
        {
            string str_sql = " 	UPDATE Detalles" +
                "  	SET id_producto = @id_producto," +
                "  	cantidad=@cantidad, " +
                "   p_facturado=@p_facturado " +

                "	WHERE id_detalle =@id_detalle  ";

            var parametros = new Dictionary<string, object>();
            parametros.Add("id_producto", obj.producto.id_producto);
            parametros.Add("cantidad", obj.cantidad);
            parametros.Add("p_facturado", obj.p_facturado);
           // parametros.Add("id_atencion", obj.id_atencion);
            parametros.Add("id_detalle", obj.id_detalle);
            //                 "   id_atencion=@id_atencion " +

            return (ClienteSingleton.GetInstance().EjecutarSQL(str_sql, parametros) == 1);

        }

        public bool Delete(Detalles obj)
        {
            string str_sql = " UPDATE detalles" +
                "  SET borrado = 1" +
                "  WHERE id_detalle =@id_detalle  ";
            var parametros = new Dictionary<string, object>();
            parametros.Add("id_detalle", obj.id_detalle);


            return (ClienteSingleton.GetInstance().EjecutarSQL(str_sql, parametros) == 1);

        }

        //public IList<Detalles> GetByFilters(Dictionary<string, object> parametros)
        //{
        //    List<Detalles> lst = new List<Detalles>();
        //    String strSql = string.Concat(" select a.id_atencion, a.fecha, a.a_descrip, a.importe, a.borrado, m.id_mascota, m.m_nombre, u.id_usuario, u.usuario from Atencion a join Mascotas m on a.id_mascota = m.id_mascota join Usuarios u on u.id_usuario = a.id_usuario   ");

        //    if (parametros.ContainsKey("id_detalle"))
        //        strSql += " where (m.m_nombre LIKE '%' + @mascota + '%') ";

        //    strSql += " order by a.borrado, a.fecha desc    ";

        //    var resultado = ClienteSingleton.GetInstance().ConsultaSQL(strSql, parametros);

        //    foreach (DataRow row in resultado.Rows)
        //        lst.Add(ObjectMapping(row));

        //    return lst;
        //}


        //public Atenciones GetOne(string cadena)
        //{
        //    //Construimos la consulta sql para buscar el Cliente en la base de datos.
        //    String strSql = string.Concat(" select m.id_mascota as id, m.m_nombre as mascota, m.edad, m.borrado, t.id_tipo, t_descrip as animal, c.id_cliente, c.sexo, c.c_nombre as dueño from mascotas m join clientes c on m.id_cliente=c.id_cliente join tipos t on t.id_tipo=m.id_tipo   where m.m_nombre=@mascota ");


        //    var parametros = new Dictionary<string, object>();
        //    parametros.Add("mascota", cadena);
        //    //Usando el método GetDBHelper obtenemos la instancia unica de DBHelper (Patrón Singleton) y ejecutamos el método ConsultaSQL()
        //    var resultado = ClienteSingleton.GetInstance().ConsultaSQL(strSql, parametros);

        //    // Validamos que el resultado tenga al menos una fila.
        //    if (resultado.Rows.Count > 0)
        //    {
        //        return ObjectMapping(resultado.Rows[0]);
        //    }

        //    return null;
        //}


    }
}
