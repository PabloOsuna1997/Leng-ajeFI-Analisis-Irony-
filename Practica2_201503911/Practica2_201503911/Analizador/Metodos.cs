using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practica2_201503911.Analizador
{
    class Metodos

    {
        String Nombre;
        String Tipo;
        String Dato;
        List<String> Parametros;
        ParseTreeNode Retorno;
        ParseTreeNode Cuerpo;

        public Metodos(String Nombre, String Tipo, List<String> Parametros, ParseTreeNode Retorno,ParseTreeNode Cuerpo)
        {
            this.Nombre = Nombre;
            this.Tipo = Tipo;
            this.Parametros = Parametros;
            this.Retorno = Retorno;
            this.Cuerpo = Cuerpo;
        }

        public List<String> getParametros()
        {
            return Parametros;
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
        public ParseTreeNode getRetorno()
        {
            return Retorno;
        }
        public void setRetorno(ParseTreeNode Retorno)
        {
            this.Retorno = Retorno;
        }
        public ParseTreeNode getCuerpo()
        {
            return Cuerpo;
        }
        public void setCuerpo(ParseTreeNode Cuerpo)
        {
            this.Cuerpo = Cuerpo;
        }
    }
}
