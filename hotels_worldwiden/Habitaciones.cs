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
    public partial class Habitaciones : Form
    {
        public Habitaciones()
        {
            InitializeComponent();
            // Configurar la posición de inicio del formulario al centro de la pantalla
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new System.Drawing.Point(
                (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2,
                (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2
                ); 
        }

        private void Habitaciones_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = ObtenerHabitacioneslibres();
        }

        public DataTable ObtenerHabitacioneslibres()
        {
            DataTable dt = new DataTable();
            string consulta = "select habitacionID, categoria, PrecioPorPersona, estado from Habitaciones";
            SqlCommand cmd = new SqlCommand(consulta, Conexion.Conectar());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }
    }
}
