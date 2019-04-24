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

        public List<Enlaces> obtenerColeccionEnlaces(string asignatura)
        {
            conectar();
            MySqlCommand cmd = new MySqlCommand();
            //La palabra BINARY sirve para hacer distinción de mayúsculas y minúsculas
            cmd.CommandText = "select e.Id,e.Link,e.Titulo,e.Descripcion,e.Valoracion,e.Imagen,e.Tipo,e.Tema,e.Uploader,e.Activo from enlaces e JOIN temas t on e.Tema = t.id JOIN asignaturas a on t.Asignatura = a.Id where a.Nombre = @asignatura AND e.activo = 1; ";
            cmd.Parameters.AddWithValue("@asignatura", asignatura);
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

        public string sumarYRestarValoracion(int id, string operacion)
        {
            if(id > 0)
            {
                conectar();
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "Select valoracion from enlaces WHERE id = @id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Connection = conexion;
                MySqlDataReader login = cmd.ExecuteReader();
                if (login.Read())
                {
                    int valoracion = int.Parse(login.GetString(0));
                    if (operacion.Equals("sumar"))
                    {
                        if(valoracion == 100)
                        {
                            return "correcto";
                        }
                        else
                        {
                            valoracion += 1;
                        }
                    }
                    else if(operacion.Equals("restar"))
                    {
                        if (valoracion == 0)
                        {
                            return "correcto";
                        }
                        else
                        {
                            valoracion -= 1;
                        }
                    }
                    
                    
                    conexion.Close();

                    conectar();
                    cmd = new MySqlCommand();
                    // La palabra BINARY sirve para hacer distinción de mayúsculas y minúsculas
                    string sql = "UPDATE enlaces SET valoracion = @val WHERE id = @id";
                    cmd.Parameters.AddWithValue("@val", valoracion);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandText = sql;
                    cmd.Connection = conexion;
                    if (cmd.ExecuteNonQuery() == 1) // El usuario se ha guardado
                    {
                        conexion.Close();
                        return "correcto";
                    }
                    else
                    {
                        conexion.Close();
                        return "incorrecto";
                    }
                    

                    //cmd.CommandText = "UPDATE enlaces SET valoracion = @val WHERE id = @id";
                    ////cmd.Parameters.AddWithValue("@val", valoracion);
                    ////cmd.Parameters.AddWithValue("@id", id);
                    //cmd.Connection = conexion;
                    //cmd.ExecuteNonQuery();

                    //conexion.Close();
                    //return "correcto";
                }
                else
                {
                    conexion.Close();
                    return "incorrecto";
                }
            }else
            {
                return "incorrecto";
            }
            
        }

    }
}


