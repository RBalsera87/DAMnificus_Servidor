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

        public List<Enlaces> obtenerColeccionEnlaces(string asignatura, string usuario)
        {
            conectar();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = "select e.Id,e.Link,e.Titulo,e.Descripcion,e.Valoracion,e.Imagen,e.Tipo,t.Nombre,e.Uploader,e.Activo from enlaces e JOIN temas t on e.Tema = t.id JOIN asignaturas a on t.Asignatura = a.Id where a.Nombre = @asignatura AND (e.activo = 1 OR e.activo = 2);";
            cmd.Parameters.AddWithValue("@asignatura", asignatura);
            if (usuario.ToUpper().Equals("ADMIN"))
            {
                cmd.CommandText =  cmd.CommandText.Remove(cmd.CommandText.Length - 36);
                cmd.CommandText += ";";
            }
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
                    enlace.reportarFallo = int.Parse(enlace.activo);
                    listaEnlaces.Add(enlace);

                }


            }

            conexion.Close();
            return listaEnlaces;

        }

        public List<string> obtenerNombreAsignaturas(string curso)
        {
            conectar();
            MySqlCommand cmd = new MySqlCommand();
            //La palabra BINARY sirve para hacer distinción de mayúsculas y minúsculas
            cmd.CommandText = "SELECT Nombre FROM asignaturas WHERE curso = @curso  ORDER BY Id";
            cmd.Parameters.AddWithValue("@curso", curso);
            cmd.Connection = conexion;
            MySqlDataReader Datos = cmd.ExecuteReader();
            List<string> listaAsignaturas = new List<string> { };
            if (Datos.HasRows)
            {
                while (Datos.Read())
                {
                    listaAsignaturas.Add(Datos.GetString(0));
                }
            }
            conexion.Close();
            return listaAsignaturas;

        }
        public List<string> obtenerNombreTemas(string asignatura)
        {
            conectar();
            MySqlCommand cmd = new MySqlCommand();
            //La palabra BINARY sirve para hacer distinción de mayúsculas y minúsculas
            cmd.CommandText = "SELECT Nombre FROM temas WHERE asignatura = (SELECT Id FROM asignaturas WHERE Nombre = @asignatura)  ORDER BY Id";
            cmd.Parameters.AddWithValue("@asignatura", asignatura);
            cmd.Connection = conexion;
            MySqlDataReader Datos = cmd.ExecuteReader();
            List<string> listaTemas = new List<string> { };
            if (Datos.HasRows)
            {
                while (Datos.Read())
                {
                    listaTemas.Add(Datos.GetString(0));
                }
            }
            conexion.Close();
            return listaTemas;

        }
        public List<double> recogidaNotas(string curso, string usuario)
        {
            conectar();
            MySqlCommand cmd = new MySqlCommand();
            //La palabra BINARY sirve para hacer distinción de mayúsculas y minúsculas
            cmd.CommandText = "SELECT n.Nota FROM notas n INNER JOIN asignaturas a ON n.Asignatura = a.Id WHERE a.Curso = @curso AND n.Usuario = @usuario ORDER BY a.Id, n.Trimestre";
            cmd.Parameters.AddWithValue("@curso", curso);
            cmd.Parameters.AddWithValue("@usuario", usuario);
            cmd.Connection = conexion;
            MySqlDataReader Datos = cmd.ExecuteReader();
            List<double> listaNotas = new List<double> { };
            if (Datos.HasRows)
            {
                while (Datos.Read())
                {
                    listaNotas.Add((double)Datos.GetDecimal(0));
                }
            }
            conexion.Close();
            return listaNotas;

        }

        public List<double> mediaNotas(string curso, string usuario)
        {
            conectar();
            MySqlCommand cmd = new MySqlCommand();
            //La palabra BINARY sirve para hacer distinción de mayúsculas y minúsculas
            cmd.CommandText = "SELECT CAST( AVG( n.Nota ) AS DECIMAL( 4, 2 ) ) FROM notas n INNER JOIN asignaturas a ON n.Asignatura = a.Id WHERE a.Curso = @curso AND n.Usuario = @usuario GROUP BY n.Asignatura ORDER BY a.Id";
            cmd.Parameters.AddWithValue("@curso", curso);
            cmd.Parameters.AddWithValue("@usuario", usuario);
            cmd.Connection = conexion;
            MySqlDataReader Datos = cmd.ExecuteReader();
            List<double> listaNotas = new List<double> { };
            if (Datos.HasRows)
            {
                while (Datos.Read())
                {
                    listaNotas.Add((double)Datos.GetDecimal(0));
                }
            }
            conexion.Close();
            return listaNotas;

        }

        public bool introducirUsuarioEnBBDD(string usuario)
        {
            conectar();
            MySqlCommand cmd = new MySqlCommand();

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
        public bool introducirNuevoEnlace(string usuario, string titulo, string imagen, string descripcion, string tipo, string enlace, string tema, string rango)
        {
            conectar();
            MySqlCommand cmd = new MySqlCommand();

            string sql = "INSERT INTO enlaces VALUES( null, @enlace, @titulo, @descripcion, 50, @imagen, @tipo, (SELECT Id FROM temas WHERE Nombre = @tema), (SELECT Id FROM usuarios WHERE Nombre = @user), @rango )"; // Cambiar a 0 el ultimo valor!!!!
            cmd.Parameters.AddWithValue("@enlace", enlace);
            cmd.Parameters.AddWithValue("@titulo", titulo);
            cmd.Parameters.AddWithValue("@imagen", imagen);
            cmd.Parameters.AddWithValue("@descripcion", descripcion);
            cmd.Parameters.AddWithValue("@tipo", tipo);
            cmd.Parameters.AddWithValue("@tema", tema);
            cmd.Parameters.AddWithValue("@user", usuario);
            if (rango.Equals("admin")) // Si es admin el link se sube ya en activo
                cmd.Parameters.AddWithValue("@rango", 1);
            else
                cmd.Parameters.AddWithValue("@rango", 0);
            cmd.CommandText = sql;
            cmd.Connection = conexion;
            if (cmd.ExecuteNonQuery() == 1) // El enlace se ha guardado
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
        public int sacarUsuario(string usuario)
        {
            conectar();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = "Select Id from usuarios where BINARY Nombre = @user";
            cmd.Parameters.AddWithValue("@user", usuario);
            cmd.Connection = conexion;
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                int user = reader.GetInt32(0);
                conexion.Close();
                return user;
            }
            else
            {
                conexion.Close();
                return -1;
            }
        }
        public bool cambiarCurso(string usuario, int curso)
        {
            int user = sacarUsuario(usuario);
            bool hayNota = comprobarExistenciaNotas(user, curso);
            if(!hayNota)
            {
                switch (curso)
                {
                    case 1:insertarNotasPrimero(user);
                        break;
                    case 2:insertarNotasSegundo(user);
                        break;
                }
            }
            conectar();
            MySqlCommand cmd = new MySqlCommand();
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

        public bool borrarEnlace(int id)
        {
            conectar();
            MySqlCommand cmd = new MySqlCommand();
            string sql = "DELETE FROM enlaces WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.CommandText = sql;
            cmd.Connection = conexion;
            if (cmd.ExecuteNonQuery() == 1)
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

        public string cambiarActivoRevisionDesactivo(int id,string usuario)
        {
            if (id > 0)
            {
                conectar();
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "Select valoracion from enlaces WHERE id = @id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Connection = conexion;
                MySqlDataReader login = cmd.ExecuteReader();
                if (login.Read())
                {
                    int estadoNuevo = 0;
                    int estado = int.Parse(login.GetString(0));
                    if(usuario == "admin")
                    {
                        if(estado == 0)
                        {
                            estadoNuevo = 1;
                        }else if(estado == 1)
                        {
                            estadoNuevo = 2;
                        }
                        else
                        {
                            estadoNuevo = 0;
                        }
                    }else if(usuario != "invitado")
                    {
                        if(estado != 2)
                        {
                            estadoNuevo = 2;
                        }
                        else
                        {
                            return "correcto";
                        }
                        
                    }

                    conectar();
                    cmd = new MySqlCommand();
                    string sql = "UPDATE enlaces SET Activo = @val WHERE id = @id";
                    cmd.Parameters.AddWithValue("@val", estadoNuevo);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandText = sql;
                    cmd.Connection = conexion;
                    if (cmd.ExecuteNonQuery() == 1) // El campo se ha modificado
                    {
                        conexion.Close();
                        return "correcto";
                    }
                    else
                    {
                        conexion.Close();
                        return "incorrecto";
                    }
                 
                }
                else
                {
                    conexion.Close();
                    return "incorrecto";
                }
            }
            else
            {
                return "incorrecto";
            }
            
        }

        public void borrarNotas(int usuario, int curso)
        {
            conectar();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = "delete from notas where Usuario = @usuario and Asignatura in (Select Id from asignaturas where Curso = @curso)";
            cmd.Parameters.AddWithValue("@usuario", usuario);
            cmd.Parameters.AddWithValue("@curso", curso);
            cmd.Connection = conexion;
            cmd.ExecuteNonQuery();
            conexion.Close();
        }

        public void insertarNotasPrimero(int user)
        {
            conectar();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = "INSERT INTO notas (Usuario, Asignatura, Trimestre, Nota) VALUES (@usuario, 1, 1, 0), (@usuario, 1, 2, 0), (@usuario, 1, 3, 0), (@usuario, 2, 1, 0), (@usuario, 2, 2, 0), (@usuario, 2, 3, 0), (@usuario, 3, 1, 0), (@usuario, 3, 2, 0), (@usuario, 3, 3, 0), (@usuario, 4, 1, 0), (@usuario, 4, 2, 0), (@usuario, 4, 3, 0), (@usuario, 5, 1, 0), (@usuario, 5, 2, 0), (@usuario, 5, 3, 0), (@usuario, 11, 1, 0), (@usuario, 11, 2, 0), (@usuario, 11, 3, 0);";
            cmd.Parameters.AddWithValue("@usuario", user);
            cmd.Connection = conexion;
            cmd.ExecuteNonQuery();
            conexion.Close();
        }

        public void insertarNotasSegundo(int user)
        {
            conectar();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = "INSERT INTO notas (Usuario, Asignatura, Trimestre, Nota) VALUES (@usuario, 6, 1, 0),(@usuario, 6, 2, 0),(@usuario, 7, 1, 0),(@usuario, 7, 2, 0),(@usuario, 8, 1, 0),(@usuario, 8, 2, 0),(@usuario, 9, 1, 0),(@usuario, 9, 2, 0),(@usuario, 10, 1, 0), (@usuario, 10, 2, 0),(@usuario, 12, 1, 0),(@usuario, 12, 2, 0),(@usuario, 13, 1, 0),(@usuario, 13, 2, 0);";
            cmd.Parameters.AddWithValue("@usuario", user);
            cmd.Connection = conexion;
            cmd.ExecuteNonQuery();
            conexion.Close();
        }

        public bool comprobarExistenciaNotas(int usuario, int curso)
        {
            bool salida = false;
            conectar();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = "Select Nota from notas where Usuario = @usuario and Asignatura in (Select Id from asignaturas where Curso = @curso) limit 1";
            cmd.Parameters.AddWithValue("@usuario", usuario);
            cmd.Parameters.AddWithValue("@curso", curso);
            cmd.Connection = conexion;
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                salida = true;                
            }
            conexion.Close();
            return salida;
        }
        public string hayNota(string trimestre, string asignatura, string usuario)
        {
            string salida = "no";
            conectar();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = "SELECT Nota FROM notas WHERE Trimestre = @trimestre AND Asignatura = (SELECT Id FROM asignaturas WHERE Nombre = @asignatura) AND Usuario = @usuario";
            cmd.Parameters.AddWithValue("@trimestre", trimestre);
            cmd.Parameters.AddWithValue("@asignatura", asignatura);
            cmd.Parameters.AddWithValue("@usuario", usuario);
            cmd.Connection = conexion;
            MySqlDataReader Datos = cmd.ExecuteReader();
            if (Datos.HasRows)
            {
                while (Datos.Read())
                {
                    int aux = (int)Datos.GetDecimal(0);
                    if (aux > 0)
                    {
                        salida = "si";
                    }
                }
            }
            conexion.Close();
            return salida;
        }

        public void agregarNota(string nota, string trimestre, string asignatura, string usuario)
        {
            conectar();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = "UPDATE notas SET Nota = @nota WHERE Trimestre = @trimestre AND Asignatura = (SELECT Id FROM asignaturas WHERE Nombre = @asignatura) AND Usuario = @usuario";
            cmd.Parameters.AddWithValue("nota", nota);
            cmd.Parameters.AddWithValue("@trimestre", trimestre);
            cmd.Parameters.AddWithValue("@asignatura", asignatura);
            cmd.Parameters.AddWithValue("@usuario", usuario);
            cmd.Connection = conexion;
            cmd.ExecuteNonQuery();
            conexion.Close();
        }
    }
}


