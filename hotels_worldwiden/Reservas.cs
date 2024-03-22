using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hotels_worldwiden
{
    public partial class Reservas : Form
    {
        public Reservas()
        {
            InitializeComponent();
        }
        public DataTable Obtenerreservas()
        {
            DataTable dt = new DataTable();
            string consulta = "select * from Reservas";
            SqlCommand cmd = new SqlCommand(consulta, Conexion.Conectar());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }
        private void Reservas_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = Obtenerreservas();   
        }
    }
}
