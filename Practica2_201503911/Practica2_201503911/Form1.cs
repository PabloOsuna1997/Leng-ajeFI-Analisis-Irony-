using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Practica2_201503911
{
    public partial class Form1 : Form
    {
        NuevaPestana instancia;
        public Form1()
        {
            InitializeComponent();
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                instancia = new NuevaPestana(tabControl1, tabControl2);
                instancia.abrir();
            }
            catch (Exception)
            {
                MessageBox.Show("Agregar Pestaña Primero", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }           
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void analizarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool resultado = instancia.analizar();
            
            if (resultado) Console.WriteLine("Cadena Correcta.");
            else Console.WriteLine("Cadena Incorrecta.");
        }

        private void agregarPestañaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            instancia = new NuevaPestana(tabControl1, tabControl2);
        }
    }
}
