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
            IdentifierTerminal Id = new IdentifierTerminal("Id", "([a-zA-Z]|_)([a-zA-Z]|[0-9]|_)*");


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
            var tkCOMA = ToTerm(",");

            #endregion

            //------------>
            #region Non Terminal
            NonTerminal INICIO = new NonTerminal("INICIO"),
                PARAMETROS = new NonTerminal("PARAMETROS"),
                PARAME = new NonTerminal("PARAME"),
                SENTENCIAS = new NonTerminal("SENTENCIAS"),
                OPERACIONES = new NonTerminal("OPERACIONES"),
                OPERACIONES1 = new NonTerminal("OPERACIONES1"),
                OPERACIONES2 = new NonTerminal("OPERACIONES2"),
                OPERACIONES3 = new NonTerminal("OPERACIONES3"),
                OPERACIONES4 = new NonTerminal("OPERACIONES4"),
                OPERACIONES5 = new NonTerminal("OPERACIONES5"),
                DECLARACION = new NonTerminal("DECLARACION"),
                TIPOVAR = new NonTerminal("TIPOVAR"),
                MASDECLA = new NonTerminal("MASDECLA"),
                LISTAVARIABLES = new NonTerminal("LISTAVARIABLES"),
                ASIGNACION = new NonTerminal("ASIGNACION"),
                POSIBLEASIGNACION = new NonTerminal("POSIBLEASIGNACION"),
                VAR = new NonTerminal("VAR"),
                METODO = new NonTerminal("METODO"),
                MET = new NonTerminal("MET"),
                LISTASENTENCIAS = new NonTerminal("LISTASENTENCIAS");
            #endregion

            //------------>
            #region Gramatica

            INICIO.Rule = tkINT + tkMAIN + tkPARA +PARAMETROS+ tkPARC + tkLLAVA + SENTENCIAS + tkRETURN + "0" + ";" + tkLLAVC
                         ;

            SENTENCIAS.Rule = "hola";

            //declaracion de metodos


            //declaracion de variables

            DECLARACION.Rule = TIPOVAR + Id + MASDECLA + tkPUNTOYCOMA
            ;

            TIPOVAR.Rule = tkINT
                           | tkFLOAT
                           | tkCHAR
                           | tkBOOL
            ;

            MASDECLA.Rule = tkCOMA + LISTAVARIABLES 
                            |tkIGUAL + OPERACIONES + LISTAVARIABLES
                            |Empty
                     
            ;

            LISTAVARIABLES.Rule = LISTAVARIABLES + tkCOMA + VAR
                                  |VAR   
                                  |Empty
            ;

            VAR.Rule = Id + POSIBLEASIGNACION
            ;

            POSIBLEASIGNACION.Rule = tkIGUAL + OPERACIONES
                                     | Empty

            ;
                       
            //operaciones

            OPERACIONES.Rule = OPERACIONES + tkMAS + OPERACIONES1
                              | OPERACIONES1;

            OPERACIONES1.Rule = OPERACIONES1 + tkMENOS + OPERACIONES2
                               | OPERACIONES2;
            ;

            OPERACIONES2.Rule = OPERACIONES2 + tkPOR + OPERACIONES3
                               | OPERACIONES3;
            ;

            OPERACIONES3.Rule = OPERACIONES3 + tkDIV + OPERACIONES4
                                | OPERACIONES4;
            ;

            OPERACIONES4.Rule = OPERACIONES4 + tkPORCENT + OPERACIONES5
                                | tkMENOS + OPERACIONES5
                                | OPERACIONES5;
            ;

            OPERACIONES5.Rule = Entero
                               | Id
                               | tkPARA + OPERACIONES + tkPARC
            ;

            #endregion

            //preferencias
            //------------>
            #region EstadoInicio  
            this.Root = METODO;
            #endregion
        }
    }
}
