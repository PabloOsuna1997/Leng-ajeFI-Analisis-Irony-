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

        public static List<String> erroressintacticos = new List<String>();
        public static List<String> erroreslexicos = new List<String>();
        public static ParseTreeNode analizador(String cadena)
        {

            Gramatica grama = new Gramatica();
            LanguageData lenguaje = new LanguageData(grama);
            Parser parser = new Parser(lenguaje);
            ParseTree arbol = parser.Parse(cadena);
            ParseTreeNode NodoRaiz = arbol.Root;

            if (NodoRaiz != null)
            {
                generardot(NodoRaiz);
                return NodoRaiz;
            }
            else
            {
                for (int i = 0; i < arbol.ParserMessages.Count; i++)
                {
                    String[] tipoerror = arbol.ParserMessages.ElementAt(i).Message.Split(' ');
                    if (tipoerror[0].Equals("Syntax"))
                    {
                        String Error = "Error Sintactico: " + arbol.ParserMessages.ElementAt(i).Message + " Linea: " +(arbol.ParserMessages.ElementAt(i).Location.Line + 1) + " Columna: " + arbol.ParserMessages.ElementAt(i).Location.Column;
                        erroressintacticos.Add(Error);
                    }
                    else
                    {
                        String Error = "Error Lexico: " + arbol.ParserMessages.ElementAt(i).Message + " Linea: " + (arbol.ParserMessages.ElementAt(i).Location.Line + 1) + " Columna: " + arbol.ParserMessages.ElementAt(i).Location.Column;
                        erroressintacticos.Add(Error);
                    }

                }
                return null;
            }
        }

        private static void generardot(ParseTreeNode Raiz)
        {
            String dot = GeneracionDOT.GeneracionDot(Raiz);

            try
            {
                StreamWriter sw = new StreamWriter("C:\\Users\\asddd\\OneDrive\\Escritorio\\DOT.dot");
                sw.WriteLine(dot);
                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("No se genero dot: " + e);
            }

        }

    }
}
