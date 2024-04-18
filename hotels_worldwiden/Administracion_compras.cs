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
    public partial class Administracion_compras : Form

    {

        public Administracion_compras()
        {
            InitializeComponent();
            // Configurar la posición de inicio del formulario al centro de la pantalla
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new System.Drawing.Point(
                (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2,
                (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2
                );

           
        }

        private void Administracion_compras_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 back_login = new Form1();
            back_login.ShowDialog();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            DialogResult resp = MessageBox.Show("Desea salir del sistema?", "Salir", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (resp == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int Proveedor = 0;
            string seleccionProveedor = comboBox1.SelectedItem.ToString();

            if (string.IsNullOrEmpty(textBox1.Text) ||
                string.IsNullOrEmpty(textBox2.Text) ||
                string.IsNullOrEmpty(textBox3.Text) ||
                string.IsNullOrEmpty(textBox4.Text) ||
                string.IsNullOrEmpty(textBox5.Text))
            {
                MessageBox.Show("Hay campos vacios", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {


                switch (seleccionProveedor)
                {
                    case "maxipali":
                        Proveedor = 1;
                        break;
                    case "teleria":
                        Proveedor = 2;
                        break;
                    case "colono":
                        Proveedor = 3;
                        break;
                    case "carniceria":
                        Proveedor = 4;
                        break;
                    case "ferreteria":
                        Proveedor = 5;
                        break;
                    case "muebleria":
                        Proveedor = 6;
                        break;
                    default:
                        MessageBox.Show("Proveedor desconocido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                }


                DateTime fechaActual = DateTime.Now;
                string solicitado = "solicitado";

                using (SqlConnection connection = Conexion.Conectar())
                {

                    string queryreserva = "insert into Compras (producto, cantidad, precioUnitario, descripcion, fechaSolicitud, estado, cedula, ProveedorID) values (@producto, @cantidad, @precioUnitario, @descripcion, @fechaSolicitud, @estado, @cedula, @ProveedorID)";

                    using (SqlCommand cmd1 = new SqlCommand(queryreserva, connection))
                    {
                        cmd1.Parameters.AddWithValue("@producto", textBox1.Text);
                        cmd1.Parameters.AddWithValue("@cantidad", int.Parse(textBox2.Text));
                        cmd1.Parameters.AddWithValue("@precioUnitario", int.Parse(textBox3.Text));
                        cmd1.Parameters.AddWithValue("@descripcion", textBox4.Text);
                        cmd1.Parameters.AddWithValue("@fechaSolicitud", fechaActual);
                        cmd1.Parameters.AddWithValue("@estado", solicitado);
                        cmd1.Parameters.AddWithValue("@cedula", int.Parse(textBox5.Text));
                        cmd1.Parameters.AddWithValue("@ProveedorID", Proveedor);
                        cmd1.ExecuteNonQuery();

                    }

                }

                MessageBox.Show("Solicitud de compra realizada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al hacer la solicitud " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
