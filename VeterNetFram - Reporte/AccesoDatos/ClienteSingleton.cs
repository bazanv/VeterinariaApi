using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace veterNetFram.AccesoDatos
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



    }
}
