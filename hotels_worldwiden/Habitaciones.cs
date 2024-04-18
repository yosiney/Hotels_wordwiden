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
            string consulta = "SELECT R.ReservaID, R.estado AS estadoReserva, H.HabitacionID, H.categoria, H.estado AS estadoHabitacion\r\nFROM Reservas AS R\r\nINNER JOIN Habitaciones AS H ON R.HabitacionID = H.HabitacionID\r\nWHERE H.estado = 'ocupada' AND R.estado = 'activa';\r\n";
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

            int NumeroHabitacion = int.Parse(textDesocupa.Text);
            int NumeroReservacion = int.Parse(textfinalizar.Text);
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

                    string queryreserva = "UPDATE Reservas SET estado = @estado WHERE ReservaID = " + NumeroReservacion + "";

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

                textfinalizar.Text = ReservaID.ToString();
                textDesocupa.Text = habitacionID.ToString();
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
                    
                }

                switch (combotipodepago.SelectedItem.ToString())
                {
                    case "Efectivo":
                        groupBox5.Visible = true;
                        groupBox7.Visible = false;
                        groupBox6.Visible = false;
                        break;
                    case "Tarjeta":
                        groupBox7.Visible = true;
                        groupBox6.Visible = false;
                        groupBox5.Visible = false;
                        break;
                    default:
                        groupBox6.Visible = true;
                        groupBox5.Visible = false;
                        groupBox7.Visible = false;
                        break;
                }

            }
              catch (Exception ex)
             {

                MessageBox.Show("Error al crear el usuario: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }











        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView2.DataSource = ObtenerHabitacionesOcupadas();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = ObtenerHabitacioneslibres();
        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                decimal precioTotal = decimal.Parse(labelTotal.Text);
                decimal precioPersona = decimal.Parse(labelpreciopersona.Text);
                decimal montoPagado = decimal.Parse(textBox1.Text);
                int tipocambio = 600;

                decimal cobrarEndolar = precioTotal / tipocambio;

                if (comboTipoMoneda.SelectedItem.ToString() == "Colon")
                {
                    if (montoPagado >= precioTotal)
                    {

                        decimal vuelto = montoPagado - precioTotal;


                        MessageBox.Show($"El vuelto es: {vuelto} colones");

                    }
                    else
                    {
                        MessageBox.Show($"Error, está pagando con menos");
                    }
                }
                else
                {
                    if (montoPagado >= cobrarEndolar)
                    {

                        decimal vuelto = montoPagado - cobrarEndolar;

                        // Verificar si la moneda seleccionada es dólares y convertir el vuelto a colones si es necesario
                        if (comboTipoMoneda.SelectedItem.ToString() == "Dolar")
                        {
                            decimal vueltocolon = tipocambio * vuelto;
                            vueltocolon = Math.Round(vueltocolon, 2); // Redondear a dos decimales
                            MessageBox.Show($"El vuelto es: {vueltocolon} colones");
                            MessageBox.Show("Se ha creado la factura!!!");
                            
                        }
                        else
                        {
                            vuelto = Math.Round(vuelto, 2); // Redondear a dos decimales
                            MessageBox.Show($"El vuelto es: {vuelto}");
                            MessageBox.Show("Se ha creado la factura!!!");
                            limpiartodo();
                        }
                        textBox1.Text = "";
                        groupBox5.Visible = false;
                        limpiartodo();
                    }
                    else
                    {
                        MessageBox.Show($"Error, está pagando con menos");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al calcular el vuelto: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }



        private void labelReserva_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                decimal precioTotal = decimal.Parse(labelTotal.Text);
                decimal precioPersona = decimal.Parse(labelpreciopersona.Text);
                decimal montoEfectivo = decimal.Parse(textBox2.Text);
                decimal montoTarjeta = decimal.Parse(textBox3.Text); // Supongamos que el monto con tarjeta se ingresa en otro campo
                int tipocambio = 600;

                // Calcular el total pagado
                decimal totalPagado = montoEfectivo + montoTarjeta;

                decimal cobrarEndolar = precioTotal / tipocambio;

                if (comboTipoMoneda.SelectedItem.ToString() == "Colon")
                {
                    if (totalPagado >= precioTotal)
                    {
                        decimal vuelto = totalPagado - precioTotal;
                        MessageBox.Show($"El vuelto es: {vuelto} colones");
                        MessageBox.Show("Se ha creado la factura!!!");
                        textBox2.Text = "";
                        textBox3.Text = "";
                        groupBox6.Visible = false;
                        limpiartodo();
                    }
                    else
                    {
                        MessageBox.Show($"Error, está pagando con menos");
                    }
                }
                else // En este caso, se asume que el tipo de moneda es "Dolar"
                {
                    if (totalPagado >= cobrarEndolar)
                    {
                        decimal vueltoEnDolares = totalPagado - cobrarEndolar;

                        // Convertir el vuelto en dólares a colones si es necesario
                        decimal vueltoEnColones = tipocambio * vueltoEnDolares;
                        vueltoEnColones = Math.Round(vueltoEnColones, 2); // Redondear a dos decimales

                        MessageBox.Show($"El vuelto es: {vueltoEnColones} colones");
                        MessageBox.Show("Se ha creado la factura!!!");
                        textBox2.Text = "";
                        textBox3.Text = "";
                        groupBox6.Visible = false;
                        limpiartodo();
                    }
                    else
                    {
                        MessageBox.Show($"Error, está pagando con menos");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al calcular el vuelto: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                decimal precioTotal = decimal.Parse(labelTotal.Text);
                decimal montoTarjeta = decimal.Parse(textBox4.Text);
                int tipocambio = 600;

                decimal cobrarEndolar = precioTotal / tipocambio;

                if (montoTarjeta >= cobrarEndolar)
                {
                    MessageBox.Show("Pago realizado con tarjeta correctamente.");
                    MessageBox.Show("Se ha creado la factura!!!");
                    textBox4.Text = "";
                    groupBox7.Visible = false;
                    limpiartodo();
                }
                else
                {
                    MessageBox.Show($"Error, el monto pagado con tarjeta es insuficiente");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al procesar el pago con tarjeta: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void limpiartodo() 
        {
            labelReserva.Text = "_____";
            labelpreciopersona.Text = "_____";
            labelTotal.Text = "_____";

            combotipodepago.Text = "";
            comboTipoMoneda.Text = "";
            textObservaciones.Text = "";

            textCeRecepcionista.Text = "";
            texthabitacionID.Text = "";
            textCantidadP.Text = "";
            TextCedulaCliente.Text = "";
            textNombreCliente.Text = "";
            dateInicio.Text = "";
            dateFin.Text = "";

            groupBox1.Enabled = false;
            groupBox2.Enabled = true;

        }


        private void label18_Click(object sender, EventArgs e)
        {

        }
    }
}
