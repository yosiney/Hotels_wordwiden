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
    
    public partial class Gerencia : Form
    {

        public Gerencia()
        {
            InitializeComponent();
            // Configurar la posición de inicio del formulario al centro de la pantalla
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new System.Drawing.Point(
                (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2,
                (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2
                );
        }

        private void Gerencia_Load(object sender, EventArgs e)
        {

        }
        private void Limpiartexbox()
        {
            textcedula.Text = "";
            textnombre.Text = "";
            textcontra.Text = "";
            comboestado.Text = "";
            combotipousuario.Text = "";


        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            DialogResult resp = MessageBox.Show("Desea salir del sistema?", "Salir", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (resp == DialogResult.Yes)
            {
                // Cierra la aplicación cuando se cierra el formulario correspondiente al rol
                Application.Exit();
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 back_login = new Form1();
            back_login.ShowDialog();
            
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            string tipoUsuario = combotipousuario.SelectedItem.ToString();

            int tipoUsuarioid = 0; 

            if (tipoUsuario == "Gerente")
            {
                tipoUsuarioid = 1; 
            }
            else if (tipoUsuario == "Adminstrador de compras") 
            {
                tipoUsuarioid = 2;
            }
            else if (tipoUsuario == "Recepcion")
            {
                tipoUsuarioid = 3;
            }

            try 
            {
                string agregarUsuario = "INSERT INTO Usuarios (cedula, nombre, contraseña, estado, TipoUsuarioID) VALUES (@cedula, @nombre, @contraseña, @estado, @TipoUsuarioID);";

                using (SqlCommand cmdAgregarUsario = new SqlCommand(agregarUsuario, Conexion.Conectar()))
                {
                    cmdAgregarUsario.Parameters.AddWithValue("@cedula", int.Parse(textcedula.Text));
                    cmdAgregarUsario.Parameters.AddWithValue("@nombre", textnombre.Text);
                    cmdAgregarUsario.Parameters.AddWithValue("@contraseña", textcontra.Text);
                    cmdAgregarUsario.Parameters.AddWithValue("@estado", comboestado.SelectedItem.ToString());
                    cmdAgregarUsario.Parameters.AddWithValue("@TipoUsuarioID", tipoUsuarioid);
                    cmdAgregarUsario.ExecuteNonQuery();
                    Limpiartexbox();
                    MessageBox.Show("No se ha creado el usuario!!!");
                }

            }

            catch (Exception ex)
            {

                MessageBox.Show("Error al crear el usuario: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Limpiartexbox();
        }
    }
}
