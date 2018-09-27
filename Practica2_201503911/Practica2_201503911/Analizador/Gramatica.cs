using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;

namespace Practica2_201503911.Analizador
{
    class Gramatica : Grammar
    {
        public Gramatica() : base(caseSensitive: true)
        {


            //------------>
            #region ExpresionesRegulares
            //numeros 
            RegexBasedTerminal Entero = new RegexBasedTerminal("Entero", "[0-9]+");
            #endregion

            #region Terminal
            var tkINT = ToTerm("int");
            var tkFLOAT = ToTerm("float");
            var tkBOOL = ToTerm("bool");

            #endregion

            //------------>
            #region Non Terminal
            NonTerminal INICIO = new NonTerminal("INICIO");

            #endregion

            //------------>
            #region Gramatica

            INICIO.Rule = tkINT
                         | tkFLOAT
                         | tkBOOL;
            #endregion

            //preferencias
            //------------>
            #region EstadoInicio  
            this.Root = INICIO;
            #endregion
        }
    }
}
