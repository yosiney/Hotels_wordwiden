using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotels_worldwiden
{
    internal class Conexion
    {
        public static SqlConnection Conectar()
        {
            //Yosiney: DESKTOP-VTB1K29\SQLEXPRESS//
            SqlConnection conectar = new SqlConnection("Server=DESKTOP-VTB1K29\\SQLEXPRESS;Database=hotels_worldwiden3;Integrated Security=True;");
            conectar.Open();
            return conectar;
        }
    }
}
