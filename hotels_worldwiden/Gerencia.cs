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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
            dataGridView1.DataSource = Obtenerusuarios();
            dataGridView2.DataSource = Obtenerusuarios();
            dataGridView3.DataSource = Obtenerusuarios();
            dataGridView4.DataSource = ObtenerBitacora();
            dataGridView5.DataSource = ObtenerIva();
        }
        public DataTable ObtenerIva() 
        {
            DataTable dt = new DataTable();
            string consulta = "SELECT IvaID, Iva from Iva where IvaID = 1";
            SqlCommand cmd = new SqlCommand(consulta, Conexion.Conectar());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }
        public DataTable Obtenerusuarios()
        {
            DataTable dt = new DataTable();
            string consulta = "SELECT * from Usuarios";
            SqlCommand cmd = new SqlCommand(consulta, Conexion.Conectar());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }
        public DataTable ObtenerBitacora()
        {
            DataTable dt = new DataTable();
            string consulta = "SELECT * from bitacora";
            SqlCommand cmd = new SqlCommand(consulta, Conexion.Conectar());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
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
                    MessageBox.Show("Se ha creado el usuario!!!");
                    
                }
                dataGridView2.DataSource = Obtenerusuarios();
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

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            DataGridViewRow filaSeleccionada = dataGridView1.CurrentRow;

            if (filaSeleccionada != null)
            {
                int cedula = Convert.ToInt32(filaSeleccionada.Cells["cedula"].Value);
                string nombre = filaSeleccionada.Cells["nombre"].Value.ToString();
                string contraseña = filaSeleccionada.Cells["contraseña"].Value.ToString();
                string estado = filaSeleccionada.Cells["estado"].Value.ToString();
                string TipoUsuarioID = filaSeleccionada.Cells["TipoUsuarioID"].Value.ToString();

                string tipoUsuarioid = "";

                if (TipoUsuarioID == "1")
                {
                    tipoUsuarioid = "Gerente";
                }
                else if (TipoUsuarioID == "2")
                {
                    tipoUsuarioid = "Adminstrador de compras";
                }
                else if (TipoUsuarioID == "3")
                {
                    tipoUsuarioid = "Recepcionista";
                }

                textseCedula.Text = cedula.ToString();
                textseNombre.Text = nombre;
                textseContra.Text = contraseña;
                comboseEstado.Text = estado;
                comboseTipo.Text = tipoUsuarioid;
            }
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textseCedula.Text) ||
                string.IsNullOrEmpty(textseNombre.Text) ||
                string.IsNullOrEmpty(textseContra.Text) ||
                string.IsNullOrEmpty(comboseEstado.Text) ||
                string.IsNullOrEmpty(comboseTipo.Text))
             {
                MessageBox.Show("Hay campos vacios", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
             }

            try
            {
                using (SqlConnection connection = Conexion.Conectar())
                {
                    string tipoUsuario = comboseTipo.SelectedItem.ToString();

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
                    // Aquí realizas la actualización en la base de datos
                    string query = "UPDATE Usuarios SET nombre = @nombre, contraseña = @contraseña, estado = @estado, TipoUsuarioID = @TipoUsuarioID WHERE cedula = @cedula";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@cedula", int.Parse(textseCedula.Text));
                        cmd.Parameters.AddWithValue("@nombre", textseNombre.Text);
                        cmd.Parameters.AddWithValue("@contraseña", textseContra.Text);
                        cmd.Parameters.AddWithValue("@estado", comboseEstado.Text);
                        cmd.Parameters.AddWithValue("@TipoUsuarioID", tipoUsuarioid);
                        cmd.ExecuteNonQuery();

                    }
                }

                MessageBox.Show("Usuario actualizado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dataGridView1.DataSource = Obtenerusuarios();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar usuario: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboseTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboseTipo.DropDownStyle = ComboBoxStyle.DropDownList;

        }

        private void comboseEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboseEstado.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void comboestado_SelectedIndexChanged(object sender, EventArgs e)
        {
            combotipousuario.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        private void EliminarUsuario(int cedula)
        {
            try
            {
                using (SqlConnection connection = Conexion.Conectar())
                {
                    // Aquí realizas la eliminación en la base de datos
                    string query = "DELETE FROM Usuarios WHERE cedula = @cedula";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@cedula", cedula);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Usuario eliminado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dataGridView3.DataSource = Obtenerusuarios();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar usuario: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void pictureBox8_Click(object sender, EventArgs e)
        {
            // Obtener la fila seleccionada en el DataGridView
            DataGridViewRow filaSeleccionada = dataGridView3.CurrentRow;

            if (filaSeleccionada != null)
            {
                // Obtener el nombre del usuario de la fila seleccionada
                string nombreUsuario = filaSeleccionada.Cells["Nombre"].Value.ToString();

                // Mostrar un mensaje de confirmación
                DialogResult resultado = MessageBox.Show($"¿Estás seguro de eliminar al usuario {nombreUsuario}?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (resultado == DialogResult.Yes)
                {
                    // Obtener la cédula del usuario
                    int cedula = Convert.ToInt32(filaSeleccionada.Cells["Cedula"].Value);

                    // Llamar a la función para eliminar al usuario
                    EliminarUsuario(cedula);
                }

            }
            else
            {
                MessageBox.Show("Selecciona un usuario antes de intentar eliminar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataGridViewRow filaSeleccionada = dataGridView5.CurrentRow;

            if (filaSeleccionada != null)
            {
                decimal Iva = Convert.ToDecimal(filaSeleccionada.Cells["Iva"].Value);
                decimal IvaID = Convert.ToDecimal(filaSeleccionada.Cells["IvaID"].Value);

                textBox1.Text = Iva.ToString();
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = Conexion.Conectar())
                {
                    decimal newIva = Convert.ToDecimal(textBox1.Text);
                    string queryIva = "UPDATE Iva SET Iva = @Iva WHERE IvaID = 1";

                    using (SqlCommand cmd1 = new SqlCommand(queryIva, connection))
                    {
                        cmd1.Parameters.AddWithValue("@Iva", newIva);
                        cmd1.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Iva actualizado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dataGridView5.DataSource = ObtenerIva();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al desocupar la habitacion: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
