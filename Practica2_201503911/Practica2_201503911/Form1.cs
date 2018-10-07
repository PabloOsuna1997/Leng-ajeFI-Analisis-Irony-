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
                int lineas = NuevaPestana.cantidadlineas;
                Contadorlineas.Clear();

                for (int i = 1; i <= lineas + 1; i++)
                {
                    Contadorlineas.AppendText(i + "\n");
                }
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
            Consola.Clear();
            try
            {
                bool resultado = instancia.analizar();

                if (resultado)
                {
                    Console.WriteLine("Cadena Correcta.\n\n");


                    List<String> errseman = new List<string>();
                    errseman = Analizador.Acciones.ErroresSemanticos;
                    for (int i = 0; i < errseman.Count; i++)
                    {
                        Consola.AppendText(errseman[i] + "\n");
                    }
                }

                else
                {
                    Console.WriteLine("Cadena Incorrecta.\n\n");

                    List<String> errosintac = new List<string>();
                    errosintac = Analizador.Analizar.erroressintacticos;
                    for (int i = 0; i < errosintac.Count; i++)
                    {

                        string remplaz0 = errosintac[i].Replace("expected", "Se esperaba");
                        errosintac[i] = remplaz0;
                        string remplaz1 = errosintac[i].Replace(",,", " ");
                        errosintac[i] = remplaz1;
                        Consola.AppendText(errosintac[i] + "\n");
                    }

                    List<String> errolexic = new List<string>();
                    errolexic = Analizador.Analizar.erroreslexicos;
                    for (int i = 0; i < errolexic.Count; i++)
                    {
                        Consola.AppendText(errolexic[i] + "\n");
                    }


                }


            }
            catch (Exception)
            {
                MessageBox.Show("Porfavor Agregue un texto de trabajo.");
            }
        }

        private void agregarPestañaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            instancia = new NuevaPestana(tabControl1, tabControl2);
            Contadorlineas.Clear();
        }

        private void tabControl1_MouseClick(object sender, MouseEventArgs e)
        {
            label2.Text = e.X.ToString() + "," + e.Y.ToString();
        }

        private void tabControl1_MouseMove(object sender, MouseEventArgs e)
        {

            label2.Text = e.X.ToString() + "  ,  " + e.Y.ToString();
        }
    }
}
