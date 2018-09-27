using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;

namespace Practica2_201503911.Analizador
{
    class Analizar
    {
        public static bool analizador(String cadena){

            Gramatica grama = new Gramatica();                      
            LanguageData lenguaje = new LanguageData(grama);       
            Parser parser = new Parser(lenguaje);
            ParseTree arbol = parser.Parse(cadena);
            ParseTreeNode NodoRaiz = arbol.Root;

            if (NodoRaiz != null)
            {
                return true;
            }
            else
            {
                return false;        
            }
        }
    }
}
