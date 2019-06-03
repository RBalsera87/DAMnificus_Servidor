using System;

namespace ServidorConexion.Metodos
{
    class ConsolaDebug
    {
        private static bool debugActivado = true; // Activa o desactiva los mensajes de depuración en la consola
        public static void escribirEnConsola(string tipo, string texto)
        {
            string hora = DateTime.Now.ToString("[HH:mm:ss]");
            string tipomsg = string.Format("[{0}] ", tipo);
            if (tipo.Equals("INFO")) { Console.ForegroundColor = ConsoleColor.Gray; }
            else if (tipo.Equals("INFO+")) { Console.ForegroundColor = ConsoleColor.White; }
            else if (tipo.Equals("DEBUG")) { if (!debugActivado) { return; } Console.ForegroundColor = ConsoleColor.DarkYellow; }
            else if (tipo.Equals("WARNING")) { Console.ForegroundColor = ConsoleColor.Yellow; }
            else if (tipo.Equals("ERROR")) { Console.ForegroundColor = ConsoleColor.Red; }
            Console.WriteLine(hora + tipomsg + texto);
        }
        public static void escribirEnConsola(string tipo, string texto, string args)
        {
            string hora = DateTime.Now.ToString("[HH:mm:ss]");
            string tipomsg = string.Format("[{0}] ", tipo);
            if (tipo.Equals("INFO")) { Console.ForegroundColor = ConsoleColor.Gray; }
            else if (tipo.Equals("INFO+")) { Console.ForegroundColor = ConsoleColor.White; }
            else if (tipo.Equals("DEBUG")) { if (!debugActivado) { return; } Console.ForegroundColor = ConsoleColor.DarkYellow; }
            else if (tipo.Equals("WARNING")) { Console.ForegroundColor = ConsoleColor.Yellow; }
            else if (tipo.Equals("ERROR")) { Console.ForegroundColor = ConsoleColor.Red; }
            if (tipo.Equals("DEBUG") && !debugActivado)
            {
                return;
            }
            Console.WriteLine(hora + tipomsg + texto, args);
        }
        public static void cargarConsola()
        {
            Console.SetWindowSize(120, 35);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(" _____   _______  _______         __   ___  __                       _______\n" +
                              "|     \\ |   _   ||   |   |.-----.|__|.'  _||__|.----..--.--..-----. |     __|.-----..----..--.--..-----..----.\n" +
                              "|  --  ||       ||       ||     ||  ||   _||  ||  __||  |  ||__ --| |__     ||  -__||   _||  |  ||  -__||   _|");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("|_____/ |___|___||__|_|__||__|__||__||__|  |__||____||_____||_____| |_______||_____||__|   \\___/ |_____||__| \n");
            Console.ForegroundColor = ConsoleColor.DarkCyan;

            Console.WriteLine("░░░░░░░░░░░░░░░░░░░▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▓▓▓▓▓▓▓▓▓▓▓▓▓▓██████████▓▓▓▓▓▓▓▓▓▓▓▓▓▓▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒░░░░░░░░░░░░░░░░░░░");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Version 1.0 - Junio 2019");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
