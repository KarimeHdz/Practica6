using System;
using System.IO.Ports;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace Practica_Temp
{
    public partial class Form1 : Form
    {
        SerialPort serialPort;
        private string lastTemperature = "";
        public Form1()
        {
            InitializeComponent();

            serialPort = new SerialPort();
            serialPort.BaudRate = 9600;
            serialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);

            // Cargar los puertos disponibles en el ComboBox
            cbPorts.Items.AddRange(SerialPort.GetPortNames());
            if (cbPorts.Items.Count > 0)
                cbPorts.SelectedIndex = 0;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (cbPorts.SelectedItem == null)
            {
                MessageBox.Show("Por favor selecciona un puerto COM");
                return;
            }

            if (!serialPort.IsOpen)
            {
                serialPort.PortName = cbPorts.SelectedItem.ToString();
                try
                {
                    serialPort.Open();  // Abre el puerto serie
                    MessageBox.Show("Conectado correctamente", "Conexión exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnConnect.Text = "Desconectar";
                }
                catch (UnauthorizedAccessException)
                {
                    MessageBox.Show("Acceso denegado al puerto. Verifica que ninguna otra aplicación esté usando el puerto.", "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (IOException ioEx)
                {
                    MessageBox.Show($"Error de entrada/salida: {ioEx.Message}. Verifica si el puerto está conectado y disponible.", "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error inesperado: " + ex.Message, "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                serialPort.Close();
                MessageBox.Show("Conexión cerrada", "Desconectado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnConnect.Text = "Conectar";
            }
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                // Leer los datos completos del puerto serie
                string data = serialPort.ReadLine();
                Console.WriteLine($"Datos recibidos: {data}"); // Para verificar el valor recibido

                // Verificar si los datos contienen la temperatura (identificador "T:")
                if (data.StartsWith("T:"))
                {
                    // Extraer el valor de temperatura
                    string tempValue = data.Substring(2).Trim(); // Eliminar "T:" y espacios en blanco

                    // Almacenar el valor de la temperatura para mostrarlo posteriormente
                    lastTemperature = tempValue;
                    Console.WriteLine($"Temperatura extraída: {lastTemperature} °C"); // Para verificar el valor

                    // Asegurarse de que la actualización del control se haga en el hilo principal
                    this.Invoke(new Action(() =>
                    {
                        labelTemperature.Text = $"Temperatura: {tempValue} °C"; // Actualiza el label
                    }));
                }
                else
                {
                    Console.WriteLine("Datos recibidos no válidos: " + data); // Para verificar datos no válidos
                }
            }
            catch (Exception ex)
            {
                // Mostrar el error en un cuadro de diálogo usando Invoke para asegurar que se ejecute en el hilo principal
                this.Invoke(new Action(() =>
                {
                    MessageBox.Show("Error al leer los datos: " + ex.Message, "Error de lectura", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }));
            }
        }

        // Evento al hacer clic en el botón "Mostrar Temperatura"
        private void btnShowTemp_Click_1(object sender, EventArgs e)
        {
            Console.WriteLine("Botón Mostrar Temperatura presionado."); // Agrega esta línea

            // Verificar si hay una temperatura disponible
            if (!string.IsNullOrEmpty(lastTemperature))
            {
                // Mostrar la temperatura en el Label aparte
                lblShowTemp.Text = $"Última Temperatura: {lastTemperature} °C";
            }
            else
            {
                MessageBox.Show("No se ha recibido ninguna temperatura aún", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void labelTemperature_Click(object sender, EventArgs e)
        {

        }
    }
}
   
