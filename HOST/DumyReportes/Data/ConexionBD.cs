//s:[include Sherlock]

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DumyReportes.Data
{

    public class ConexionBD ///s:# Administra conexión a la DB
                            ///s:p> Maneja un Singleton de la conexión a DB, está centralizado, así que si se quiere cambia la DB, solamente se debe
    //cambiar la CADENA_CONEXION<s:p
    ///s:t Cadena de conexión a la DB
    ///s:c>
    {
        private static string CADENA_CONEXION =
         //   @"Data Source=localhost\SQLEXPRESSTESTV,50891;Initial Catalog=ChecadasVic;User ID=cheto;Password=chetoconsalsa";

         @"Data Source=localhost\SQLEXPRESSTESTV,8465;Initial Catalog=ReportApp;User ID=ReportApp;Password=Marves2022;";

        // @"Data Source=172.16.0.7\SQLEXPRESS,1433;Initial Catalog=Checadas;User ID=asistencia;Password=w0RkbE4t";
        ///<s:c




        ///s:t Singleton de conexión a DB. 
        ///s:c>
        private static SqlConnection connection;
        ///<s:c

        ///s:t Funcion que hace funcionar el Singleton, si la conexión es nula, la crea, si está cerrada, la abre  y la retorna.
        ///s:c>
        public static SqlConnection getConexion()
        {

            if (ConexionBD.connection == null)
            {
                // ConexionBD.connection = new SqlConnection("server=JEFSISN\\SQLEXPRESS ; database=datos_suprema ; integrated security = true");
                ConexionBD.connection = new SqlConnection(ConexionBD.CADENA_CONEXION + "Connection Timeout=10;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;");

            }

            if (ConexionBD.connection.State == System.Data.ConnectionState.Closed)
            {
                ConexionBD.connection.ConnectionString = ConexionBD.CADENA_CONEXION + "Connection Timeout=10;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";
                ConexionBD.connection.Open();
            }

            return ConexionBD.connection;

        }
        ///<s:c

        ///s:t Funcion auxiliar para cerrar la conexión
        ///s:c>
        public static void desconectar()
        {
            if (ConexionBD.connection == null)
                return;
            if (ConexionBD.connection.State == System.Data.ConnectionState.Closed)
                ConexionBD.connection.Close();

        }
        ///<s:c


    }
}