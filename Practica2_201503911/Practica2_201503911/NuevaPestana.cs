using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Irony.Parsing;
using Practica2_201503911.Analizador;

namespace Practica2_201503911
{
    class NuevaPestana
    {
        TabControl Texto;
        TabControl Salida;
        RichTextBox Area;
        public static TabPage Nuevapest;
        public static TabPage Salidapest;
        List<String> Errores = new List<string>();
        public static int cantidadlineas = 0;
        public NuevaPestana(TabControl Texto, TabControl Salida)
        {
            this.Texto = Texto;
            this.Salida = Salida;

            crearNuevaPestaña();
        }

        public void crearNuevaPestaña()
        {

            Nuevapest = new TabPage("Pestaña");
            Area = new RichTextBox();

            Area.SetBounds(1, 1, 460, 236);
            Area.AcceptsTab = true;
            Nuevapest.Controls.Add(Area);

            Salidapest = new TabPage("Salida");
            RichTextBox salida = new RichTextBox();
            salida.SetBounds(1, 1, 396, 236);
            salida.AcceptsTab = true;
            Salidapest.Controls.Add(salida);

            Texto.TabPages.Add(Nuevapest);
            Salida.TabPages.Add(Salidapest);

            Texto.SelectedIndex = Texto.SelectedIndex + 1;
            Salida.SelectedIndex = Salida.SelectedIndex + 1;
        }


        //captura la pestaña seleccionada de entradaa
        public TabPage PestañaSeleccionda()
        {
            return Texto.SelectedTab;
        }
        //captura la pestaña seleccionada de salida
        public TabPage PestañaSelecciondaSalida()
        {
            return Salida.SelectedTab;
        }

        //metodo que mandara a analizar el texto escrito
        public bool analizar()
        {
            ParseTreeNode Root;
            //se captura la pestaña seleccionada y el siguiente componente que sera el textarea, luego se parseara a string
            String TextoAanalizar = PestañaSeleccionda().GetNextControl(PestañaSeleccionda(), true).Text;

            if (!TextoAanalizar.Equals(""))
            {
                Root = Analizar.analizador(TextoAanalizar);        //llamo al metodo analizar de la clase analizar por ende esta public y estatica

                if (Root != null)
                {
                    Acciones.RealizarAcciones(Root);

                    PestañaSelecciondaSalida().GetNextControl(PestañaSelecciondaSalida(), true).Text = " ";
                    List<String> impresiones = Analizador.Acciones.Impresiones;
                    String impre = "";
                    for (int i = 0; i < impresiones.Count; i++)
                    {
                        impre += impresiones[i] + "\n";
                        
                    }
                    PestañaSelecciondaSalida().GetNextControl(PestañaSelecciondaSalida(), true).Text = impre;
                    Analizador.Acciones.Impresiones.Clear();

                    return true;
                }
                else
                {

                    return false;
                }
            }
            else
            {
                MessageBox.Show("Area Vacia", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return false;
            }

        }

        public void abrir()
        {
            try
            {
                OpenFileDialog abrir = new OpenFileDialog();
                abrir.Filter = "All Files (*.fi)|*.fi"; // tipos de formatos
                if (abrir.ShowDialog() == System.Windows.Forms.DialogResult.OK && abrir.FileName.Length > 0)
                {
                    Area.LoadFile(abrir.FileName, RichTextBoxStreamType.PlainText);
                    String path1 = System.IO.Path.GetFullPath(abrir.FileName);
                    Nuevapest.Text = System.IO.Path.GetFileNameWithoutExtension(path1);
                    Salidapest.Text = System.IO.Path.GetFileNameWithoutExtension(path1);
                }

                Control con =  PestañaSeleccionda().GetNextControl(PestañaSeleccionda(), true);
                RichTextBox ri = (RichTextBox)con;

                 cantidadlineas = ri.Lines.Length;

            }
            catch (Exception)
            {
                MessageBox.Show("No se pudo abrir el archivo.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

    }
}
