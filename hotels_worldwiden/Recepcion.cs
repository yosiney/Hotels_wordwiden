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
    public partial class Recepcion : Form
    {
        public Recepcion()
        {
            InitializeComponent();

            // Configurar la posición de inicio del formulario al centro de la pantalla
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new System.Drawing.Point(
                (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2,
                (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2
                );
        }

        private void Recepcion_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Habitaciones habitacion = new Habitaciones();
            habitacion.ShowDialog();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 back_login = new Form1();
            back_login.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Reservas reserva = new Reservas();
            reserva.ShowDialog();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            DialogResult resp = MessageBox.Show("Desea salir del sistema?", "Salir", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (resp == DialogResult.Yes)
            {
                Application.Exit();
            }
            try
            {
                using (SqlConnection bitacoraConnection = Conexion.Conectar())
                {
                    string query = "INSERT INTO Bitacora (fecha, accion, detalle, cedula) VALUES (@fecha, @accion, @detalle, @cedula)";

                    using (SqlCommand cmd2 = new SqlCommand(query, bitacoraConnection))
                    {
                        cmd2.Parameters.AddWithValue("@fecha", DateTime.Now);
                        cmd2.Parameters.AddWithValue("@accion", "Salida");
                        cmd2.Parameters.AddWithValue("@detalle", "Usuario Salio del sistema correctamente");
                        cmd2.Parameters.AddWithValue("@cedula", 444);

                        cmd2.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al insertar en bitácora: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }
    }
}
