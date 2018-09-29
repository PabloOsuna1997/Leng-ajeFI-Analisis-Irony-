﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Practica2_201503911.Analizador;

namespace Practica2_201503911
{
    class NuevaPestana
    {
        TabControl Texto;
        TabControl Salida;
        RichTextBox Area;
        TabPage Nuevapest;
        TabPage Salidapest;
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
            Area.SetBounds(1, 1, 430, 230);
            Area.AcceptsTab = true;
            Nuevapest.Controls.Add(Area);

            Salidapest = new TabPage("Salida");
            RichTextBox salida = new RichTextBox();
            salida.SetBounds(1, 1, 440, 230);
            salida.AcceptsTab = true;
            Salidapest.Controls.Add(salida);

            Texto.TabPages.Add(Nuevapest);
            Salida.TabPages.Add(Salidapest);

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
        public void analizar()
        {
            bool resultado = false;
            //se captura la pestaña seleccionada y el siguiente componente que sera el textarea, luego se parseara a string
            String TextoAanalizar = PestañaSeleccionda().GetNextControl(PestañaSeleccionda(), true).Text;
            if (!TextoAanalizar.Equals(""))
            {
                resultado = Analizar.analizador(TextoAanalizar);        //llamo al metodo analizar de la clase analizar por ende esta public y estatica
         
                if (resultado) Console.WriteLine("Cadena Correcta.");
                else Console.WriteLine("Cadena Incorrecta.");

                //return resultado;
            }
            else
            {                     
                MessageBox.Show("Area Vacia", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }

        }

        public void abrir()
        {
            try
            {
                OpenFileDialog abrir = new OpenFileDialog();
                abrir.Filter = "All Files (*.xml)|*.xml"; // tipos de formatos
                if (abrir.ShowDialog() == System.Windows.Forms.DialogResult.OK && abrir.FileName.Length > 0)
                {
                    Area.LoadFile(abrir.FileName, RichTextBoxStreamType.PlainText);
                    String path1 = System.IO.Path.GetFullPath(abrir.FileName);
                    Nuevapest.Text = System.IO.Path.GetFileNameWithoutExtension(path1);
                    Salidapest.Text = System.IO.Path.GetFileNameWithoutExtension(path1);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("No se pudo abrir el archivo.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }
    }
}
