﻿using System.Collections.Generic;
using MySql.Data.MySqlClient;
using EntidadesCompartidas;
using System;

namespace ServidorConexion.Metodos
{
    public class ConexionEnlaces
    {
        private string cadenaConexion;
        private MySqlConnection conexion;
        private Object bloqueo = new Object(); // Bloqueo para proteger los accesos a la base de datos por los hilos


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

        public List<Enlaces> obtenerColeccionEnlaces(Dictionary<string, string> datos, string usuario)
        {
            string asignatura = datos["asignatura"];
            string credenciales = datos["credenciales"];
            conectar();
            MySqlCommand cmd = new MySqlCommand();
            if (asignatura.Equals("todas"))
            {
                cmd.CommandText = "select e.Id,e.Link,e.Titulo,e.Descripcion,e.Valoracion,e.Imagen,e.Tipo,t.Nombre,e.Uploader,e.Activo,a.Nombre from enlaces e JOIN temas t on e.Tema = t.id JOIN asignaturas a on t.Asignatura = a.Id; ";

            }else if (asignatura.Equals("personal"))
            {
                cmd.CommandText = "select e.Id,e.Link,e.Titulo,e.Descripcion,e.Valoracion,e.Imagen,e.Tipo,t.Nombre,e.Uploader,e.Activo,a.Nombre from enlaces e JOIN temas t on e.Tema = t.id JOIN asignaturas a on t.Asignatura = a.Id where e.Uploader = (SELECT id from usuarios where nombre = @usuario) AND (e.activo <> 0);";
                cmd.Parameters.AddWithValue("@usuario", usuario);
            }
            else
            {
                if (credenciales.Equals("admin"))
                {
                    cmd.CommandText = "select e.Id,e.Link,e.Titulo,e.Descripcion,e.Valoracion,e.Imagen,e.Tipo,t.Nombre,e.Uploader,e.Activo,a.Nombre from enlaces e JOIN temas t on e.Tema = t.id JOIN asignaturas a on t.Asignatura = a.Id where a.Nombre = @asignatura;";
                }else
                {
                    cmd.CommandText = "select e.Id,e.Link,e.Titulo,e.Descripcion,e.Valoracion,e.Imagen,e.Tipo,t.Nombre,e.Uploader,e.Activo,a.Nombre from enlaces e JOIN temas t on e.Tema = t.id JOIN asignaturas a on t.Asignatura = a.Id where a.Nombre = @asignatura AND (e.activo = 1 OR e.activo = 2);";

                }
                cmd.Parameters.AddWithValue("@asignatura", asignatura);               
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
                    enlace.asignatura = Datos.GetString(10);
                    listaEnlaces.Add(enlace);

                }


            }

            conexion.Close();
            return listaEnlaces;

        }
        public List<Enlaces> obtenerColeccionEnlacesDeUsuario(int usuario)
        {
            conectar();
            MySqlCommand cmd = new MySqlCommand();
           
            cmd.CommandText = "select e.Id,e.Link,e.Titulo,e.Descripcion,e.Valoracion,e.Imagen,e.Tipo,t.Nombre,e.Uploader,e.Activo,a.Nombre from enlaces e JOIN temas t on e.Tema = t.id JOIN asignaturas a on t.Asignatura = a.Id where e.uploader = @usuario; ";           
            cmd.Parameters.AddWithValue("@usuario", usuario);           
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
                    enlace.asignatura = Datos.GetString(10);
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
            lock (bloqueo)
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
            lock (bloqueo)
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
                // Exception: MySql.Data.MySqlClient.MySqlException (0x80004005): Duplicate entry 'abc' for key 'Link'
                try
                {
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
                catch (MySqlException e) // Si el enlace esta repetido se envia false
                {
                    ConsolaDebug.escribirEnConsola("WARNING", "Enlace duplicado en la base de datos, el nuevo no se guardará");
                    conexion.Close();
                    return false;
                }
                
                
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
            lock (bloqueo)
            {
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
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int valoracion = int.Parse(reader.GetString(0));
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
                    lock (bloqueo)
                    {
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
        public bool cambiarUploaderRollback(List<Enlaces> listaUsuarios)
        {
            lock (bloqueo)
            {
                conectar();
                foreach (Enlaces enlace in listaUsuarios)
                {
                    MySqlCommand cmd = new MySqlCommand();
                    string sql = "UPDATE enlaces SET Uploader = @uploader WHERE id = @id;";
                    cmd.Parameters.AddWithValue("@id", enlace.uploader);
                    cmd.Parameters.AddWithValue("@uploader", enlace.id);
                    cmd.CommandText = sql;
                    cmd.Connection = conexion;
                    if (cmd.ExecuteNonQuery() != 1)
                    {
                        conexion.Close();
                        return false;
                    }
                }

                return true;
            }
        }
        public bool borrarUsuario(int id, int newUploader)
        {
            lock (bloqueo)
            {
                conectar();
                MySqlTransaction Transaccion;
                Transaccion = conexion.BeginTransaction();
                ConsolaDebug.escribirEnConsola("INFO", "Comenzando transacción de borrado en BD enlaces...");
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.Transaction = Transaccion;
                try
                {
                    // Primer update de la transacción
                    string sql = "UPDATE enlaces SET uploader = @newUploader WHERE uploader = @id";
                    cmd.Parameters.AddWithValue("@newUploader", newUploader);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                    ConsolaDebug.escribirEnConsola("INFO", "Update en BD ENLACES tabla enlaces se ha cambiado el uploader al usuario admin satisfactoriamente");
                    // Segundo delete de la transacción
                    sql = "DELETE FROM usuarios WHERE id = @id";
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                    Transaccion.Commit();
                    ConsolaDebug.escribirEnConsola("INFO", "Delete en BD ENLACES tabla ususarios ejecutado satisfactoriamente");
                    return true;
                }
                catch (Exception e)
                {
                    try
                    {
                        ConsolaDebug.escribirEnConsola("WARNING", "Problema en transacción, comenzando ROLLBACK");
                        Transaccion.Rollback();
                        ConsolaDebug.escribirEnConsola("INFO", "ROLLBACK ejecutado satisfactoriamente");
                    }
                    catch (MySqlException ex)
                    {
                        ConsolaDebug.escribirEnConsola("WARNING", "Problema en ROLLBACK");
                        if (Transaccion.Connection != null)
                        {
                            ConsolaDebug.escribirEnConsola("ERROR", "Excepción lanzada: {0}", ex.Message);
                        }
                    }
                    ConsolaDebug.escribirEnConsola("WARNING", "No se ha borrado nada en la BD");
                    ConsolaDebug.escribirEnConsola("ERROR", "Excepción lanzada: {0}", e.Message);
                    return false;
                }
                finally
                {
                    conexion.Close();
                }
            }

        }
        public bool cambiarUploader(int uploader, int newUploader)
        {
            conectar();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = "SELECT Id FROM enlaces WHERE Uploader = @uploader";
            cmd.Parameters.AddWithValue("@uploader", uploader);
            cmd.Connection = conexion;
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                conexion.Close();
                lock (bloqueo)
                {
                    conectar();
                    cmd = new MySqlCommand();
                    string sql = "UPDATE enlaces SET Uploader = @newUploader WHERE Uploader = @uploader;";
                    cmd.Parameters.AddWithValue("@newUploader", newUploader);
                    cmd.Parameters.AddWithValue("@uploader", uploader);
                    cmd.CommandText = sql;
                    cmd.Connection = conexion;
                    if (cmd.ExecuteNonQuery() > 0)
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
                    
            }else
            {
                return true;
            }
        }
        public bool borrarEnlace(int id)
        {
            conectar();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = "SELECT Id FROM enlaces WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Connection = conexion;
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                lock (bloqueo)
                {
                    conectar();
                    cmd = new MySqlCommand();
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
                    
            }else
            {
                return true;
            }
        }

        public Dictionary<string, int> cambiarActivoRevisionDesactivo(int id, string credenciales)
        {
            Dictionary<string, int> datos = new Dictionary<string, int>();
            conectar();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = "Select activo from enlaces WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Connection = conexion;
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                int estadoNuevo = 0;
                int estado = int.Parse(reader.GetString(0));
                if(credenciales == "admin")
                {
                    datos.Add("email", 0);
                    if (estado == 0)
                    {
                        estadoNuevo = 1;
                        datos.Add("estado", estadoNuevo);
                    }
                    else if(estado == 1)
                    {
                        estadoNuevo = 2;
                        datos.Add("estado", estadoNuevo);
                    }
                    else
                    {
                        estadoNuevo = 0;
                        datos.Add("estado", estadoNuevo);
                    }
                }
                else 
                {
                    if (estado == 1)
                    {
                        estadoNuevo = 2;
                        datos.Add("email", 1);
                        datos.Add("estado", estadoNuevo);
                    }
                    else
                    {
                        datos.Add("email", 0);
                        datos.Add("estado", 2);
                        conexion.Close();
                        return datos;
                    }
                        
                }
                conexion.Close();
                lock (bloqueo)
                {
                    conectar();
                    cmd = new MySqlCommand();
                    string sql = "UPDATE enlaces SET Activo = @estadoNuevo WHERE id = @id";
                    cmd.Parameters.AddWithValue("@estadoNuevo", estadoNuevo);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandText = sql;
                    cmd.Connection = conexion;
                    if (cmd.ExecuteNonQuery() == 1) // El campo se ha modificado
                    {
                        conexion.Close();
                        return datos;
                    }
                    else
                    {
                        conexion.Close();
                        datos.Clear();
                        datos.Add("email", 0);
                        datos.Add("estado", -1);
                        return datos;
                    }
                }               
            }
            else
            {
                conexion.Close();
                datos.Clear();
                datos.Add("email", 0);
                datos.Add("estado", -1);
                return datos;
            }
            
            
        }

        public void borrarNotas(int usuario, int curso)
        {
            lock (bloqueo)
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
                
        }

        public void insertarNotasPrimero(int user)
        {
            lock (bloqueo)
            {
                conectar();
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "INSERT INTO notas (Usuario, Asignatura, Trimestre, Nota) VALUES (@usuario, 1, 1, 0), (@usuario, 1, 2, 0), (@usuario, 1, 3, 0), (@usuario, 2, 1, 0), (@usuario, 2, 2, 0), (@usuario, 2, 3, 0), (@usuario, 3, 1, 0), (@usuario, 3, 2, 0), (@usuario, 3, 3, 0), (@usuario, 4, 1, 0), (@usuario, 4, 2, 0), (@usuario, 4, 3, 0), (@usuario, 5, 1, 0), (@usuario, 5, 2, 0), (@usuario, 5, 3, 0), (@usuario, 11, 1, 0), (@usuario, 11, 2, 0), (@usuario, 11, 3, 0);";
                cmd.Parameters.AddWithValue("@usuario", user);
                cmd.Connection = conexion;
                cmd.ExecuteNonQuery();
                conexion.Close();
            }
                
        }

        public void insertarNotasSegundo(int user)
        {
            lock (bloqueo)
            {
                conectar();
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "INSERT INTO notas (Usuario, Asignatura, Trimestre, Nota) VALUES (@usuario, 6, 1, 0),(@usuario, 6, 2, 0),(@usuario, 7, 1, 0),(@usuario, 7, 2, 0),(@usuario, 8, 1, 0),(@usuario, 8, 2, 0),(@usuario, 9, 1, 0),(@usuario, 9, 2, 0),(@usuario, 10, 1, 0), (@usuario, 10, 2, 0),(@usuario, 12, 1, 0),(@usuario, 12, 2, 0),(@usuario, 13, 1, 0),(@usuario, 13, 2, 0);";
                cmd.Parameters.AddWithValue("@usuario", user);
                cmd.Connection = conexion;
                cmd.ExecuteNonQuery();
                conexion.Close();
            }
                
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
            lock (bloqueo)
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
}


