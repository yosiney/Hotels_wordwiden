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

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Recepcion back_recepcion = new Recepcion();
            back_recepcion.ShowDialog();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            DialogResult resp = MessageBox.Show("Desea salir del sistema?", "Salir", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (resp == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
