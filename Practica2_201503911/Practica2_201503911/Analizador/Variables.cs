using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practica2_201503911.Analizador
{
    class Variables
    {
        String Nombre;
        String Tipo;
        String Dato;
        String Ambito;

        public Variables(String Nombre, String Tipo, String Dato, String Ambito)
        {
            this.Nombre = Nombre;
            this.Tipo = Tipo;
            this.Dato = Dato;
            this.Ambito = Ambito;
        }


        public String getId()
        {
            return Nombre;
        }
        public void setId(String Nombre)
        {
            this.Nombre = Nombre;
        }

        public String getTipo()
        {
            return Tipo;
        }
        public void setTipo(String Tipo)
        {
            this.Tipo = Tipo;
        }
        public String getDato()
        {
            return Dato;
        }
        public void setDato(String Dato)
        {
            this.Dato = Dato;
        }
        public String getAmbito()
        {
            return Ambito;
        }
        public void setAmbito(String Ambito)
        {
            this.Ambito = Ambito;
        }
    }
}
