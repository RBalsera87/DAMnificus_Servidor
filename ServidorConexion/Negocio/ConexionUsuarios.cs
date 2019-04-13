using EntidadesCompartidas;
using MySql.Data.MySqlClient;
using ServidorConexion.Negocio;
using System.Collections.Generic;

namespace ServidorConexion
{   
    class ConexionUsuarios
    {
        private string cadenaConexion;
        private MySqlConnection conexion;

       
        public ConexionUsuarios() { }

        private void conectar()
        {
            try
            {
                
                //if (conexion != null && conexion.State != ConnectionState.Open)
                //{
                    cadenaConexion = "Server=localhost;Database=damnificus_usuarios;Uid=root;Pwd=;";
                    conexion = new MySqlConnection(cadenaConexion);
                    conexion.Open();
                //}
                    
            }
            catch (MySqlException e)
            {
                throw;
            }
        }

        public string obtenerPassHash(Peticion peticion)
        {
            conectar();
            MySqlCommand cmd = new MySqlCommand();
            //La palabra BINARY sirve para hacer distinción de mayúsculas y minúsculas
            cmd.CommandText = "Select pass_hash from usuarios where BINARY usuario = @user";
            cmd.Parameters.AddWithValue("@user", peticion.usuario);
            //cmd.Parameters.AddWithValue("@pass", pass);
            cmd.Connection = conexion;
            MySqlDataReader login = cmd.ExecuteReader();
            if (login.Read())
            {
                string clave = login.GetString(0);
                conexion.Close();
                return clave;
            }
            else
            {
                conexion.Close();
                return "null";
            }

        }
        public bool actualizarTokenEnBBDD(string token, string usuario)
        {
            conectar();
            MySqlCommand cmd = new MySqlCommand();
            // La palabra BINARY sirve para hacer distinción de mayúsculas y minúsculas
            string sql = "UPDATE credenciales SET Token = @token WHERE Id = (SELECT Id FROM usuarios WHERE BINARY Nombre = @user)";
            cmd.Parameters.AddWithValue("@token", token);
            cmd.Parameters.AddWithValue("@user", usuario);
            cmd.CommandText = sql;
            cmd.Connection = conexion;
            if (cmd.ExecuteNonQuery() == 1) // El token se ha guardado
            {                
                conexion.Close();
                return true;
            }else
            {
                conexion.Close();
                return false;
            }
        }
        public bool comprobarSiExisteEnBD(Peticion peticionActual)
        {

            conectar();
            MySqlCommand cmd = new MySqlCommand();            
            if (peticionActual.peticion.Equals("buscaEmailenBD"))
            {
                cmd.CommandText = "Select email from usuarios where BINARY email = @arg";
                cmd.Parameters.AddWithValue("@arg", peticionActual.datos["email"]);
            }
            else if (peticionActual.peticion.Equals("buscaUsuarioenBD"))
            {
                cmd.CommandText = "Select usuario from usuarios where BINARY usuario = @arg";
                cmd.Parameters.AddWithValue("@arg", peticionActual.usuario);
            }
            cmd.Connection = conexion;
            MySqlDataReader resultado = cmd.ExecuteReader();
            if (resultado.HasRows)
            {
                // Ya existe en BD
                conexion.Close();
                return true;
            }
            else
            {
                // No exxiste en BD
                conexion.Close();
                return false;
            }

        }
        public bool introducirUsuarioEnBBDD(string usuario, string email, string pass, string nombre, string apellidos)
        {
            conectar();
            MySqlCommand cmd = new MySqlCommand();
            // La palabra BINARY sirve para hacer distinción de mayúsculas y minúsculas
            string sql = "INSERT INTO usuarios VALUES( @user, @email, @pass, @name, @apell )";            
            cmd.Parameters.AddWithValue("@user", usuario);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@pass", pass);
            cmd.Parameters.AddWithValue("@name", nombre);
            cmd.Parameters.AddWithValue("@apell", apellidos);
            cmd.CommandText = sql;
            cmd.Connection = conexion;
            if (cmd.ExecuteNonQuery() == 1) // El usuario se ha guardado
            {
                conexion.Close();
                return true;
            }
            else
            {
                conexion.Close();
                return false;
            }
        }
    }
}
