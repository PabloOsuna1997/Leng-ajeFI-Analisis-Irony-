using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;
using Practica2_201503911.Grafo;

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
                generardot(NodoRaiz);
                return true;
            }
            else
            {
                return false;        
            }
        }

        private static void generardot(ParseTreeNode Raiz) {
            String dot = GeneracionDOT.GeneracionDot(Raiz);

            try { 
                StreamWriter sw = new StreamWriter("C:\\Users\\asddd\\OneDrive\\Escritorio\\DOT.dot");
                sw.WriteLine(dot);
                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }
        }
            
    }
}
