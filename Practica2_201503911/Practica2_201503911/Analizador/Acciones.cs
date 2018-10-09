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

        public static List<String> ErroresSemanticos = new List<string>();
        public static List<String> Impresiones = new List<string>();

        private static String Print = "";
        static String acceso = "";
        static String acceso1 = "";
        static string idmetodo = "";        //variable que me servira en la llamada de metodos.
        static int contadorparametros = 0;
        static String nombremetodomaximo = "";
        static String tipometodomaximo = "";

        private static bool soymain = false;

        private static int contadormetodos = 1;
        private static String Id = "";
        private static String tipo = "";
        private static String Id1 = "";
        private static String tipo1 = "";

        private static String Id_parametro = "";
        private static String tipo_parametro = "";
        static bool parame = false;

        private static String RESULTADOGENERAL = "";
        private static String Ambito = "";
        private static String ambitollamada = "";

        public static ArrayList TS = new ArrayList();
        public static ArrayList MT = new ArrayList();
        public static List<String> Parametros_Metodos = new List<string>();

        public static void RealizarAcciones(ParseTreeNode Root)
        {
            //llamada a metodo que generara la acicon
            Print = "";
            acceso = "";
            acceso1 = "";
            contadormetodos = 1;
            Id = "";
            tipo = "";
            Id_parametro = "";
            tipo_parametro = "";
            parame = false;
            RESULTADOGENERAL = "";
            Ambito = "";
            ambitollamada = "";
            TS = new ArrayList();
            MT = new ArrayList();
            Parametros_Metodos.Clear();
            idmetodo = "";        //variable que me servira en la llamada de metodos.
            contadorparametros = 0;
            nombremetodomaximo = "";
            tipometodomaximo = "";
            soymain = false;
            Id1 = "";
            tipo1 = "";
            ErroresSemanticos.Clear();


            Accion(Root);

            Console.WriteLine("\nVariables:\n");
            for (int i = 0; i < TS.Count; i++)
            {
                Variables Simbolo = (Variables)TS[i];
                Console.WriteLine("Tipo: " + Simbolo.getTipo() + " Id: " + Simbolo.getId() + " Valor: " + Simbolo.getDato() + " Ambito: " + Simbolo.getAmbito());

            }/*

            for (int i = 0; i < MT.Count; i++)
            {
                Metodos Met = (Metodos)MT[i];

                Console.WriteLine("\nTipo: " + Met.getTipo() + " Id: " + Met.getId() + "\nParametros:");
                List<String> Param = Met.getParametros();

                for (int j = 0; j < Param.Count; j++)
                {
                    Console.WriteLine(Param[j]);
                }
            }*/
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
                                try { if (tipo.Equals("int")) { int num = Convert.ToInt32(RESULTADOGENERAL); } } catch (Exception) { ErroresSemanticos.Add("Error de tipos"); }
                                try { if (tipo.Equals("bool")) { bool num = Convert.ToBoolean(RESULTADOGENERAL); } } catch (Exception) { ErroresSemanticos.Add("Error de tipos"); }
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

                #region AsignacionVariables
                case "ASIGNACION":
                    for (int i = 0; i < Nodo.ChildNodes.Count; i++)
                    {
                        if (i == 2) RESULTADOGENERAL = Operaciones(Nodo.ChildNodes[i]);
                        else Accion(Nodo.ChildNodes[i]);
                    }

                    //buscamos el id en mi tabla de simbolos
                    for (int i = 0; i < TS.Count; i++)
                    {
                        Variables Var = (Variables)TS[i];

                        if (Var.getId().Equals(Id) && (Var.getAmbito().Equals("Global") || Var.getAmbito().Equals(Ambito)))
                        {
                            Var.setDato(RESULTADOGENERAL);
                        }
                    }
                    break;
                #endregion


                #region comentado
                case "METODO":

                    Ambito = "Metodo " + Nodo.ChildNodes[1].Token.Value.ToString();
                    contadormetodos++;
                    ParseTreeNode Retorno = null;
                    ParseTreeNode Cuerpo = Nodo;


                    parame = false;
                    for (int i = 0; i < Nodo.ChildNodes.Count; i++)
                    {

                        if (i == 8) { Retorno = Nodo.ChildNodes[i]; }
                        if (i == 6 && !Nodo.ChildNodes[1].Token.Value.ToString().Equals("main")) { }
                        else
                        {
                            if (i == 1) { tipometodomaximo = tipo; }
                            else if (i == 2) { nombremetodomaximo = Id; }
                            Accion(Nodo.ChildNodes[i]);
                        }
                    }

                    List<String> list2 = new List<String>();        //copio los elementos porque antes los estaba poniendo por referencias y los cambios se veian afectados
                    list2 = Parametros_Metodos.ToList();

                    MT.Add(new Metodos(nombremetodomaximo, tipometodomaximo, list2, Retorno, Cuerpo));        //la lista de parametros tambien deben ser variables declaradas 
                    Parametros_Metodos.Clear();

                    Ambito = "Global";
                    Id = "";
                    tipo = "";
                    parame = false;


                    break;

                case "PARAMETROS":

                    for (int i = 0; i < Nodo.ChildNodes.Count; i++)
                    {
                        Accion(Nodo.ChildNodes[i]);
                    }
                    parame = false;
                    break;

                case "PARAME":
                    parame = true;
                    for (int i = 0; i < Nodo.ChildNodes.Count; i++)
                    {
                        Accion(Nodo.ChildNodes[i]);
                    }
                    Parametros_Metodos.Add(tipo_parametro + " " + Id_parametro);
                    TS.Add(new Variables(Id_parametro, tipo_parametro, "", Ambito));           //los paramteros tambien los agregamos a la tabla de simbolos
                    break;

                case "LISTASENTENCIAS":
                    for (int i = 0; i < Nodo.ChildNodes.Count; i++)
                    {
                        Accion(Nodo.ChildNodes[i]);
                    }
                    break;

                case "SENTENCIAS":
                    /*SENTENCIAS.Rule = LLAMADA + tkPUNTOYCOMA
                              | DECLARACION
                              | ASIGNACION
                              | PRINT
                              | IF
                              | WHILE
                              | DO
                              | Empty;*/
                    for (int i = 0; i < Nodo.ChildNodes.Count; i++)
                    {
                        Accion(Nodo.ChildNodes[i]);
                    }
                    break;


                case "PRINT":
                    //PRINT.Rule = tkPRINT + tkPARA + LEXPRESION + tkPARC + tkPUNTOYCOMA
                    Print = "";
                    Accion(Nodo.ChildNodes[2]);
                    Console.WriteLine(Print);
                    Impresiones.Add(Print);

                    break;


                case "LEXPRESION":

                    if (Nodo.ChildNodes.Count == 3)         //LEXPRESION + tkMAS + OPERACIONES   <------- tengo que cambiar ese mas porque si no me lo toma como que fuera parte de la operacion
                    {
                        Accion(Nodo.ChildNodes[0]);
                        String texto = Operaciones(Nodo.ChildNodes[2]);
                        Print += texto;
                    }
                    else if (Nodo.ChildNodes.Count == 1)
                    {
                        String texto = Operaciones(Nodo.ChildNodes[0]);
                        Print += texto;
                    }

                    break;

                case "IF":
                    acceso = "";
                    bool bandera = false;
                    // IF.Rule = tkIF + tkPARA + CONDICIONES + tkPARC + tkLLAVA + LISTASENTENCIAS  + tkLLAVC + BLOQUEELSE
                    for (int i = 0; i < Nodo.ChildNodes.Count; i++)
                    {
                        if (i == 2)
                        {
                            acceso = Condiciones(Nodo.ChildNodes[i]);
                            if (!acceso.Equals("")) bandera = Convert.ToBoolean(acceso);
                            if (bandera)
                            {
                                //Console.WriteLine(acceso); //acceso permitido lee las sentencias de if
                            }
                            else
                            {
                                i = 6; //acceso denegado   salta a las sentencias de else7
                            }
                        }
                        else
                        {
                            if (bandera && i == 7)
                            {
                                break;
                            }
                            else
                            {
                                Accion(Nodo.ChildNodes[i]);
                            }
                        }
                    }
                    break;
                case "BLOQUEELSE":
                    if (Nodo.ChildNodes.Count > 0)   // BLOQUEELSE.Rule = tkELSE + tkLLAVA + LISTASENTENCIAS + tkLLAVC
                    {
                        Accion(Nodo.ChildNodes[2]);
                    }
                    break;

                case "WHILE":
                    acceso = "";
                    bool bandera1 = false;
                    //WHILE.Rule = tkWHILE + tkPARA + CONDICIONES + tkPARC + tkLLAVA + LISTASENTENCIAS + tkLLAVC

                    for (int i = 0; i < Nodo.ChildNodes.Count; i++)
                    {
                        if (i == 2)
                        {
                            acceso = Condiciones(Nodo.ChildNodes[i]);
                            if (!acceso.Equals("")) bandera1 = Convert.ToBoolean(acceso);
                            if (!bandera1) i = 6; //acceso denegado  se salta al ultimo token y se acaba el for y no entrara a las sentencias del while.
                        }
                        else
                        {
                            if (i == 5 && bandera1)
                            {
                                // se va a aquedar enciclado hasta que la condicion se cumpla.
                                while (bandera1 == true)
                                {
                                    Accion(Nodo.ChildNodes[i]);
                                    i = 0;
                                    break;
                                }
                            }
                            else
                            {
                                Accion(Nodo.ChildNodes[i]);
                            }
                        }
                    }
                    break;

                case "DO":
                    acceso = "";
                    bool bandera2 = false;
                    //  DO.Rule = tkDO + tkLLAVA + LISTASENTENCIAS + tkLLAVC + tkWHILE + tkPARA + CONDICIONES + tkPARC + tkPUNTOYCOMA
                    for (int i = 0; i < Nodo.ChildNodes.Count; i++)
                    {
                        if (i == 6)
                        {
                            acceso = Condiciones(Nodo.ChildNodes[i]);
                            if (!acceso.Equals("")) bandera2 = Convert.ToBoolean(acceso);
                            if (bandera2 == true)
                            {
                                i = 0;
                            }
                        }
                        else
                        {
                            Accion(Nodo.ChildNodes[i]);
                        }
                    }
                    break;

                case "LLAMADA":
                    bool existemetodo = false;
                    Accion(Nodo.ChildNodes[0]);     //solo capturo el id y ya 
                    for (int i = 0; i < MT.Count; i++)
                    {
                        Metodos met = (Metodos)MT[i];
                        if (met.getId().Equals(Id))
                        {
                            LLamadaMetodo(Nodo);
                            AccionesMetodo(met.getCuerpo());
                            existemetodo = true;
                            break;
                        }
                    }
                    if (!existemetodo)
                    {
                        Console.WriteLine("Error Semantico metodo " + Id + " no ha sido declarado.");
                    }
                    Id = "";

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

        public static void LLamadaMetodo(ParseTreeNode Nodo)
        {
            switch (Nodo.Term.Name.ToString())
            {
                #region LLamadaMetodos
                case "LLAMADA":
                    idmetodo = "";
                    contadorparametros = 0;

                    for (int i = 0; i < Nodo.ChildNodes.Count; i++)
                    {
                        if (i == 0)
                        {
                            idmetodo = Nodo.ChildNodes[0].Token.Value.ToString();   //capturo su id 
                        }
                        else
                        {
                            LLamadaMetodo(Nodo.ChildNodes[i]);
                        }
                    }
                    break;
                case "LVARIABLES":

                    if (Nodo.ChildNodes.Count == 3)
                    {
                        for (int i = 0; i < Nodo.ChildNodes.Count; i++)
                        {
                            LLamadaMetodo(Nodo.ChildNodes[i]);
                        }
                    }
                    else if (Nodo.ChildNodes.Count == 1)
                    {
                        LLamadaMetodo(Nodo.ChildNodes[0]);
                    }
                    break;

                case "VARIA":
                    String Operacion = Operaciones(Nodo.ChildNodes[0]);

                    for (int i = 0; i < MT.Count; i++)
                    {
                        Metodos met = (Metodos)MT[i];
                        if (met.getId().Equals(idmetodo))
                        {
                            List<String> parametros = met.getParametros();

                            for (int j = 0; j < parametros.Count; j++)
                            {
                                String parametro = parametros[j];
                                String[] para = parametro.Split(' ');
                                String pa = para[1].Trim();

                                if (j == contadorparametros)
                                {

                                    for (int z = 0; z < TS.Count; z++)
                                    {
                                        Variables var = (Variables)TS[z];
                                        if (var.getId().Equals(pa))
                                        {
                                            var.setDato(Operacion);
                                        }
                                    }
                                }

                            }
                        }
                    }
                    contadorparametros++;
                    break;
            }
            #endregion
        }

        public static String Operaciones(ParseTreeNode Nodo)
        {
            String resultado = "";
            try
            {
                double number1 = 0.0;
                double number2 = 0.0;

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
                            if (Nodo.ChildNodes[0].Term.Name.ToString().Equals("LLAMADA"))
                            {
                                //Operaciones(Nodo.ChildNodes[0]);
                                LLamadaMetodo(Nodo.ChildNodes[0]);
                                for (int i = 0; i < MT.Count; i++)
                                {
                                    Metodos met = (Metodos)MT[i];

                                    if (met.getId().Equals(idmetodo))
                                    {
                                        //mandaremos acciones
                                        // Console.WriteLine("\n\n");
                                        AccionesMetodo(met.getCuerpo());
                                        resultado = Operaciones(met.getRetorno());
                                    }
                                }
                            }
                            else
                            {

                                resultado = Operaciones(Nodo.ChildNodes[0]);
                            }
                        }
                        break;

                    case "BOOLEANOS":
                        resultado = Operaciones(Nodo.ChildNodes[0]);
                        break;



                    case "Entero":
                    case "Decimal":
                    case "Cadena":
                    case "true":
                    case "false":
                        resultado = Nodo.Token.Value.ToString();
                        break;

                    case "Id":      //verificacion que el id exista y tenga valor

                        if (TS.Count > 0)
                        {
                            for (int i = 0; i < TS.Count; i++)
                            {
                                Variables VAR = (Variables)TS[i];
                                if (Nodo.Token.Value.ToString().Equals(VAR.getId()))
                                {
                                    resultado = VAR.getDato();
                                    break;
                                }
                                else
                                {
                                    resultado = "Error Semantico.";
                                }
                            }
                        }
                        else
                        {
                            resultado = "Error Semantico, variable especificada no existe.";
                            ErroresSemanticos.Add(resultado);
                        }
                        break;
                }
                return resultado;
            }
            catch (Exception e)
            {
                resultado = "Error Semantico de tipo de datos.";
                return resultado;
            }
        }

        public static String Condiciones(ParseTreeNode Nodo)
        {
            String resultado = "";
            String Condicion1 = "";
            String Condicion2 = "";

            switch (Nodo.Term.Name.ToString())
            {
                case "CONDICIONES":
                    if (Nodo.ChildNodes.Count == 3)
                    {
                        for (int i = 0; i < Nodo.ChildNodes.Count; i++)
                        {
                            if (i == 0)
                            {
                                Condicion1 = Condiciones(Nodo.ChildNodes[i]);
                            }
                            else
                            {
                                Condicion2 = Condiciones(Nodo.ChildNodes[i]);
                            }
                        }

                        if (Condicion1.Equals("true") && Condicion2.Equals("true")) resultado = "true";
                        else resultado = "false";

                        return resultado;
                    }
                    else
                    {
                        resultado = Condiciones(Nodo.ChildNodes[0]);
                    }

                    break;
                case "CONDICIONES1":
                    if (Nodo.ChildNodes.Count == 3)
                    {
                        for (int i = 0; i < Nodo.ChildNodes.Count; i++)
                        {
                            if (i == 0)
                            {
                                Condicion1 = Condiciones(Nodo.ChildNodes[i]);
                            }
                            else
                            {
                                Condicion2 = Condiciones(Nodo.ChildNodes[i]);
                            }
                        }

                        if (Condicion1.Equals("true") || Condicion2.Equals("true")) resultado = "true";
                        else resultado = "false";

                        return resultado;
                    }
                    else
                    {
                        resultado = Condiciones(Nodo.ChildNodes[0]);
                    }

                    break;
                case "CONDICIONES2":
                    if (Nodo.ChildNodes.Count == 3)
                    {
                        for (int i = 0; i < Nodo.ChildNodes.Count; i++)
                        {
                            if (i == 0)
                            {
                                Condicion1 = Condiciones(Nodo.ChildNodes[i]);
                            }
                            else
                            {
                                Condicion2 = Condiciones(Nodo.ChildNodes[i]);
                            }
                        }

                        if (Convert.ToDouble(Condicion1) < Convert.ToDouble(Condicion2)) resultado = "true";
                        else resultado = "false";

                        return resultado;
                    }
                    else
                    {
                        resultado = Condiciones(Nodo.ChildNodes[0]);
                    }

                    break;
                case "CONDICIONES3":
                    if (Nodo.ChildNodes.Count == 3)
                    {
                        for (int i = 0; i < Nodo.ChildNodes.Count; i++)
                        {
                            if (i == 0)
                            {
                                Condicion1 = Condiciones(Nodo.ChildNodes[i]);
                            }
                            else
                            {
                                Condicion2 = Condiciones(Nodo.ChildNodes[i]);
                            }
                        }

                        if (Convert.ToDouble(Condicion1) > Convert.ToDouble(Condicion2)) resultado = "true";
                        else resultado = "false";

                        return resultado;
                    }
                    else
                    {
                        resultado = Condiciones(Nodo.ChildNodes[0]);
                    }

                    break;
                case "CONDICIONES4":
                    if (Nodo.ChildNodes.Count == 3)
                    {
                        for (int i = 0; i < Nodo.ChildNodes.Count; i++)
                        {
                            if (i == 0)
                            {
                                Condicion1 = Condiciones(Nodo.ChildNodes[i]);
                            }
                            else
                            {
                                Condicion2 = Condiciones(Nodo.ChildNodes[i]);
                            }
                        }

                        if (Convert.ToDouble(Condicion1) <= Convert.ToDouble(Condicion2)) resultado = "true";
                        else resultado = "false";

                        return resultado;
                    }
                    else
                    {
                        resultado = Condiciones(Nodo.ChildNodes[0]);
                    }

                    break;

                case "CONDICIONES5":
                    if (Nodo.ChildNodes.Count == 3)
                    {
                        for (int i = 0; i < Nodo.ChildNodes.Count; i++)
                        {
                            if (i == 0)
                            {
                                Condicion1 = Condiciones(Nodo.ChildNodes[i]);
                            }
                            else
                            {
                                Condicion2 = Condiciones(Nodo.ChildNodes[i]);
                            }
                        }

                        if (Convert.ToDouble(Condicion1) >= Convert.ToDouble(Condicion2)) resultado = "true";
                        else resultado = "false";

                        return resultado;
                    }
                    else
                    {
                        resultado = Condiciones(Nodo.ChildNodes[0]);
                    }

                    break;

                case "CONDICIONES6":
                    if (Nodo.ChildNodes.Count == 4)
                    {
                        for (int i = 0; i < Nodo.ChildNodes.Count; i++)
                        {
                            if (i == 0)
                            {
                                Condicion1 = Condiciones(Nodo.ChildNodes[i]);
                            }
                            else
                            {
                                Condicion2 = Condiciones(Nodo.ChildNodes[i]);
                            }
                        }

                        if (Condicion1.Equals(Condicion2)) resultado = "true";
                        else resultado = "false";

                        return resultado;
                    }
                    else
                    {
                        resultado = Condiciones(Nodo.ChildNodes[0]);
                    }

                    break;
                case "CONDICIONES7":
                    if (Nodo.ChildNodes.Count == 4)
                    {
                        for (int i = 0; i < Nodo.ChildNodes.Count; i++)
                        {
                            if (i == 0)
                            {
                                Condicion1 = Condiciones(Nodo.ChildNodes[i]);
                            }
                            else
                            {
                                Condicion2 = Condiciones(Nodo.ChildNodes[i]);
                            }
                        }

                        if (!Condicion1.Equals(Condicion2)) resultado = "true";
                        else resultado = "false";

                        return resultado;
                    }
                    else
                    {
                        resultado = Condiciones(Nodo.ChildNodes[0]);
                    }

                    break;

                case "CONDICIONES8":
                    if (Nodo.ChildNodes.Count == 3)     //vienen parentesis entonces por precedencia se hara primero
                    {
                        resultado = Condiciones(Nodo.ChildNodes[1]);
                    }
                    else
                    {
                        resultado = Condiciones(Nodo.ChildNodes[0]);
                    }
                    break;


                case "OPERACIONES":
                    resultado = Operaciones(Nodo);
                    break;

                case "BOOLEANOS":
                    resultado = Operaciones(Nodo.ChildNodes[0]);
                    break;

                case "Entero":
                case "Decimal":
                case "Cadena":
                case "true":
                case "false":

                    resultado = Nodo.Token.Value.ToString();

                    break;

                case "Id":      //verificacion que el id exista y tenga valor

                    if (TS.Count > 0)
                    {
                        for (int i = 0; i < TS.Count; i++)
                        {
                            Variables VAR = (Variables)TS[i];
                            if (Nodo.Token.Value.ToString().Equals(VAR.getId()))
                            {
                                resultado = VAR.getDato();
                                break;
                            }
                            else
                            {
                                resultado = "Error Semantico.";
                            }
                        }
                    }
                    else
                    {
                        resultado = "Error Semantico.";
                    }
                    break;
                default:
                    break;
            }

            return resultado;
        }

        public static void AccionesMetodo(ParseTreeNode Nodo)
        {
            switch (Nodo.Term.Name.ToString())
            {
                case "METODO":
                    ambitollamada = "Metodo " + Nodo.ChildNodes[1].Token.Value.ToString();
                    AccionesMetodo(Nodo.ChildNodes[6]);
                    break;

                case "LISTASENTENCIAS":
                    for (int i = 0; i < Nodo.ChildNodes.Count; i++)
                    {
                        AccionesMetodo(Nodo.ChildNodes[i]);
                    }
                    break;

                case "SENTENCIAS":
                    /*SENTENCIAS.Rule = LLAMADA + tkPUNTOYCOMA
                              | DECLARACION
                              | ASIGNACION
                              | PRINT
                              | IF
                              | WHILE
                              | DO
                              | Empty;*/
                    for (int i = 0; i < Nodo.ChildNodes.Count; i++)
                    {
                        AccionesMetodo(Nodo.ChildNodes[i]);
                    }
                    break;

                case "PRINT":
                    //PRINT.Rule = tkPRINT + tkPARA + LEXPRESION + tkPARC + tkPUNTOYCOMA
                    Print = "";
                    AccionesMetodo(Nodo.ChildNodes[2]);
                    Console.WriteLine(Print);
                    Impresiones.Add(Print);

                    break;

                case "LEXPRESION":

                    if (Nodo.ChildNodes.Count == 3)         //LEXPRESION + tkMAS + OPERACIONES   <_------- tengo que cambiar ese mas porque si no me lo toma como que fuera parte de la operacion
                    {
                        AccionesMetodo(Nodo.ChildNodes[0]);
                        String texto = Operaciones(Nodo.ChildNodes[2]);
                        Print += texto;
                    }
                    else if (Nodo.ChildNodes.Count == 1)
                    {
                        String texto = Operaciones(Nodo.ChildNodes[0]);
                        Print += texto;
                    }

                    break;

                case "IF":
                    acceso1 = "";
                    bool bandera = false;
                    // IF.Rule = tkIF + tkPARA + CONDICIONES + tkPARC + tkLLAVA + LISTASENTENCIAS  + tkLLAVC + BLOQUEELSE
                    for (int i = 0; i < Nodo.ChildNodes.Count; i++)
                    {
                        if (i == 2)
                        {
                            acceso1 = Condiciones(Nodo.ChildNodes[i]);
                            if (!acceso1.Equals("")) bandera = Convert.ToBoolean(acceso1);

                            if (bandera)
                            {
                                Console.WriteLine(acceso1); //acceso permitido lee las sentencias de if
                            }
                            else
                            {
                                i = 6; //acceso denegado   salta a las sentencias de else7
                            }
                        }
                        else
                        {
                            if (bandera && i == 7)
                            {
                                break;
                            }
                            else
                            {
                                AccionesMetodo(Nodo.ChildNodes[i]);
                            }
                        }
                    }
                    break;
                case "BLOQUEELSE":
                    if (Nodo.ChildNodes.Count > 0)   // BLOQUEELSE.Rule = tkELSE + tkLLAVA + LISTASENTENCIAS + tkLLAVC
                    {
                        AccionesMetodo(Nodo.ChildNodes[2]);
                    }
                    break;

                case "WHILE":
                    acceso1 = "";
                    bool bandera1 = false;
                    //WHILE.Rule = tkWHILE + tkPARA + CONDICIONES + tkPARC + tkLLAVA + LISTASENTENCIAS + tkLLAVC

                    for (int i = 0; i < Nodo.ChildNodes.Count; i++)
                    {
                        if (i == 2)
                        {
                            acceso1 = Condiciones(Nodo.ChildNodes[i]);
                            if (!acceso1.Equals("")) bandera1 = Convert.ToBoolean(acceso1);
                            if (bandera1 == false) i = 6; //acceso denegado  se salta al ultimo token y se acaba el for y no entrara a las sentencias del while.
                        }
                        else
                        {
                            if (i == 5 && bandera1)
                            {
                                // se va a aquedar enciclado hasta que la condicion se cumpla.
                                while (bandera1 == true)
                                {
                                    AccionesMetodo(Nodo.ChildNodes[i]);
                                    i = 0;
                                    break;
                                }
                            }
                            else
                            {
                                AccionesMetodo(Nodo.ChildNodes[i]);
                            }
                        }
                    }
                    break;

                case "DO":
                    acceso1 = "";
                    bool bandera2 = false;
                    //  DO.Rule = tkDO + tkLLAVA + LISTASENTENCIAS + tkLLAVC + tkWHILE + tkPARA + CONDICIONES + tkPARC + tkPUNTOYCOMA
                    for (int i = 0; i < Nodo.ChildNodes.Count; i++)
                    {
                        if (i == 6)
                        {
                            acceso1 = Condiciones(Nodo.ChildNodes[i]);
                            if (!acceso1.Equals("")) bandera2 = Convert.ToBoolean(acceso1);
                            if (bandera2)
                            {
                                i = 0;
                            }
                        }
                        else
                        {
                            AccionesMetodo(Nodo.ChildNodes[i]);
                        }
                    }
                    break;


                case "ASIGNACION":
                    for (int i = 0; i < Nodo.ChildNodes.Count; i++)
                    {
                        if (i == 2) RESULTADOGENERAL = Operaciones(Nodo.ChildNodes[i]);
                        else AccionesMetodo(Nodo.ChildNodes[i]);
                    }
                    //buscamos el id en mi tabla de simbolos
                    for (int i = 0; i < TS.Count; i++)
                    {
                        Variables Var = (Variables)TS[i];

                        if (Var.getId().Equals(Id1))
                        {
                            Var.setDato(RESULTADOGENERAL);
                        }
                    }
                    break;

                case "LLAMADA":
                    bool existemetodo = false;
                    AccionesMetodo(Nodo.ChildNodes[0]);     //solo capturo el id y ya 
                    for (int i = 0; i < MT.Count; i++)
                    {
                        Metodos met = (Metodos)MT[i];
                        if (met.getId().Equals(Id1))
                        {
                            LLamadaMetodo(Nodo);
                            AccionesMetodo(met.getCuerpo());
                            existemetodo = true;
                            break;
                        }
                    }
                    if (!existemetodo)
                    {
                        Console.WriteLine("Error Semantico metodo " + Id1 + " no ha sido declarado.");
                    }
                    Id1 = "";

                    break;


                #region Declaracion Variables
                case "DECLARACION":

                    for (int i = 0; i < Nodo.ChildNodes.Count; i++)
                    {
                        AccionesMetodo(Nodo.ChildNodes[i]);
                    }
                    break;

                case "TIPOVAR":
                    AccionesMetodo(Nodo.ChildNodes[0]);
                    break;

                case "MASDECLA":

                    if (Nodo.ChildNodes.Count == 2)         //tkCOMA + LISTAVARIABLES
                    {
                        //Console.WriteLine(tipo + " " + Id + ";");

                        TS.Add(new Variables(Id1, tipo1, "", ambitollamada));
                        Id1 = "";
                        AccionesMetodo(Nodo.ChildNodes[1]);
                    }
                    else if (Nodo.ChildNodes.Count == 3)        //tkIGUAL + OPERACIONES + LISTAVARIABLES
                    {
                        for (int i = 0; i < Nodo.ChildNodes.Count; i++)
                        {
                            if (i == 1)
                            {
                                RESULTADOGENERAL = Operaciones(Nodo.ChildNodes[i]);
                                // Console.WriteLine(tipo + " " + Id + "= " + RESULTADOGENERAL + ";");

                                TS.Add(new Variables(Id1, tipo1, RESULTADOGENERAL, ambitollamada));
                                Id = "";
                                RESULTADOGENERAL = "";
                            }

                            else Accion(Nodo.ChildNodes[i]);

                        }
                    }
                    else        //Empty 
                    {
                        //Console.WriteLine(tipo + " " + Id + ";");
                        TS.Add(new Variables(Id1, tipo1, "", ambitollamada));
                        Id1 = "";
                    }
                    break;

                case "LISTAVARIABLES":

                    for (int i = 0; i < Nodo.ChildNodes.Count; i++)
                    {
                        AccionesMetodo(Nodo.ChildNodes[i]);
                    }
                    break;
                case "VAR":
                    for (int i = 0; i < Nodo.ChildNodes.Count; i++)
                    {
                        AccionesMetodo(Nodo.ChildNodes[i]);
                    }
                    break;

                case "POSIBLEASIGNACION":

                    if (Nodo.ChildNodes.Count > 0)  // POSIBLEASIGNACION = "=" + OPERACIONES 
                    {
                        RESULTADOGENERAL = Operaciones(Nodo.ChildNodes[1]);
                        // Console.WriteLine(tipo + " " + Id + "= " + RESULTADOGENERAL + ";");
                        TS.Add(new Variables(Id1, tipo1, RESULTADOGENERAL, ambitollamada));
                        Id1 = "";
                        RESULTADOGENERAL = "";
                    }
                    else                            //Empty
                    {
                        // Console.WriteLine(tipo + " " + Id + ";");
                        TS.Add(new Variables(Id1, tipo1, "", ambitollamada));
                        Id1 = "";
                        RESULTADOGENERAL = "";
                    }
                    break;

                #endregion             

                #region TERMINALES



                case "int":
                case "bool":
                case "float":
                    tipo1 = Nodo.Token.Value.ToString();
                    break;

                case "char":
                    tipo1 = "char*";

                    break;

                case "Id":
                    Id1 = Nodo.Token.Value.ToString();
                    break;
                #endregion

                default:
                    // Console.WriteLine(Nodo.Term.Name.ToString());
                    break;
            }
        }

        public bool ValidacionVariables(String NombreVariable, String Ambito)
        {

            return true;
        }
    }
}
