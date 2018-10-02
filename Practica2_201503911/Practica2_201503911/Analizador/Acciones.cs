using Irony.Parsing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practica2_201503911.Analizador
{
    class Acciones
    {
        private static String Id = "";
        private static String tipo = "";

        private static String Id_parametro = "";
        private static String tipo_parametro = "";
        static bool parame = false;

        private static String RESULTADOGENERAL = "";
        private static String Ambito = "";

        static ArrayList TS = new ArrayList();
        static ArrayList MT = new ArrayList();
        static List<String> Parametros_Metodos = new List<string>();

        public static void RealizarAcciones(ParseTreeNode Root)
        {
            //llamada a metodo que generara la acicon
            Accion(Root);

            for (int i = 0; i < TS.Count; i++)
            {
                Variables Simbolo = (Variables)TS[i];
                Console.WriteLine("Tipo: " + Simbolo.getTipo() + " Id: " + Simbolo.getId() + " Valor: " + Simbolo.getDato() + " Ambito: " + Simbolo.getAmbito());

            }

            for (int i = 0; i < MT.Count; i++)
            {
                Metodos Met = (Metodos)MT[i];

                Console.WriteLine("\nTipo: " + Met.getTipo() + " Id: " + Met.getId() + "\nParametros:");
                List<String> Param = Met.getParametros();

                for (int j = 0; j < Param.Count; j++)
                {
                    Console.WriteLine(Param[j]);
                }

            }
        }

        public static void Accion(ParseTreeNode Nodo)
        {
            switch (Nodo.Term.Name.ToString())
            {    //capturamos lo que trae el nodo

                case "INICIO":
                    Ambito = "Global";
                    for (int i = 0; i < Nodo.ChildNodes.Count; i++)
                    {
                        Accion(Nodo.ChildNodes[i]);
                    }

                    break;

                case "INSTRUCCIONES":
                    Accion(Nodo.ChildNodes[0]);
                    break;

                #region Declaracion Variables
                case "DECLARACION":
                    for (int i = 0; i < Nodo.ChildNodes.Count; i++)
                    {
                        Accion(Nodo.ChildNodes[i]);
                    }
                    break;

                case "TIPOVAR":
                    Accion(Nodo.ChildNodes[0]);
                    break;

                case "MASDECLA":

                    if (Nodo.ChildNodes.Count == 2)         //tkCOMA + LISTAVARIABLES
                    {
                        //Console.WriteLine(tipo + " " + Id + ";");
                        TS.Add(new Variables(Id, tipo, "", Ambito));
                        Id = "";
                        Accion(Nodo.ChildNodes[1]);
                    }
                    else if (Nodo.ChildNodes.Count == 3)        //tkIGUAL + OPERACIONES + LISTAVARIABLES
                    {
                        for (int i = 0; i < Nodo.ChildNodes.Count; i++)
                        {
                            if (i == 1)
                            {
                                RESULTADOGENERAL = Operaciones(Nodo.ChildNodes[i]);
                                // Console.WriteLine(tipo + " " + Id + "= " + RESULTADOGENERAL + ";");
                                TS.Add(new Variables(Id, tipo, RESULTADOGENERAL, Ambito));
                                Id = "";
                                RESULTADOGENERAL = "";
                            }

                            else Accion(Nodo.ChildNodes[i]);

                        }
                    }
                    else        //Empty 
                    {
                        //Console.WriteLine(tipo + " " + Id + ";");
                        TS.Add(new Variables(Id, tipo, "", Ambito));
                        Id = "";
                    }
                    break;

                case "LISTAVARIABLES":

                    for (int i = 0; i < Nodo.ChildNodes.Count; i++)
                    {
                        Accion(Nodo.ChildNodes[i]);
                    }
                    break;
                case "VAR":
                    for (int i = 0; i < Nodo.ChildNodes.Count; i++)
                    {
                        Accion(Nodo.ChildNodes[i]);
                    }
                    break;

                case "POSIBLEASIGNACION":

                    if (Nodo.ChildNodes.Count > 0)  // POSIBLEASIGNACION = "=" + OPERACIONES 
                    {
                        RESULTADOGENERAL = Operaciones(Nodo.ChildNodes[1]);
                        // Console.WriteLine(tipo + " " + Id + "= " + RESULTADOGENERAL + ";");
                        TS.Add(new Variables(Id, tipo, RESULTADOGENERAL, Ambito));
                        Id = "";
                        RESULTADOGENERAL = "";
                    }
                    else                            //Empty
                    {
                        // Console.WriteLine(tipo + " " + Id + ";");
                        TS.Add(new Variables(Id, tipo, "", Ambito));
                        Id = "";
                        RESULTADOGENERAL = "";
                    }
                    break;

                #endregion

                #region Declaracion Metodos

                case "METODO":
                    parame = false;
                    
                    for (int i = 0; i < Nodo.ChildNodes.Count; i++)
                    {
                        Accion(Nodo.ChildNodes[i]);
                    }

                    List<String> list2 = new List<String>();        //copio los elementos porque antes los estaba poniendo por referencias y los cambios se veian afectados
                    list2 = Parametros_Metodos.ToList();

                    MT.Add(new Metodos(Id, tipo, list2));

                    Parametros_Metodos.Clear();
                    break;

                case "PARAMETROS":

                    for (int i = 0; i < Nodo.ChildNodes.Count; i++)
                    {
                        Accion(Nodo.ChildNodes[i]);
                    }

                    break;

                case "PARAME":
                    parame = true;
                    for (int i = 0; i < Nodo.ChildNodes.Count; i++)
                    {
                        Accion(Nodo.ChildNodes[i]);
                    }
                    Parametros_Metodos.Add(tipo_parametro + " " + Id_parametro);
                    break;

                #endregion

                #region TERMINALES

                case "int":
                case "bool":
                case "float":
                    if (parame) tipo_parametro = Nodo.Token.Value.ToString();
                    else tipo = Nodo.Token.Value.ToString();
                    break;

                case "char":
                    if (parame) tipo_parametro = "char*";
                    else tipo = "char*";

                    break;

                case "Id":
                    if (parame) Id_parametro = Nodo.Token.Value.ToString();
                    else Id = Nodo.Token.Value.ToString();
                    break;
                #endregion

                default:
                    // Console.WriteLine(Nodo.Term.Name.ToString());
                    break;
            }
        }

        public static String Operaciones(ParseTreeNode Nodo)
        {
            double number1 = 0.0;
            double number2 = 0.0;

            String resultado = "";

            switch (Nodo.Term.Name.ToString())
            {
                case "OPERACIONES":

                    if (Nodo.ChildNodes.Count == 3)  // OPERACIONES = OPERACIONES + OPEREACIONES1
                    {
                        for (int i = 0; i < Nodo.ChildNodes.Count; i++)
                        {
                            if (i == 0)
                            {
                                number1 = Convert.ToDouble(Operaciones(Nodo.ChildNodes[i]));
                            }
                            else if (i == 2)
                            {
                                number2 = Convert.ToDouble(Operaciones(Nodo.ChildNodes[i]));
                            }
                        }
                        resultado = (number1 + number2).ToString();
                        return resultado;
                    }
                    else
                    {
                        resultado = Operaciones(Nodo.ChildNodes[0]);

                    }
                    break;
                case "OPERACIONES1":

                    if (Nodo.ChildNodes.Count == 3)  // OPERACIONES = OPERACIONES + OPEREACIONES1
                    {
                        for (int i = 0; i < Nodo.ChildNodes.Count; i++)
                        {
                            if (i == 0)
                            {
                                number1 = Convert.ToDouble(Operaciones(Nodo.ChildNodes[i]));
                            }
                            else if (i == 2)
                            {
                                number2 = Convert.ToDouble(Operaciones(Nodo.ChildNodes[i]));
                            }
                        }
                        resultado = (number1 - number2).ToString();
                        return resultado;
                    }
                    else
                    {
                        resultado = Operaciones(Nodo.ChildNodes[0]);

                    }



                    break;
                case "OPERACIONES2":

                    if (Nodo.ChildNodes.Count == 3)  // OPERACIONES = OPERACIONES + OPEREACIONES1
                    {
                        for (int i = 0; i < Nodo.ChildNodes.Count; i++)
                        {
                            if (i == 0)
                            {
                                number1 = Convert.ToDouble(Operaciones(Nodo.ChildNodes[i]));
                            }
                            else if (i == 2)
                            {
                                number2 = Convert.ToDouble(Operaciones(Nodo.ChildNodes[i]));
                            }
                        }
                        resultado = (number1 * number2).ToString();
                        return resultado;
                    }
                    else
                    {
                        resultado = Operaciones(Nodo.ChildNodes[0]);

                    }

                    break;
                case "OPERACIONES3":

                    if (Nodo.ChildNodes.Count == 3)  // OPERACIONES = OPERACIONES + OPEREACIONES1
                    {
                        for (int i = 0; i < Nodo.ChildNodes.Count; i++)
                        {
                            if (i == 0)
                            {
                                number1 = Convert.ToDouble(Operaciones(Nodo.ChildNodes[i]));
                            }
                            else if (i == 2)
                            {
                                number2 = Convert.ToDouble(Operaciones(Nodo.ChildNodes[i]));
                            }
                        }
                        resultado = number1 / number2 + "";
                        return resultado;
                    }
                    else
                    {
                        resultado = Operaciones(Nodo.ChildNodes[0]);

                    }

                    break;
                case "OPERACIONES4":
                    if (Nodo.ChildNodes.Count == 2)
                    {
                        resultado = "-" + Operaciones(Nodo.ChildNodes[1]);
                        return resultado;
                    }
                    else if (Nodo.ChildNodes.Count == 1)
                    {
                        resultado = Operaciones(Nodo.ChildNodes[0]);
                    }


                    break;
                case "OPERACIONES5":
                    if (Nodo.ChildNodes.Count == 3)     //vienen parentesis entonces por precedencia se hara primero
                    {
                        resultado = Operaciones(Nodo.ChildNodes[1]);
                    }
                    else
                    {
                        resultado = Operaciones(Nodo.ChildNodes[0]);
                    }
                    break;

                case "Entero":
                case "Decimal":
                case "Id":
                case "Cadena":

                    resultado = Nodo.Token.Value.ToString();

                    break;

            }

            return resultado;
        }
    }
}