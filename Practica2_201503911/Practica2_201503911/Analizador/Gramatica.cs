using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

            RegexBasedTerminal Decimal = new RegexBasedTerminal("Decimal", "[0-9]+[.][0-9]+");
            //id
            IdentifierTerminal Id = new IdentifierTerminal("Id");

            //cadena 
            StringLiteral Cadena = new StringLiteral("Cadena", "\"");

            //comentarios
            CommentTerminal COMENTARIOLINEA = new CommentTerminal("LINE_COMMENT", "//", "\n", "\r\n");
            CommentTerminal COMENTARIOBLOQUE = new CommentTerminal("BLOCK_COMMENT", "/*", "*/");
            #endregion

            #region Terminal
            var tkVOID = ToTerm("void");
            var tkINT = ToTerm("int");
            var tkFLOAT = ToTerm("float");
            var tkBOOL = ToTerm("bool");
            var tkCHAR = ToTerm("char");

            var tkMAIN = ToTerm("main");
            var tkRETURN = ToTerm("return");
            var tkTRUE = ToTerm("true");
            var tkFALSE = ToTerm("false");
            var tkFOR = ToTerm("for");
            var tkIF = ToTerm("if");
            var tkELSE = ToTerm("else");
            var tkDO = ToTerm("do");
            var tkWHILE = ToTerm("while");
            var tkPRINT = ToTerm("print");

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
                LISTASENTENCIAS = new NonTerminal("LISTASENTENCIAS"),
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
                POSIBLEASIGNACION = new NonTerminal("POSIBLEASIGNACION"),
                VAR = new NonTerminal("VAR"),
                METODO = new NonTerminal("METODO"),
                MET = new NonTerminal("MET"),
                
                PARAME = new NonTerminal("PARAME"),
                LLAMADA = new NonTerminal("LLAMADA"),
                LVARIABLES = new NonTerminal("LVARIABLES"),
                VARIA = new NonTerminal("VARIA"),
                ASIGNACION = new NonTerminal("ASIGNACION"),
                IF = new NonTerminal("IF"),
                CONDICIONES = new NonTerminal("CONDICIONES"),
                CONDICIONES1 = new NonTerminal("CONDICIONES1"),
                CONDICIONES2 = new NonTerminal("CONDICIONES2"),
                CONDICIONES3 = new NonTerminal("CONDICIONES3"),
                CONDICIONES4 = new NonTerminal("CONDICIONES4"),
                CONDICIONES5 = new NonTerminal("CONDICIONES5"),
                CONDICIONES6 = new NonTerminal("CONDICIONES6"),
                CONDICIONES7 = new NonTerminal("CONDICIONES7"),
                CONDICIONES8 = new NonTerminal("CONDICIONES8"),
                BOOLEANOS = new NonTerminal("BOOLEANOS"),
                BLOQUEELSE = new NonTerminal("BLOQUEELSE"),
                OPERACIONESESPECIALES = new NonTerminal("OPERACIONESESPECIALES"),
                TIPOOPERACION = new NonTerminal("TIPOOPERACION"),
                WHILE = new NonTerminal("WHILE"),
                DO = new NonTerminal("DO"),
                PRINT = new NonTerminal("PRINT"),
                LEXPRESION = new NonTerminal("LEXPRESION"),
                INSTRUCCIONES = new NonTerminal("INSTRUCCIONES"),
                OPESPECIAL = new NonTerminal("OPESPECIAL");
            #endregion

            //------------>
            #region Gramatica

           
            INICIO.Rule = INICIO + INSTRUCCIONES
                          |INSTRUCCIONES
            ;

            INSTRUCCIONES.Rule =  DECLARACION
                                 | ASIGNACION
                                 | METODO
            ;


            LISTASENTENCIAS.Rule = LISTASENTENCIAS + SENTENCIAS     //a esta produccion caeran todas las sentencias de metodos, if´s, etc.
                                   | SENTENCIAS
            ;

            SENTENCIAS.Rule = LLAMADA + tkPUNTOYCOMA
                              |DECLARACION
                              |ASIGNACION
                              |PRINT
                              |IF
                              |WHILE
                              |DO
                              |Empty;
           
            #region LLamada a un metodo
            //llamada a un metodo  (recordar que despues de la llamada de donde se llamo poner ';' si es necesario solo en operaciones no.)

            LLAMADA.Rule = Id + tkPARA + LVARIABLES + tkPARC 
            ;

            LVARIABLES.Rule = LVARIABLES + tkCOMA + VARIA
                              | VARIA
                              |Empty
            ;

            VARIA.Rule = Id
                        | OPERACIONES
            ;
            #endregion

            #region declaracion de metodos
            //declaracion de metodos

            METODO.Rule = TIPOVAR + Id + tkPARA + PARAMETROS + tkPARC + tkLLAVA + LISTASENTENCIAS + tkRETURN + LEXPRESION +tkPUNTOYCOMA+ tkLLAVC
            ;

            PARAMETROS.Rule = PARAMETROS + tkCOMA + PARAME
                              | PARAME
                              | Empty
            ;

            PARAME.Rule = TIPOVAR + Id;
            #endregion

            #region declaracion y asignacion de variables
            //declaracion y asignacion de variables en la misma linea

            DECLARACION.Rule = TIPOVAR + Id + MASDECLA + tkPUNTOYCOMA
            ;

            TIPOVAR.Rule = tkINT
                           | tkFLOAT
                           | tkCHAR + tkPOR
                           | tkBOOL
            ;

            BOOLEANOS.Rule = tkTRUE
                             | tkFALSE
            ;

            MASDECLA.Rule = tkCOMA + LISTAVARIABLES
                            | tkIGUAL + OPERACIONES + LISTAVARIABLES
                            | Empty

            ;

            LISTAVARIABLES.Rule = LISTAVARIABLES + tkCOMA + VAR
                                  | VAR
                                  | Empty
            ;

            VAR.Rule = Id + POSIBLEASIGNACION
            ;

            POSIBLEASIGNACION.Rule = tkIGUAL + OPERACIONES
                                     | Empty

            ;
            #endregion

            #region Asignacion de variables
            //Asignacion de variables 

            ASIGNACION.Rule = Id + tkIGUAL + OPERACIONES + tkPUNTOYCOMA;

            OPESPECIAL.Rule = tkIGUAL
                              | tkMAS + tkIGUAL
                              | tkMENOS + tkIGUAL
                              | tkMAS + tkMAS
                              | tkMENOS + tkMENOS
            ;

            #endregion

            #region Operaciones Aritmeticas 
            //operaciones Aritmeticas

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
                               | Decimal
                               | Id
                               | tkPARA + OPERACIONES + tkPARC
                               | LLAMADA
                               | Cadena

            ;
            #endregion

            #region Print
            //print
            
            PRINT.Rule = tkPRINT + tkPARA + LEXPRESION + tkPARC + tkPUNTOYCOMA
            ;

            LEXPRESION.Rule = LEXPRESION + tkMAS + OPERACIONES      //tambien lo usare para el retorn y asi tambien pueda devolver concatenaciones 
                            | OPERACIONES
            ;
            #endregion

            #region if-else
            //if-else

            IF.Rule = tkIF + tkPARA + CONDICIONES + tkPARC + tkLLAVA + LISTASENTENCIAS  + tkLLAVC + BLOQUEELSE
            ;

            BLOQUEELSE.Rule = tkELSE + tkLLAVA + LISTASENTENCIAS + tkLLAVC
                              | Empty
            ;
            #endregion

            #region while
            //while

            WHILE.Rule = tkWHILE + tkPARA + CONDICIONES + tkPARC + tkLLAVA + LISTASENTENCIAS + tkLLAVC
            ;
            #endregion

            #region do-while
            //do-while

            DO.Rule = tkDO + tkLLAVA + LISTASENTENCIAS + tkLLAVC + tkWHILE + tkPARA + CONDICIONES + tkPARC + tkPUNTOYCOMA
            ;
            #endregion

            #region Operaciones logicas y relacionales
            //operaciones Logicas y especiales

            CONDICIONES.Rule = CONDICIONES + tkAND + CONDICIONES1
                               | CONDICIONES1
            ;

            CONDICIONES1.Rule = CONDICIONES1 + tkOR + CONDICIONES2
                           | CONDICIONES2
            ;
            CONDICIONES2.Rule = CONDICIONES2 + tkMENOR + CONDICIONES3
                                | CONDICIONES3
            ;

            CONDICIONES3.Rule = CONDICIONES3 + tkMAYOR + CONDICIONES4
                                | CONDICIONES4
            ;

            CONDICIONES4.Rule = CONDICIONES4 + tkMENORIGUAL + CONDICIONES5
                                | CONDICIONES5
            ;

            CONDICIONES5.Rule = CONDICIONES5 + tkMAYORIGUAL + CONDICIONES6
                                | CONDICIONES6
            ;

            CONDICIONES6.Rule = CONDICIONES6 + tkIGUAL + tkIGUAL + CONDICIONES7
                                | CONDICIONES7
            ;

            CONDICIONES7.Rule = CONDICIONES7 + tkDISTINTO + tkIGUAL + CONDICIONES8
                                | CONDICIONES8
            ;

            CONDICIONES8.Rule = OPERACIONES 
                                | BOOLEANOS
                                | tkPARA + CONDICIONES + tkPARC
            ;
            #endregion

            #region Comentarios
            //agregamos los comentarios y si vienen que no haga nada.
            NonGrammarTerminals.Add(COMENTARIOLINEA);
            NonGrammarTerminals.Add(COMENTARIOBLOQUE);
            #endregion

            #endregion

            //preferencias
            //------------>
            #region EstadoInicio  
            this.Root = INICIO;
            #endregion
        }
    }
}
