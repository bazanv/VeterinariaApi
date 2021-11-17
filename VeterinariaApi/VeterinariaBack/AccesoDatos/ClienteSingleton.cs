using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeterinariaBack.Dominio;

namespace VeterinariaBack.AccesoDatos
{
    public class ClienteSingleton
    {
        private string string_conexion;
        private static ClienteSingleton instance;

        public ClienteSingleton()
        {
            string_conexion = @"Data Source=M26\SQLEXPRESS;Initial Catalog=V;Integrated Security=True";
        }

        public static ClienteSingleton GetInstance()
        {
            if (instance == null)
                instance = new ClienteSingleton();

            return instance;
        }

        public DataTable ConsultaSQL(string strSql, Dictionary<string, object> prs = null)
        {

            SqlConnection dbConnection = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            DataTable tabla = new DataTable();
            try
            {

                dbConnection.ConnectionString = string_conexion;
                dbConnection.Open();
                cmd.Connection = dbConnection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = strSql;

                //Agregamos a la colección de parámetros del comando los filtros recibidos
                if (prs != null)
                {
                    foreach (var item in prs)
                    {
                        cmd.Parameters.AddWithValue(item.Key, item.Value);
                    }
                }

                tabla.Load(cmd.ExecuteReader());
                return tabla;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (dbConnection.State != ConnectionState.Closed)
                    dbConnection.Close();
            }
        }


        public int EjecutarSQL(string strSql, Dictionary<string, object> prs = null)
        {
            // Se utiliza para sentencias SQL del tipo “Insert/Update/Delete”
            SqlConnection dbConnection = new SqlConnection();
            SqlCommand cmd = new SqlCommand();

            int rtdo = 0;


            try
            {
                dbConnection.ConnectionString = string_conexion;
                dbConnection.Open();
                cmd.Connection = dbConnection;
                cmd.CommandType = CommandType.Text;
                // Establece la instrucción a ejecutar
                cmd.CommandText = strSql;

                //Agregamos a la colección de parámetros del comando los filtros recibidos
                if (prs != null)
                {
                    foreach (var item in prs)
                    {
                        cmd.Parameters.AddWithValue(item.Key, item.Value);
                    }
                }

                // Retorna el resultado de ejecutar el comando
                rtdo = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dbConnection.State != ConnectionState.Closed)
                    dbConnection.Close();
            }
            return rtdo;
        }

        public object ConsultaSQLScalar(string strSql)
        {
            SqlConnection dbConnection = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                dbConnection.ConnectionString = string_conexion;
                dbConnection.Open();
                cmd.Connection = dbConnection;
                cmd.CommandType = CommandType.Text;
                // Establece la instrucción a ejecutar
                cmd.CommandText = strSql;
                return cmd.ExecuteScalar();
            }
            catch (SqlException ex)
            {
                throw (ex);
            }
            finally
            {
                if (dbConnection.State != ConnectionState.Closed)
                    dbConnection.Close();
            }
        }



        public bool InsertarAtencion(Atenciones obj)
        {
            int rtdo = 0;
            bool estado = true;
            SqlConnection dbConnection = new SqlConnection();

            SqlTransaction transaccion = null;

            string strSqlMaestro = "     INSERT INTO Atencion (fecha, a_descrip, importe, id_mascota, id_usuario, borrado)" +
                             "     VALUES (@fecha, @a_descrip, @importe, @id_mascota, @id_usuario, 0)";

            string max_id = " select max(id_atencion) from atencion ";

            string strSqlDetalle = "     INSERT INTO Detalles (id_producto, cantidad, p_facturado, id_atencion, borrado)" +
         "     VALUES (@id_producto, @cantidad, @p_facturado, @id_atencion, 0) ";

            try
            {
                dbConnection.ConnectionString = string_conexion;
                dbConnection.Open();
                transaccion = dbConnection.BeginTransaction();
                SqlCommand cmdM = new SqlCommand();
                cmdM.Connection = dbConnection;
                cmdM.Transaction = transaccion;
                cmdM.CommandType = CommandType.Text;
                cmdM.CommandText = strSqlMaestro;

                cmdM.Parameters.AddWithValue("fecha", obj.fecha);
                cmdM.Parameters.AddWithValue("a_descrip", obj.a_descrip);
                cmdM.Parameters.AddWithValue("importe", obj.importe);
                cmdM.Parameters.AddWithValue("id_mascota", obj.mascota.id_mascota);
                cmdM.Parameters.AddWithValue("id_usuario", obj.usuario.id_usuario);

                rtdo = cmdM.ExecuteNonQuery();

                SqlCommand cmdID = new SqlCommand();
                cmdID.Connection = dbConnection;
                cmdID.Transaction = transaccion;
                cmdID.CommandType = CommandType.Text;
                // Establece la instrucción a ejecutar
                cmdID.CommandText = max_id;
                var idAtencion = cmdID.ExecuteScalar();

                //var idAtencion = (int)ClienteSingleton.GetInstance().ConsultaSQLScalar(max_id);



                foreach (Detalles det in obj.detalle)
                {
                    SqlCommand comandoDetalle = new SqlCommand();
                    comandoDetalle.Connection = dbConnection;
                    comandoDetalle.Transaction = transaccion;
                    comandoDetalle.CommandType = CommandType.Text;
                    comandoDetalle.CommandText = strSqlDetalle;

                    comandoDetalle.Parameters.AddWithValue("id_producto", det.producto.id_producto);
                    comandoDetalle.Parameters.AddWithValue("cantidad", det.cantidad);
                    comandoDetalle.Parameters.AddWithValue("p_facturado", det.p_facturado);
                    comandoDetalle.Parameters.AddWithValue("id_atencion", idAtencion);

                    comandoDetalle.ExecuteNonQuery();

                }

                transaccion.Commit();

            }
            catch (SqlException ex)
            {
                transaccion.Rollback();
                estado = false;
            }
            finally
            {
                if (dbConnection.State != ConnectionState.Closed)
                    dbConnection.Close();
            }

            return estado;
        }


        public bool EliminarAtencion(Atenciones obj)
        {
            int rtdo = 0;
            bool estado = true;
            SqlConnection dbConnection = new SqlConnection();

            SqlTransaction transaccion = null;

            string strSqlMaestro = " UPDATE Atencion" +
                "  SET borrado = 1" +
                "  WHERE id_atencion = " + obj.id_atencion.ToString();


            string strSqlDetalle = " UPDATE Detalles" +
                "  SET borrado = 1" +
                "  WHERE id_atencion = " + obj.id_atencion.ToString();

            try
            {
                dbConnection.ConnectionString = string_conexion;
                dbConnection.Open();
                transaccion = dbConnection.BeginTransaction();
                SqlCommand cmdM = new SqlCommand();
                cmdM.Connection = dbConnection;
                cmdM.Transaction = transaccion;
                cmdM.CommandType = CommandType.Text;
                cmdM.CommandText = strSqlMaestro;

                rtdo = cmdM.ExecuteNonQuery();

                SqlCommand comandoDetalle = new SqlCommand();
                comandoDetalle.Connection = dbConnection;
                comandoDetalle.Transaction = transaccion;
                comandoDetalle.CommandType = CommandType.Text;
                comandoDetalle.CommandText = strSqlDetalle;

                comandoDetalle.ExecuteNonQuery();


                transaccion.Commit();

            }
            catch (SqlException ex)
            {
                transaccion.Rollback();
                estado = false;
            }
            finally
            {
                if (dbConnection.State != ConnectionState.Closed)
                    dbConnection.Close();
            }

            return estado;
        }

    }
}
