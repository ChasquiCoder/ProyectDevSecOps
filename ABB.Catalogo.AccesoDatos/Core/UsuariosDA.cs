using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABB.Catalogo.Entidades.Core;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ABB.Catalogo.Utiles.Helpers;

namespace ABB.Catalogo.AccesoDatos.Core
{
    public class UsuariosDA
    {
        public Usuarios LlenarEntidad(IDataReader reader)
        {
            Usuarios usuario = new Usuarios();
            reader.GetSchemaTable().DefaultView.RowFilter = "ColumnName = 'IdUsuario'";
            if (reader.GetSchemaTable().DefaultView.Count.Equals(1))
            {
                if (!Convert.IsDBNull(reader["IdUsuario"]))
                    usuario.IdUsuario = Convert.ToInt32(reader["IdUsuario"]);

            }
            reader.GetSchemaTable().DefaultView.RowFilter = "ColumnName = 'CodUsuario'";
            if (reader.GetSchemaTable().DefaultView.Count.Equals(1))
            {
                if (!Convert.IsDBNull(reader["CodUsuario"]))
                    usuario.CodUsuario = Convert.ToString(reader["CodUsuario"]);

            }
            reader.GetSchemaTable().DefaultView.RowFilter = "ColumnName = 'Clave'";
            if (reader.GetSchemaTable().DefaultView.Count.Equals(1))
            {
                //if (!Convert.IsDBNull(reader["Clave"]))
                //    usuario.Clave = Convert.ToString(reader["Clave"]);

            }
            reader.GetSchemaTable().DefaultView.RowFilter = "ColumnName = 'Nombres'";
            if (reader.GetSchemaTable().DefaultView.Count.Equals(1))
            {
                if (!Convert.IsDBNull(reader["Nombres"]))
                    usuario.Nombres = Convert.ToString(reader["Nombres"]);

            }
            reader.GetSchemaTable().DefaultView.RowFilter = "ColumnName = 'IdRol'";
            if (reader.GetSchemaTable().DefaultView.Count.Equals(1))
            {
                if (!Convert.IsDBNull(reader["IdRol"]))
                    usuario.IdRol = Convert.ToInt32(reader["IdRol"]);

            }
            reader.GetSchemaTable().DefaultView.RowFilter = "ColumnName = 'DesRol'";
            if (reader.GetSchemaTable().DefaultView.Count.Equals(1))
            {
                if (!Convert.IsDBNull(reader["DesRol"]))
                    usuario.DesRol = Convert.ToString(reader["DesRol"]);

            }

            return usuario;
        }

        public int GetUsuarioId(string pUsuario, string pPassword)
        {
            try
            {
                //  string UserPass = Utilitario.GetMd5Hash2(pPassword);
                byte[] UserPass = EncriptacionHelper.EncriptarByte(pPassword);
                int returnedVal = 0;
                using (SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["cnnSql"]].ConnectionString))
                {
                    using (SqlCommand comando = new SqlCommand("paUsuario_BuscaCodUserClave", conexion))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@ParamUsuario", pUsuario);
                        comando.Parameters.AddWithValue("@ParamPass", UserPass);
                        conexion.Open();
                        returnedVal = Convert.ToInt32(comando.ExecuteScalar());
                        conexion.Close();
                    }

                }

                return Convert.ToInt32(returnedVal);
            }
            catch (Exception ex)
            {
                string innerException = (ex.InnerException == null) ? "" : ex.InnerException.ToString();
                //Logger.paginaNombre = this.GetType().Name;
                //Logger.Escribir("Error en Logica de Negocio: " + ex.Message + ". " + ex.StackTrace + ". " + innerException);
                return -1;
            }
        }

        public List<Usuarios> ListarUsuarios()
        {
            List<Usuarios> ListaEntidad = new List<Usuarios>();
            Usuarios entidad = null;
            using (SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["cnnSql"]].ConnectionString))
            {
                using (SqlCommand comando = new SqlCommand("ListarUsuarios", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    conexion.Open();
                    SqlDataReader reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        entidad = LlenarEntidad(reader);
                        ListaEntidad.Add(entidad);
                    }
                }
                conexion.Close();
            }
            return ListaEntidad;
        }
        public Usuarios InsertarUsuario(Usuarios usuario)
        {
            byte[] UserPass = EncriptacionHelper.EncriptarByte(usuario.ClaveTxt);
            usuario.Clave = UserPass;

            using (SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["cnnSql"]].ConnectionString))
            {
                using (SqlCommand comando = new SqlCommand("paUsuario_insertar", conexion))
                {
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@Clave", usuario.Clave);
                    comando.Parameters.AddWithValue("@CodUsuario", usuario.CodUsuario);
                    comando.Parameters.AddWithValue("@Nombres", usuario.Nombres);
                    comando.Parameters.AddWithValue("@IdRol", usuario.IdRol);

                    conexion.Open();
                    usuario.IdUsuario = Convert.ToInt32(comando.ExecuteScalar());
                    conexion.Close();
                }
            }
            return usuario;
        }
        public Usuarios ModificarUsuario(int IdUsuario, Usuarios usuario)
        {
            Usuarios SegSSOMUsuario = null;
            byte[] UserPass = EncriptacionHelper.EncriptarByte(usuario.ClaveTxt);
            usuario.Clave = UserPass;

            using (SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["cnnSql"]].ConnectionString))
            {

                using (SqlCommand comando = new SqlCommand("paUsuario_Modificar", conexion))
                {
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@IdUsuario", IdUsuario);
                    comando.Parameters.AddWithValue("@CodUsuario", usuario.CodUsuario);
                    comando.Parameters.AddWithValue("@Clave", usuario.Clave);
                    comando.Parameters.AddWithValue("@Nombres", usuario.Nombres);
                    comando.Parameters.AddWithValue("@IdRol", usuario.IdRol);
                    conexion.Open();
                    SqlDataReader reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        SegSSOMUsuario = LlenarEntidad(reader);

                    }

                    conexion.Close();
                }
            }
            return SegSSOMUsuario;
        }
        public Usuarios EliminarUsuario(int IdUsuario)
        {
            Usuarios SegSSOMUsuario = null;

            using (SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["cnnSql"]].ConnectionString))
            {

                using (SqlCommand comando = new SqlCommand("paUsuario_Eliminar", conexion))
                {
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.AddWithValue("@IdUsuario", IdUsuario);
                    conexion.Open();
                    SqlDataReader reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        SegSSOMUsuario = LlenarEntidad(reader);
                    }
                    conexion.Close();
                }
            }
            return SegSSOMUsuario;
        }

        public Usuarios BuscaUsuarioId(int pUsuarioId)
        {
            Usuarios entidad = null;
            try
            {

                using (SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["cnnSql"]].ConnectionString))
                {
                    using (SqlCommand comando = new SqlCommand("paUsuario_BuscaUserId", conexion))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@ParamUsuario", pUsuarioId);
                        conexion.Open();
                        SqlDataReader reader = comando.ExecuteReader();
                        while (reader.Read())
                        {
                            entidad = LlenarEntidad(reader);

                        }
                        conexion.Close();
                    }

                }

                return entidad;
            }
            catch (Exception ex)
            {
                string innerException = (ex.InnerException == null) ? "" : ex.InnerException.ToString();
                //Logger.paginaNombre = this.GetType().Name;
                //Logger.Escribir("Error en Logica de Negocio: " + ex.Message + ". " + ex.StackTrace + ". " + innerException);
                return entidad;
            }

        }

    }
}
