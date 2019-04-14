using System;

namespace ServidorConexion.Negocio
{
    class ConsolaDebug
    {
        public static void escribirEnConsola(string tipo, string texto)
        {
            string hora = DateTime.Now.ToString("[HH:mm:ss]");
            string tipomsg = string.Format("[{0}] ", tipo);
            if (tipo.Equals("INFO")) { Console.ForegroundColor = ConsoleColor.Gray; }
            else if (tipo.Equals("INFO+")) { Console.ForegroundColor = ConsoleColor.DarkGreen; }
            else if (tipo.Equals("DEBUG")) { Console.ForegroundColor = ConsoleColor.DarkYellow; }
            else if (tipo.Equals("WARNING")) { Console.ForegroundColor = ConsoleColor.Yellow; }
            else if (tipo.Equals("ERROR")) { Console.ForegroundColor = ConsoleColor.Red; }
            Console.WriteLine(hora + tipomsg + texto);
        }
        public static void escribirEnConsola(string tipo, string texto, string args)
        {
            string hora = DateTime.Now.ToString("[HH:mm:ss]");
            string tipomsg = string.Format("[{0}] ", tipo);
            if (tipo.Equals("INFO")) { Console.ForegroundColor = ConsoleColor.Gray; }
            else if (tipo.Equals("INFO+")) { Console.ForegroundColor = ConsoleColor.DarkGreen; }
            else if (tipo.Equals("DEBUG")) { Console.ForegroundColor = ConsoleColor.DarkYellow; }
            else if (tipo.Equals("WARNING")) { Console.ForegroundColor = ConsoleColor.Yellow; }
            else if (tipo.Equals("ERROR")) { Console.ForegroundColor = ConsoleColor.Red; }
            Console.WriteLine(hora + tipomsg + texto, args);
        }
        public static void cargarConsola()
        {
            Console.SetWindowSize(120, 35);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(" _____   _______  _______         __   ___  __                       _______                                  \n" +
                              "|     \\ |   _   ||   |   |.-----.|__|.'  _||__|.----..--.--..-----. |     __|.-----..----..--.--..-----..----.\n" +
                              "|  --  ||       ||       ||     ||  ||   _||  ||  __||  |  ||__ --| |__     ||  -__||   _||  |  ||  -__||   _|\n" +
                              "|_____/ |___|___||__|_|__||__|__||__||__|  |__||____||_____||_____| |_______||_____||__|   \\___/ |_____||__| \n");

            Console.WriteLine("░░░░░░░░░░░░░░░░░░░▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▓▓▓▓▓▓▓▓▓▓▓▓▓▓██████████▓▓▓▓▓▓▓▓▓▓▓▓▓▓▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒░░░░░░░░░░░░░░░░░░░");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Version 0.8 - Abril 2019");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
