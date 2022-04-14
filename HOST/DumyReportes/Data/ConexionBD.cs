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

         @"Data Source=localhost\SQLEXPRESS19,8465;Initial Catalog=ReportApp;User ID=ReportApp;Password=Marves2022;";

        private static string CADENA_CONEXION_Checadas =
         @"Data Source=172.16.0.7\SQLEXPRESS,1433;Initial Catalog=Checadas;User ID=asistencia;Password=w0RkbE4t";
        ///<s:c




        ///s:t Singleton de conexión a DB. 
        ///s:c>
        private static SqlConnection connection;
        ///<s:c

        private static SqlConnection connectionChecadas;

        public enum ConnectionDB
        {
            CHECADAS,
            REPORT_APP
        }

        ///s:t Funcion que hace funcionar el Singleton, si la conexión es nula, la crea, si está cerrada, la abre  y la retorna.
        ///s:c>
        public static SqlConnection getConexion(ConnectionDB db = ConnectionDB.REPORT_APP)
        {


            string currenStrConnection = "";
            SqlConnection tempPointer = null;

            //si el solicitado es NULL, lo instanciará.y lo reasignará
            //en caso contrario, hará que tempPpointer apunte al connection solicitado;
            if (db == ConnectionDB.CHECADAS)
            {
                currenStrConnection = ConexionBD.CADENA_CONEXION_Checadas + "Connection Timeout=10;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=10;Pooling=true;";
                if (connectionChecadas == null)
                {
                    tempPointer = new SqlConnection(currenStrConnection);
                    connectionChecadas = tempPointer;
                }
                else
                    tempPointer = connectionChecadas;
            }
            else
            {
                currenStrConnection = ConexionBD.CADENA_CONEXION + "Connection Timeout=10;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";
                if (connection == null)
                {
                    tempPointer = new SqlConnection(currenStrConnection);
                    connection = tempPointer;
                }
                else tempPointer = connection;
            }

            //tempPointer apuntará al SqlConnection requerido
            //y desde ahora puede ser usado indistintamente


            if (tempPointer.State == System.Data.ConnectionState.Closed)
            {
                tempPointer.ConnectionString = ConexionBD.CADENA_CONEXION + "Connection Timeout=10;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";
                tempPointer.Open();
            }
            //Se puede cambiar a manera de factory, ssi las instancias son de 1 solo uso .

            /*if (ConexionBD.connection.State == System.Data.ConnectionState.Closed)
            {
                ConexionBD.connection.ConnectionString = ConexionBD.CADENA_CONEXION + "Connection Timeout=10;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;";
                ConexionBD.connection.Open();
            }*/


            return tempPointer;

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