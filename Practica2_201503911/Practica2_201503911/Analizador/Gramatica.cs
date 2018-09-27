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
            //id
            IdentifierTerminal Id = new IdentifierTerminal("Id", "([a-zA-Z] |_)([a-zA-Z] |[0-9]|_)*");


            #endregion

            #region Terminal
            var tkVOID = ToTerm("void");
            var tkINT = ToTerm("int");
            var tkFLOAT = ToTerm("float");
            var tkBOOL = ToTerm("bool");
            var tkCHAR = ToTerm("char*");

            var tkMAIN = ToTerm("main");
            var tkRETURN = ToTerm("return");
            var tkTRUE = ToTerm("true");
            var tkFALSE = ToTerm("false");
            var tkFOR = ToTerm("for");
            var tkIF = ToTerm("if");
            var tkELSE = ToTerm("else");
            var tkDO = ToTerm("do");
            var tkWHILE = ToTerm("while");

            //Simbolos
            var tkMAS = ToTerm("+");
            var tkMENOS = ToTerm("-");
            var tkPOR = ToTerm("*");
            var tkDIV = ToTerm("/");
            var tkPORCENT = ToTerm("%");
            var tkPARA = ToTerm("(");
            var tkPARC = ToTerm(")");
            var tkLLAVA = ToTerm("{");
            var tkLLAVC = ToTerm("}");
            var tkMENOR = ToTerm("<");
            var tkMAYOR = ToTerm(">");
            var tkMENORIGUAL = ToTerm("<=");
            var tkMAYORIGUAL = ToTerm(">=");
            var tkIGUAL = ToTerm("=");
            var tkDISTINTO = ToTerm("!");
            var tkOR = ToTerm("||");
            var tkAND = ToTerm("&&");
            var tkPUNTO = ToTerm(".");
            var tkPUNTOYCOMA = ToTerm(";");            

            #endregion

            //------------>
            #region Non Terminal
            NonTerminal INICIO = new NonTerminal("INICIO"),
                PARAMETROS = new NonTerminal("PARAMETROS"),
                SENTENCIAS = new NonTerminal("SENTENCIAS");

            #endregion

            //------------>
            #region Gramatica

            INICIO.Rule = tkINT + tkMAIN + tkPARA + tkPARC + tkLLAVA +SENTENCIAS + tkRETURN + "0" + ";" + tkLLAVC
                         ;

            SENTENCIAS.Rule = "hola";
            #endregion

            //preferencias
            //------------>
            #region EstadoInicio  
            this.Root = INICIO;
            #endregion
        }
    }
}
