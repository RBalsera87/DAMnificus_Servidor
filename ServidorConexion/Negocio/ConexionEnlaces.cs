using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ServidorConexion.Negocio;
using EntidadesCompartidas;

namespace ServidorConexion.Negocio
{
    public class ConexionEnlaces
    {
        private string cadenaConexion;
        private MySqlConnection conexion;


        public ConexionEnlaces() { }

        private void conectar()
        {
            try
            {
                cadenaConexion = "Server=localhost;Database=damnificus_enlaces;Uid=root;Pwd=;";
                conexion = new MySqlConnection(cadenaConexion);
                conexion.Open();
            }
            catch (MySqlException)
            {
                throw;
            }
        }

        public List<Enlaces> obtenerColeccionEnlaces()
        {
            conectar();
            MySqlCommand cmd = new MySqlCommand();
            //La palabra BINARY sirve para hacer distinción de mayúsculas y minúsculas
            cmd.CommandText = "Select * from enlaces";
            //cmd.Parameters.AddWithValue("@pass", pass);
            cmd.Connection = conexion;
            MySqlDataReader Datos = cmd.ExecuteReader();
            List<Enlaces> listaEnlaces = null;
            if (Datos.HasRows)
            {
                listaEnlaces = new List<Enlaces>();
                while (Datos.Read())
                {
                    Enlaces enlace = new Enlaces();
                    enlace.id = Datos.GetString(0);
                    enlace.link = Datos.GetString(1);
                    enlace.titulo = Datos.GetString(2);
                    enlace.descripcion = Datos.GetString(3);
                    enlace.valoracion = Datos.GetString(4);
                    enlace.imagen = Datos.GetString(5);
                    enlace.tipo = Datos.GetString(6);
                    enlace.tema = Datos.GetString(7);
                    enlace.uploader = Datos.GetString(8);
                    enlace.activo = Datos.GetString(9);
                    listaEnlaces.Add(enlace);

                }


            }

            conexion.Close();
            return listaEnlaces;

        }
        public bool introducirUsuarioEnBBDD(string usuario)
        {
            conectar();
            MySqlCommand cmd = new MySqlCommand();
            // La palabra BINARY sirve para hacer distinción de mayúsculas y minúsculas
            string sql = "INSERT INTO usuarios VALUES( null, @user, 0 )";
            cmd.Parameters.AddWithValue("@user", usuario);
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
        public string obtenerCurso(string usuario)
        {
            conectar();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = "Select Curso from usuarios where BINARY Nombre = @user";
            cmd.Parameters.AddWithValue("@user", usuario);
            cmd.Connection = conexion;
            MySqlDataReader login = cmd.ExecuteReader();
            if (login.Read())
            {
                string curso = login.GetString(0);
                conexion.Close();
                return curso;
            }
            else
            {
                conexion.Close();
                return "null";
            }
        }
        public bool cambiarCurso(string usuario, int curso)
        {
            conectar();
            MySqlCommand cmd = new MySqlCommand();
            // La palabra BINARY sirve para hacer distinción de mayúsculas y minúsculas
            string sql = "UPDATE usuarios SET Curso = (SELECT Id FROM curso WHERE Id = @curso) WHERE Nombre = @user";
            cmd.Parameters.AddWithValue("@curso", curso);
            cmd.Parameters.AddWithValue("@user", usuario);            
            cmd.CommandText = sql;
            cmd.Connection = conexion;
            if (cmd.ExecuteNonQuery() == 1) // El curso se ha cambiado
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


