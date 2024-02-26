﻿using hotels_worldwiden;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace hotels_worldwiden
{

    public partial class Form1 : Form
    {
        int contarbloqueo = 0;

        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            

            try
            {
                string selectQuery = "SELECT usuarioID, contraseña, estado FROM Usuarios WHERE CAST(usuarioID AS NVARCHAR(50)) = @usuarioID";

                using (SqlConnection connection = Conexion.Conectar())
                {
                    using (SqlCommand cmd = new SqlCommand(selectQuery, connection))
                    {
                        cmd.Parameters.Add("@usuarioID", SqlDbType.NVarChar).Value = textcedula.Text;

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Se encontró el usuario, ahora verifica la clave
                                string cedulaEnBaseDeDatos = reader["usuarioID"].ToString();
                                string claveEnBaseDeDatos = reader["contraseña"].ToString();
                                string estadoEnbaseDeDatos = reader["estado"].ToString();


                                if (estadoEnbaseDeDatos == "disponible")
                                {

                                    // Verifica si la clave ingresada coincide con la clave en la base de datos
                                    if (claveEnBaseDeDatos == textContraseña.Text)
                                    {
                                        // Consulta el rol del usuario
                                        string consultaRol = "SELECT r.Descripcion FROM Usuarios " +
                                                            "INNER JOIN tipoUsuario AS r ON Usuarios.TipoUsuarioID = r.TipoUsuarioID " +
                                                            "WHERE usuarioID = @usuarioID";

                                        // Cierra el primer DataReader antes de abrir el segundo
                                        reader.Close();

                                        using (SqlCommand cmdRol = new SqlCommand(consultaRol, connection))
                                        {
                                            cmdRol.Parameters.Add("@usuarioID", SqlDbType.NVarChar).Value = textcedula.Text;

                                            using (SqlDataReader readerRol = cmdRol.ExecuteReader())
                                            {
                                                if (readerRol.Read())
                                                {
                                                    string nombreRol = readerRol["Descripcion"].ToString();

                                                    // Oculta la pantalla de inicio de sesión (login)
                                                    this.Hide();

                                                    // Abre la pantalla correspondiente al rol
                                                    if (nombreRol == "Gerente")
                                                    {
                                                        Gerencia gerente = new Gerencia();
                                                        gerente.ShowDialog();
                                                    }
                                                    else if (nombreRol == "Recepsionista")
                                                    {
                                                        Recepcion recepsionista = new Recepcion();
                                                        recepsionista.ShowDialog();
                                                    }
                                                    else if (nombreRol == "Comprador")
                                                    {
                                                        Administracion_compras comprador = new Administracion_compras();
                                                        comprador.ShowDialog();
                                                    }


                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        int cedula = int.Parse(textcedula.Text);
                                        contarbloqueo++;
                                        if (contarbloqueo == 2)
                                        {
                                            string textbloqueado = "bloqueado";

                                            string actualizarEstado = "UPDATE Usuarios set estado = @estado where usuarioID = " + cedula+"";

                                            SqlCommand cmdActualizarEstado = new SqlCommand(actualizarEstado, Conexion.Conectar());
                                            {
                                                cmdActualizarEstado.Parameters.AddWithValue("@estado", textbloqueado);
                                                cmdActualizarEstado.ExecuteNonQuery();
                                                MessageBox.Show("El usuario se ha bloqueado. Por favor, contacta al administrador.", "Cuenta Bloqueada", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                            }

                                        }
                                        textContraseña.BorderStyle = BorderStyle.FixedSingle;
                                        label1.Text = "La contraseña no coincide con el usuario.";
                                    }
                                }
                                else
                                {
                                    // Cuenta bloqueada, mostrar mensaje y salir
                                    MessageBox.Show("El usuario se encuentra bloqueado. Por favor, contacta al administrador.", "Cuenta Bloqueada", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                }

                                
                            }
                            else
                            {
                                textcedula.BorderStyle = BorderStyle.FixedSingle;

                                label1.Text = "No hay registro de la cedula insertada";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones (por ejemplo, registro, mensaje al usuario, etc.)
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private void Form1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textNombre_MouseDown(object sender, MouseEventArgs e)
        {

        }
    }
}
