﻿using System;
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
        int ReservaID = 0;
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
            groupBox1.Enabled = false;

            dataGridView1.DataSource = ObtenerHabitacioneslibres();
            dataGridView2.DataSource = ObtenerHabitacionesOcupadas();
        }

        public DataTable ObtenerHabitacioneslibres()
        {
            DataTable dt = new DataTable();
            string consulta = "select habitacionID, categoria, PrecioPorPersona, estado from Habitaciones where estado = 'disponible'";
            SqlCommand cmd = new SqlCommand(consulta, Conexion.Conectar());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }
        public DataTable ObtenerHabitacionesOcupadas()
        {
            DataTable dt = new DataTable();
            string consulta = "SELECT R.ReservaID, R.estado AS estadoReserva, H.HabitacionID, H.categoria, H.estado AS estadoHabitacion\r\nFROM Reservas AS R\r\nINNER JOIN Habitaciones AS H ON R.HabitacionID = H.HabitacionID\r\nWHERE H.estado = 'ocupada';";
            SqlCommand cmd = new SqlCommand(consulta, Conexion.Conectar());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

            this.Hide();
            Recepcion recepcion = new Recepcion();
            recepcion.ShowDialog();
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

        private void ejecutarReserva()
        {
            string EstadoHabitacion = "ocupada";
            string estadoreserva = "activa";

            int CeRecepcionista = int.Parse(textCeRecepcionista.Text);
            int NumeroHabitacion = int.Parse(texthabitacionID.Text);
            string cantidadPersonas = textCantidadP.Text;
            string cedulaCliente = TextCedulaCliente.Text;
            string fechaInicio = dateInicio.Text;
            string fechaFin = dateFin.Text;

            try
            {
                using (SqlConnection connection = Conexion.Conectar())
                {
                    // Aquí realizas la actualización en la base de datos
                    string query = "UPDATE Habitaciones SET estado = @estado WHERE HabitacionID = " + NumeroHabitacion + "";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@estado", EstadoHabitacion);
                        cmd.ExecuteNonQuery();

                    }

                    string queryreserva = "insert into Reservas (fechaInicio, fechaFin, estado, cedula, HabitacionID) values (@fechaInicio, @fechaFin, @estado, @cedula, @HabitacionID)";

                    using (SqlCommand cmd1 = new SqlCommand(queryreserva, connection))
                    {
                        cmd1.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                        cmd1.Parameters.AddWithValue("@fechaFin", fechaFin);
                        cmd1.Parameters.AddWithValue("@estado", estadoreserva);
                        cmd1.Parameters.AddWithValue("@cedula", CeRecepcionista);
                        cmd1.Parameters.AddWithValue("@HabitacionID", NumeroHabitacion);
                        cmd1.ExecuteNonQuery();

                    }

                }

                MessageBox.Show("Habitacion ocupada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dataGridView1.DataSource = ObtenerHabitacioneslibres();


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al hacer la reserva: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void ObtenerReservaID() 
        {
            int NumeroHabitacion = int.Parse(texthabitacionID.Text);
            try
            {

                using (SqlConnection connection = Conexion.Conectar())
                {
                    string query = "SELECT ReservaID FROM Reservas WHERE HabitacionID = @HabitacionID";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@habitacionID", NumeroHabitacion);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                ReservaID = Convert.ToInt32(reader["ReservaID"]);
                            }
                        }
                    }
                }
                labelReserva.Text = ReservaID.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            ejecutarReserva();

            int CeRecepcionista = int.Parse(textCeRecepcionista.Text);
            int NumeroHabitacion = int.Parse(texthabitacionID.Text);
            string cantidadPersonas = textCantidadP.Text;
            string cedulaCliente = TextCedulaCliente.Text;
            string fechaInicio = dateInicio.Text;
            string fechaFin = dateFin.Text;

            if (string.IsNullOrEmpty(textCeRecepcionista.Text) ||
                string.IsNullOrEmpty(texthabitacionID.Text) ||
                string.IsNullOrEmpty(textCantidadP.Text) ||
                string.IsNullOrEmpty(TextCedulaCliente.Text) ||
                string.IsNullOrEmpty(dateInicio.Text) ||
                string.IsNullOrEmpty(dateFin.Text))
            {
                MessageBox.Show("Hay campos vacios", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                int cantidadPersonasp = int.Parse(textCantidadP.Text);

                decimal precioHabitacion = 0;
                

                using (SqlConnection connection = Conexion.Conectar())
                {
                    string query = "SELECT PrecioPorPersona FROM Habitaciones WHERE HabitacionID = @habitacionID";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@habitacionID", NumeroHabitacion);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                precioHabitacion = Convert.ToDecimal(reader["PrecioPorPersona"]);
                            }
                        }
                    }
                }

                decimal PrecioTotalPorPagar = cantidadPersonasp * precioHabitacion;

                ObtenerReservaID();
                
                labelTotal.Text = PrecioTotalPorPagar.ToString();
                labelpreciopersona.Text = precioHabitacion.ToString();

                groupBox1.Enabled = true;
                groupBox2.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al hacer la reserva: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {
            dataGridView2.DataSource = ObtenerHabitacionesOcupadas();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            string disponible = "disponible";
            string finalizada = "finalizada";

            int NumeroHabitacion = int.Parse(textdesocupar.Text);
            int NumeroReservacion = int.Parse(textfinalizarreserva.Text);
            try
            {
                using (SqlConnection connection = Conexion.Conectar())
                {
                    // Aquí realizas la actualización en la base de datos
                    string query = "UPDATE Habitaciones SET estado = @estado WHERE HabitacionID = " + NumeroHabitacion + "";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@estado", disponible);
                        cmd.ExecuteNonQuery();

                    }

                    string queryreserva = "UPDATE Reservas SET estado = @estado WHERE HabitacionID = " + NumeroReservacion + "";

                    using (SqlCommand cmd1 = new SqlCommand(queryreserva, connection))
                    {
                        cmd1.Parameters.AddWithValue("@estado", finalizada);
                        cmd1.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Habitacion desocupada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dataGridView2.DataSource = ObtenerHabitacionesOcupadas();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al desocupar la habitacion: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DataGridViewRow filaSeleccionada = dataGridView2.CurrentRow;

            if (filaSeleccionada != null)
            {
                int ReservaID = Convert.ToInt32(filaSeleccionada.Cells["ReservaID"].Value);
                string EstadoReserva = filaSeleccionada.Cells["estadoReserva"].Value.ToString();
                string habitacionID = filaSeleccionada.Cells["HabitacionID"].Value.ToString();
                string categoria = filaSeleccionada.Cells["categoria"].Value.ToString();
                string EstadoHabitacion = filaSeleccionada.Cells["estadoHabitacion"].Value.ToString();

                textdesocupar.Text = ReservaID.ToString();
                textfinalizarreserva.Text = habitacionID.ToString();
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            return;
        }

        private void BPagar_Click(object sender, EventArgs e)
        {
            decimal iva = 13;
             try
             {
                string agregarReserva = "INSERT INTO Factura (fechaFactura, montoTotal, iva, observaciones, metodoPago, tipoMoneda, ReservaID) VALUES (@fechaFactura, @montoTotal, @iva, @observaciones, @metodoPago, @tipoMoneda, @ReservaID);";

                using (SqlCommand cmdAgregarUsario = new SqlCommand(agregarReserva, Conexion.Conectar()))
                {
                    cmdAgregarUsario.Parameters.AddWithValue("@fechaFactura", DateTime.Now);
                    cmdAgregarUsario.Parameters.AddWithValue("@montoTotal", labelTotal.Text);
                    cmdAgregarUsario.Parameters.AddWithValue("@iva", iva);
                    cmdAgregarUsario.Parameters.AddWithValue("@observaciones", textObservaciones.Text);
                    cmdAgregarUsario.Parameters.AddWithValue("@metodoPago", combotipodepago.Text);
                    cmdAgregarUsario.Parameters.AddWithValue("@tipoMoneda", comboTipoMoneda.Text);
                    cmdAgregarUsario.Parameters.AddWithValue("@ReservaID", int.Parse(labelReserva.Text));
                    cmdAgregarUsario.ExecuteNonQuery();
                    MessageBox.Show("Se ha creado el la reserva!!!");
                }
                
             }
              catch (Exception ex)
             {

                MessageBox.Show("Error al crear el usuario: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
             }



            ejecutarReserva();
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }
    }
}
