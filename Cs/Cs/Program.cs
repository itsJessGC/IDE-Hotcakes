
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;

   


namespace Cs
{
    class Program
    {
       public struct token 
        {
            public token_types type;
            
            public string lexema ;
        }
        public const int MAXLENID = 32;
        public const int MAXLENBUF = 1024;
        public const int MAXRESWORDS = 4;

       public enum token_types {
            TKN_BEGIN, TKN_END, TKN_READ, TKN_WRITE, TKN_ID,
            TKN_NUM, TKN_LPAREN, TKN_RPAREN, TKN_SEMICOLON, TKN_COMMA,
            TKN_ASSIGN, TKN_ADD, TKN_MINUS, TKN_EOF, TKN_ERROR
        }


        enum States {
            IN_START, IN_ID, IN_NUM, IN_LPAREN, IN_RPAREN, IN_SEMICOLON,
            IN_COMMA, IN_ASSIGN, IN_ADD, IN_MINUS, IN_EOF, IN_ERROR, IN_DONE
        }
        static int i = 0;
        static Stack pila1 = new Stack();
        static Stack pila2 = new Stack();
        //pila.Push(valor);
        //imprimir(pila);
        /*static void imprimir(Stack pila)
129         {
130             if (pila.Count > 0)
131             {
132                 Console.WriteLine("");
133                 foreach (int dato in pila)
134                 {
135                     Console.WriteLine(" | |");
136                     if( dato <10)
137                         Console.WriteLine(" | 0{0} |", dato);
138                     else
139                         Console.WriteLine(" | {0} |", dato);
140                     Console.WriteLine(" |______|");
141                 }
142                 Console.WriteLine("\nPresione cualquier tecla para continuar...");
143                 Console.ReadKey();
144             }
145             else 
146             {
147                 mensaje("La Pila esta vacia");                
148             }
149         }
150     }
151 }*/

      

        static public ArrayList listaTokens = new ArrayList();
        static public ArrayList listaTokens2 = new ArrayList();
        static public ArrayList listaTokens3 = new ArrayList();
        static public ArrayList lista = new ArrayList();
        public class tokens
        {
            public string palabra { get; set; }
            public string tipo { get; set; }
            public int linea { get; set; }

        }

        static void listar(string palabra, string tipo, int linea)
        {
            listaTokens.Add(new tokens()
            {
                palabra = palabra,
                tipo = tipo,
                linea = linea
            });
            
            return;
        }
        static void listar2(string tipo, string palabra, int linea)
        {
            listaTokens2.Add(new tokens()
            {
                palabra = palabra,
                tipo = tipo,
                linea = linea
            });

            return;
        }
        static void guarda(string palabra, string tipo, int linea)
        {
            lista.Add(new tokens()
            {
                palabra = palabra,
                tipo = tipo,
                linea = linea
            });

            return;
        }
        public class Nodo
        {
            private string dato;
            private Nodo hijo;
            private Nodo hermano;

            public String Dato { get=>dato; set=>dato=value; }
            public Nodo Hijo { get=>hijo; set=>hijo=value; }
            public Nodo Hermano { get=>hermano; set=>hermano= value; }
            public Nodo()
            {
                dato = "";
                hijo = null;
                hermano = null;
            }

        }
        class ArbolGenerico
        {
            private Nodo raiz;
            private Nodo trabajo;
            
            public ArbolGenerico()
            {
                raiz = new Nodo();
            }
            
            public Nodo Insertar (string pDato , Nodo pNodo)
            {
                //si no hay nodo donde insertar , tomamos como si fuera en la raiz 
                if (pNodo == null)
                {
                    raiz = new Nodo();
                    raiz.Dato = pDato;
                    //no hay hijo
                    raiz.Hijo = null;

                    raiz.Hermano = null;
                    return raiz;
                }
                //verificamos si no tiene hijo 
                //insertamos el nuevo nodo como hijo
                if (pNodo.Hijo == null)
                {
                    Nodo temp = new Nodo();
                    temp.Dato = pDato;
                    //conectamos el nuevo nodo como hijo 
                    pNodo.Hijo = temp;

                    return temp;
                }else
                {//si ya tiene hijos se pone como hermano 
                    trabajo = pNodo.Hijo;
                    while (trabajo.Hermano != null)
                    {
                        trabajo = trabajo.Hermano;
                    }
                    //creamos el nodo temporal
                    Nodo temp = new Nodo();
                    temp.Dato = pDato;
                    trabajo.Hermano = temp;
                    return temp;
                }

            }
            
        }
        static void TransversaPreO(Nodo pNodo)
        {

            if (pNodo == null)
                return;
            for (int n = 0; n < i; n++)
                Console.Write("     ");

            Console.WriteLine(pNodo.Dato);
            //luego proceso a mi hijo 
            if (pNodo.Hijo != null)
            {
                i++;
                TransversaPreO(pNodo.Hijo);
                i--;
            }
            if (pNodo.Hermano != null)
                TransversaPreO(pNodo.Hermano);
        }

        //esta funcion checa las palabras que le son enviadas para identificar 
        //si estas son palabras reservadas 
        //aqui la meta es que cheque los identificadores , los numeros , digitos y caracteres 

        static void queTokenes(String palabra,int No)
        { 
            if (palabra.Equals("program") ||
            palabra.Equals("if") ||
            palabra.Equals("then") ||
            palabra.Equals("else") ||
            palabra.Equals("fi") ||
            palabra.Equals("do") ||
            palabra.Equals("until") ||
            palabra.Equals("while") ||
            palabra.Equals("read") ||
            palabra.Equals("write") ||
            palabra.Equals("float") ||
            palabra.Equals("int") ||
            palabra.Equals("bool") ||
            palabra.Equals("not") ||
            palabra.Equals("and") ||
            palabra.Equals("or")
            //palabra.Length == 1
            )
            {
                listar(palabra, "TKN_RESERVA", No);
                return;
            }
            if (palabra.Equals("")) return;
            palabra = " " + palabra + " ";
            
            bool isMatch = Regex.IsMatch(palabra, @"\s[a-z]{1}[a-z]*\s", RegexOptions.IgnoreCase);
            if (isMatch)
            {
                listar(palabra, "TKN_IDE", No);return;
            }
          
            isMatch = Regex.IsMatch(palabra, @"\s[\d]*\.?[\d]*\s");
            if (isMatch) {
                listar(palabra, "TKN_NUM", No);  return; }
           
             isMatch = Regex.IsMatch(palabra, @"\s[0-9]\s");
            if (isMatch) { listar(palabra, "TKN_DIG", No);  return; }

            isMatch = Regex.IsMatch(palabra, @"\s[a-z]\s", RegexOptions.IgnoreCase);
            if (isMatch) { listar(palabra, "TKN_CAR", No);  return; }
            listar(palabra, "TKN_ERR", No);
         
            return;
        }


        //esta funcion checa y va haciendo palabras asta que encuentre un digito especial 
        //en caso de que asi sea checa la palabra y la manda a la funcion de arriba 
        //y ademas que identifica los signos especiales 
        static void encontrarTokens(StreamReader fp) {
        token tok;
        tok.lexema = null;
            String linea=null;
            char caracter;
            int index = 0;
            int Nolinea = 1;
            try
            {
                //Read the first line of text
                linea = fp.ReadLine();
                //Continue to read until you reach end of file
                while (linea != null)
                {
                    while (index < linea.Length)
                    {
                        caracter = linea[index];
                        switch (caracter)
                        {
                            case ' ':
                                queTokenes(tok.lexema,Nolinea);
                                tok.lexema = "";
                                index++;
                                break;
                            case '(':

                                queTokenes(tok.lexema, Nolinea);
                                tok.lexema = "";
                                listar("(", "TKN_PAR_A", Nolinea);
                                index++;
                                break;

                            case ')':
                                queTokenes(tok.lexema, Nolinea);
                                tok.lexema = "";
                                listar(")", "TKN_PAR_B", Nolinea);
                                index++;
                                break;

                            case ';':
                                queTokenes(tok.lexema, Nolinea);
                                tok.lexema = "";
                                listar(";", "TKN_PUN_COM", Nolinea);
                                index++;
                                break;

                            case ',':
                                queTokenes(tok.lexema, Nolinea);
                                tok.lexema = "";
                                listar(",", "TKN_COM", Nolinea);
                                index++;
                                break;
                            case ':':
                                queTokenes(tok.lexema, Nolinea);
                                tok.lexema = "";
                                listar(":", "TKN_DOSCOM", Nolinea); 
                                index++;
                                break;
                            case '+':
                                queTokenes(tok.lexema, Nolinea);
                                //if (linea[index + 1].Equals('='))
                               // {
                                //    listar("+=", "TKN_SUM_IGU", Nolinea);
                                //    index++;
                               // }
                               // else
                               // {
                                    listar("+", "TKN_SUM", Nolinea);
                                   
                               // }
                                tok.lexema = "";
                                index++;
                                break;
                            case '-':
                                queTokenes(tok.lexema, Nolinea);
                                tok.lexema = "";
                            //    if (linea[index + 1].Equals('='))
                             //   {
                               //     listar("-=", "TKN_RES_IGUAL", Nolinea);

                                //    index++;
                               // }
                               // else
                               // {
                                    listar("-", "TKN_SUM", Nolinea);
                                   
                               // }
                                index++;
                                break;

                            case '*':
                                queTokenes(tok.lexema, Nolinea);
                                tok.lexema = "";
                                listar("*", "TKN_MULT", Nolinea);
                                index++;
                                break;
                            case '/':
                                queTokenes(tok.lexema, Nolinea);
                                tok.lexema = "";
                                if (linea[index + 1].Equals('/'))
                                {
                                    linea = fp.ReadLine();
                                    Nolinea++;
                                    index = 0;
                                }
                                else if (linea[index + 1].Equals('*'))
                                {
                                    index = index + 2;
                                    while (!linea[index].Equals('*') && !linea[index + 1].Equals('/'))
                                    {
                                        index++;
                                        if (linea.Length < index) {
                                            linea = fp.ReadLine();
                                            Nolinea++;
                                        index = 0;
                                        }

                                    }
                                    index = index + 2;

                                }
                                else
                                {
                                    listar("/", "TKN_MULT", Nolinea);
                                    index++;
                                }
                                break;
                            case '^':
                                queTokenes(tok.lexema, Nolinea);
                                tok.lexema = "";
                                listar("^", "TKN_MULT", Nolinea);
                                index++;
                                break;
                            case '<':
                                queTokenes(tok.lexema, Nolinea);
                                tok.lexema = "";
                                if (linea[index + 1].Equals('='))
                                {
                                    listar("<=", "TKN_COMP", Nolinea);
                                    index++;
                                }
                                else
                                {
                                    listar("<", "TKN_COMP", Nolinea);
                                   
                                }
                                index++;
                                break;
                            case '>':
                                queTokenes(tok.lexema, Nolinea);
                                tok.lexema = "";
                                if (linea[index + 1].Equals('='))
                                {
                                    listar(">=", "TKN_COMP", Nolinea);
                                    index++;
                                }
                                else
                                {
                                    listar(">", "TKN_COMP", Nolinea);
                                    
                                }
                                index++;
                                break;
                            case '=':
                                queTokenes(tok.lexema, Nolinea);
                                tok.lexema = "";
                                if (linea[index + 1].Equals('='))
                                {
                                    listar("==", "TKN_COMP", Nolinea);
                                    
                                    index++;
                                }
                                else
                                {
                                    listar("=", "TKN_COMP", Nolinea);
                                    
                                }
                                index++;
                                break;
                            case '&':
                                queTokenes(tok.lexema, Nolinea);
                                tok.lexema = "";
                                listar("&", "TKN_AND", Nolinea);
                                index++;
                                break;
                                case '|':
                                queTokenes(tok.lexema, Nolinea);
                                tok.lexema = "";
                                listar("|", "TKN_OR", Nolinea);
                                index++;
                                break;
                            case '!':
                                queTokenes(tok.lexema, Nolinea);
                                tok.lexema = "";
                                if (linea[index + 1].Equals('='))
                                {
                                    listar("!=", "TKN_COMP", Nolinea);
                                    index++;
                                }
                                else
                                {
                                    
                                }
                                index++;
                                break;
                            case '{':
                                queTokenes(tok.lexema, Nolinea);
                                tok.lexema = "";
                                listar("{", "TKN_CORCHETE_A", Nolinea);
                               
                                index++;
                                break;
                            case '}':
                                queTokenes(tok.lexema, Nolinea);
                                tok.lexema = "";
                                listar("}", "TKN_CORCHETE_B", Nolinea);
                                
                                index++;
                                break;
                            case '\n':
                                queTokenes(tok.lexema, Nolinea);
                                tok.lexema = "";
                                 index++;
                                break;
                            default:
                                tok.lexema += caracter;
                                index++;
                                if(index==linea.Length)
                                queTokenes(tok.lexema, Nolinea);
                                break;
                        }
                    }
                    tok.lexema = "";
                    linea = fp.ReadLine();
                    Nolinea++;
                    index = 0;
                }
            }catch(Exception e)
            {

            }
        
        }
        static void Main(string[] args)// aqui se reciven los atributos que seria solo el del archivo que va a buscar 
        {
            bool error = false;
            try
            {
                StreamReader fp = new StreamReader(args[0]);// aqui va el args[] y no se cual lleve el nombre del archivo 
      
                //esto es para ver como imprime el string

                //esto se encarga de encontrar los tokens en el string
                encontrarTokens(fp);
                
                foreach (tokens t in listaTokens)
                {
                    Console.WriteLine("{lexema : "+t.palabra+"  Token: "+t.tipo+"  No Linea: "+t.linea+"}");
                    if (t.tipo.Equals("TKN_ERR"))
                        error = true;
                }
                var tem = 1;
                var label=1;
                

                Console.WriteLine("aqui");
                if (error == false)
                {
                   // bool bien=Gramaticas();
                    Arbol();
                }
                int fila = 0;
                int cantidadToken = 0;
                bool bandera = false;
                int no = 0;
                String[,] tabla;
                tabla = new String[20, 3];
                Console.WriteLine("tablita");
                for (int i =0;i< listaTokens.Count;i++)
                {
                    tokens j = (tokens)listaTokens[i];
                    if (j.palabra == "int" || j.palabra == "float"
                        )
                    {
                        if (i<listaTokens.Count-1)
                        {
                            tokens j2 = (tokens)listaTokens[i + 1];

                            if (j2.tipo == "TKN_IDE" || j2.tipo == "TKN_CAR")
                            {
                                for (int m=0;m<=fila;m++)
                                {
                                    if (j2.palabra == tabla[m,1])
                                    {
                                        bandera = false;
                                        Console.WriteLine("Error en declaracion: " + j.linea);
                                        break;
                                        
                                    }
                                    else
                                    {
                                        bandera = true;
                                    }  
                                }
                                if (bandera)
                                {
                                    tabla[fila, 0] = j.palabra;
                                    tabla[fila, 1] = j2.palabra;
                                }
                                bandera = false;
                                ++fila;
                            }
                            else
                            {
                                Console.WriteLine("Error en declaracion: " + j.linea);
                                break;
                            }
                        }
                        else {
                            Console.WriteLine("Error en declaracion: " + j.linea);
                            break;
                        }
                    }
                    
                    
                       if (j.tipo == "TKN_IDE" || j.tipo == "TKN_CAR")
                           {

                        if (i < listaTokens.Count - 1)
                        {
                            
                            tokens j3 = (tokens)listaTokens[i + 1];
                            
                            if (j3.palabra == "=")
                            {
                                listaTokens2.Clear();
                                cantidadToken = sublista(i + 2);
                               
                                tokens anterior, despues, actual;
                                tokens resultado = new tokens();
                                tokens susti = new tokens();
                                for (int v=0; v < cantidadToken; v++)
                                {
                                    susti = (tokens)listaTokens2[v];
                                    if(susti.tipo== "TKN_IDE" || susti.tipo == "TKN_CAR")
                                    {
                                        for (int n = 0; n <= fila; n++)
                                        {
                                            if (susti.palabra == tabla[n, 1])
                                            {
                                                susti.palabra = tabla[n, 2];
                                                listaTokens2[v] = susti;
                                                break;
                                            }
                                            else 
                                            {
                                                no++;
                                               
                                            }
                                            if (no == fila)
                                            {
                                                Console.WriteLine("no se a declarado la variable: " + susti.palabra + "  en la linea: " + susti.linea);
                                               
                                            }

                                        }
                                    }
                                }
                               
                                if (cantidadToken == 1)
                                {
                                    resultado = (tokens)listaTokens2[0];
                                    
                                }
                                while (cantidadToken > 1)
                                {
                                    //foreach (tokens t in listaTokens2)
                                    //{
                                      //  Console.WriteLine(t.palabra);
                                    //}
                                    for (int n = 1; n < cantidadToken-1; n++)
                                    {
                                       // Console.WriteLine(listaTokens2.Count);


                                        actual = (tokens)listaTokens2[n];
                                        anterior = (tokens)listaTokens2[n - 1];
                                        despues = (tokens)listaTokens2[n + 1];
                                        if (actual.palabra == "*")
                                        {
                                            float resul = float.Parse(anterior.palabra) * float.Parse(despues.palabra);

                                            resultado.tipo = "anonima";
                                            resultado.palabra = resul.ToString();
                                            resultado.linea = actual.linea;
                                            listaTokens2[n] = resultado;
                                            listaTokens2.RemoveAt(n - 1);
                                            listaTokens2.RemoveAt(n);
                                            cantidadToken = cantidadToken - 2;
                                            n = n - 1;


                                        }

                                        if (actual.palabra == "/")
                                        {
                                            
                                            float resul = float.Parse(anterior.palabra) / float.Parse(despues.palabra);

                                            resultado.tipo = "anonima";
                                            resultado.palabra = resul.ToString();
                                            resultado.linea = actual.linea;
                                            listaTokens2[n] = resultado;
                                            listaTokens2.RemoveAt(n - 1);
                                            listaTokens2.RemoveAt(n);
                                            cantidadToken = cantidadToken - 2;
                                            n = n - 1;

                                        }

                                        
                                    }

                                    
                                    for (int n = 1; n < listaTokens2.Count; n++)
                                    {
                                       
                                        actual = (tokens)listaTokens2[n];
                                        anterior = (tokens)listaTokens2[n - 1];
                                        despues = (tokens)listaTokens2[n + 1];
                                        if (actual.palabra == "+")
                                        {
                                            float resul = float.Parse(anterior.palabra) + float.Parse(despues.palabra);

                                            resultado.tipo = "anonima";
                                            resultado.palabra = resul.ToString();
                                            resultado.linea = actual.linea;
                                            listaTokens2[n] = resultado;
                                            listaTokens2.RemoveAt(n - 1);
                                            listaTokens2.RemoveAt(n);
                                            cantidadToken = cantidadToken - 2;
                                            n = n - 1;
                                        }
                                        else
                                        if (actual.palabra == "-")
                                        {
                                            float resul = float.Parse(anterior.palabra) - float.Parse(despues.palabra);

                                            resultado.tipo = "anonima";
                                            resultado.palabra = resul.ToString();
                                            resultado.linea = actual.linea;
                                            listaTokens2[n] = resultado;
                                            listaTokens2.RemoveAt(n - 1);
                                            listaTokens2.RemoveAt(n);
                                            cantidadToken = cantidadToken - 2;
                                            n = n - 1;
                                        }

                                    }
                                }
                                for (int n = 0; n <= fila; n++)
                                {
                                    if (tabla[n, 1] == j.palabra)
                                    {
                                        tabla[n, 2] = resultado.palabra;
                                    }
                                    else
                                    {
                                      //  Console.WriteLine("no se a declarado la variable");
                                    }
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Error en declaracion: " + j.linea);
                            break;
                        }

                                          
                       


                   }
               
              
                }
               
                for (int i = 0; i < fila; i++)
                {
                    if (tabla[i, 2].Contains("."))
                    {
                        Console.WriteLine("tipo: float   nombre: " + tabla[i, 1] + "  valor: " + tabla[i, 2]);
                    }
                    else
                    {

                        Console.WriteLine("tipo: int  nombre: " + tabla[i, 1] + "  valor: " + tabla[i, 2]);
                    }
                  
                }
                listaTokens3 = listaTokens;
                Console.WriteLine(" ");
                Console.WriteLine(" ");
                Console.WriteLine(" ");
                Console.WriteLine(" ");
                for (int i = 2; i < listaTokens3.Count-1; i++)
                {
                    tokens actual = (tokens)listaTokens3[i];
                    if (actual.palabra == "int" || actual.palabra == "float")
                    {
                        tokens sig = (tokens)listaTokens3[i + 1];
                        Console.WriteLine(actual.palabra + ' ' + sig.palabra);
                        i = i + 2;
                    }
                    if (actual.tipo == "TKN_IDE")
                    {
                        tokens sig = (tokens)listaTokens3[i + 1];
                        if (sig.palabra == "=")
                        {
                            tokens posterior = (tokens)listaTokens3[i + 3];
                            if (posterior.palabra == "+" ||
                                posterior.palabra == "-" ||
                                posterior.palabra == "*" ||
                                posterior.palabra == "/"
                                )
                            {
                                tokens otrosigno = (tokens)listaTokens3[i + 5];
                                if (otrosigno.palabra == "+" ||
                                otrosigno.palabra == "-" ||
                                otrosigno.palabra == "*" ||
                                otrosigno.palabra == "/")
                                {
                                    if ((posterior.palabra == "+" || posterior.palabra == "-") && (otrosigno.palabra == "*" || otrosigno.palabra == "/"))
                                    {
                                        tokens antespos = (tokens)listaTokens3[i + 4];
                                        tokens posposterior = (tokens)listaTokens3[i + 6];
                                        Console.WriteLine("t" + tem + "=" + antespos.palabra + otrosigno.palabra + posposterior.palabra);
                                        antespos.palabra = "t" + tem;
                                        antespos.tipo = "TKN_IDE";
                                        tem = tem + 1;
                                        listaTokens3[i + 4] = antespos;
                                        listaTokens3.RemoveAt(i + 5);
                                        listaTokens3.RemoveAt(i + 6);
                                        i = i - 1;
                                    }
                                    else
                                    {
                                        tokens antespos = (tokens)listaTokens3[i + 2];
                                        tokens posposterior = (tokens)listaTokens3[i + 4];
                                        Console.WriteLine("t" + tem + "=" + antespos.palabra + posterior.palabra + posposterior.palabra);
                                        antespos.palabra = "t" + tem;
                                        antespos.tipo = "TKN_IDE";
                                        tem = tem + 1;
                                        listaTokens3[i + 2] = antespos;
                                        listaTokens3.RemoveAt(i + 3);
                                        listaTokens3.RemoveAt(i + 4);
                                        i = i - 1;
                                    }
                                  
                                   
                                }
                                else
                                {
                                    tokens antespos = (tokens)listaTokens3[i + 2];
                                    tokens posposterior = (tokens)listaTokens3[i + 4];
                                    Console.WriteLine("t" + tem + "=" + antespos.palabra + posterior.palabra + posposterior.palabra);
                                    antespos.palabra = "t" + tem;
                                    antespos.tipo = "TKN_IDE";
                                    tem = tem + 1;
                                    listaTokens3[i + 2] = antespos;
                                    listaTokens3.RemoveAt(i + 3);
                                    listaTokens3.RemoveAt(i + 4);
                                    i = i - 1;
                                }


                            }
                            else
                            {
                                tokens sig2 = (tokens)listaTokens3[i + 2];
                                Console.WriteLine(actual.palabra + ' ' + sig.palabra + ' ' + sig2.palabra);
                                i = i + 3;
                            }
                        }
                    }
                    if (actual.palabra == "if")
                    {
                        tokens yo = (tokens)listaTokens3[i + 5];
                        if (yo.palabra=="&" || yo.palabra=="|") {
                            tokens comp1 = (tokens)listaTokens3[i + 3];
                            tokens comp2 = (tokens)listaTokens3[i + 7];
                            tokens ante1 = (tokens)listaTokens3[i + 2];
                            tokens desp1 = (tokens)listaTokens3[i + 4];
                            tokens ante2 = (tokens)listaTokens3[i + 6];
                            tokens desp2 = (tokens)listaTokens3[i + 8];

                            Console.WriteLine("t" + tem + " =" + ante1.palabra + comp1.palabra + desp1.palabra);
                            tem = tem + 1;
                            Console.WriteLine("t" + tem + " =" + ante2.palabra + comp2.palabra + desp2.palabra);
                            tem = tem + 1;
                            Console.WriteLine("t" + tem + " =t" +(tem-2)+" "+yo.palabra +"t" +(tem-1));

                            ante1.palabra = "t" + tem;
                            ante1.tipo = "TKN_IDE";
                            listaTokens3[i + 2] = ante1;
                            listaTokens3.RemoveAt(i + 3);
                            listaTokens3.RemoveAt(i + 4);
                            listaTokens3.RemoveAt(i + 5);
                            listaTokens3.RemoveAt(i + 6);
                            listaTokens3.RemoveAt(i + 7);
                            listaTokens3.RemoveAt(i + 8);
                            tem = tem + 1;
                            
                        }
                        tokens comp = (tokens)listaTokens3[i + 3];
                        if (comp.tipo=="TKN_COMP") {
                            tokens ante = (tokens)listaTokens3[i + 2];
                            tokens aqui = (tokens)listaTokens3[i + 3];
                            tokens desp = (tokens)listaTokens3[i + 4];

                            Console.WriteLine("t" + tem + " =" + ante.palabra + aqui.palabra + desp.palabra);
                            ante.palabra = "t" + tem;
                            ante.tipo = "TKN_IDE";
                            listaTokens3[i + 2] = ante;
                            listaTokens3.RemoveAt(i + 3);
                            listaTokens3.RemoveAt(i + 4);
                            tem = tem + 1;
                            i = i - 1;
                        } else {
                            tokens ante = (tokens)listaTokens3[i + 2];
                            Console.WriteLine("if_false "+ante.palabra+" goto L"+label);
                            
                            i=i+4;

                        }
                    }
                    if (actual.palabra == "}")
                    {
                        Console.WriteLine("label"+label);
                        label = label + 1;
                    }
                }
                Console.WriteLine("halt");
                listaTokens.Clear();
               
                //close the file
                fp.Close();
               
            }
            catch (Exception e)
            {
                //Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                
            }
        }

        private static int sublista(int posi)
        {
            int num = 0;
            tokens j2 = new tokens();
            do
            {
                if (posi < listaTokens.Count)
                {
                    j2 = (tokens)listaTokens[posi];
                    if (j2.palabra != ";")
                    {
                        listar2(j2.tipo, j2.palabra, j2.linea);
                        num++;
                    }
                }
                posi = posi + 1;
                
            } while (j2.palabra!=";");

            return num;
        }

        private static bool Gramaticas()
        {
            String pop;
            foreach (tokens t in listaTokens)
            {
                tokens j = (tokens)listaTokens[1];
                if (t.palabra.Equals("if")){
                    pila1.Push("}");
                    pila1.Push("sec-sent");
                    pila1.Push("{");
                    pila1.Push("else");
                    pila1.Push("}");
                    pila1.Push("sec-sent");
                    pila1.Push("{");
                    pila1.Push(")");
                    pila1.Push("exp");
                    pila1.Push("(");
                }
                if (t.palabra.Equals("while"))
                { 
                    pila1.Push("}");
                    pila1.Push("sec-sent");
                    pila1.Push("{");
                    pila1.Push(")");
                    pila1.Push("exp");
                    pila1.Push("(");
                }
                if (t.palabra.Equals("do"))
                {
                    pila1.Push(";");
                    pila1.Push(")");
                    pila1.Push("exp");
                    pila1.Push("(");
                    pila1.Push("while");
                    pila1.Push("}");
                    pila1.Push("sec-sent");
                    pila1.Push("{");
                }
                if (t.palabra.Equals(""))
                {
                    pila1.Push("}");
                    pila1.Push("sec-sent");
                    pila1.Push("{");
                    pila1.Push("else");
                    pila1.Push("}");
                    pila1.Push("sec-sent");
                    pila1.Push("{");
                    pila1.Push(")");
                    pila1.Push("exp");
                    pila1.Push("(");
                }
                //while
                pop = (string)pila1.Peek();
                if (pop.Equals("sec-sent")) {
                   
                }
                if (pop.Equals("sent"))
                {

                }
                if (pop.Equals("("))
                {
                    if (!t.palabra.Equals("("))
                        return false;
                }
                if (pop.Equals(")"))
                {
                    if (!t.palabra.Equals(")"))
                        return false;
                }
                if (pop.Equals("{"))
                {
                    if (!t.palabra.Equals("{"))
                        return false;
                }
                if (pop.Equals("}"))
                {
                    if (!t.palabra.Equals("}"))
                        return false;
                }
                if (pop.Equals("relacion"))
                {
                    if (!t.tipo.Equals("TKN_COMP"))
                        return false;
                }
                if (pop.Equals("opsuma"))
                {
                    if (!t.tipo.Equals("TKN_SUM"))
                        return false;
                }
                if (pop.Equals("opmult"))
                {
                    if (!t.tipo.Equals("TKN_MULT"))
                        return false;
                }
                if (pop.Equals(";"))
                {
                    if (!t.palabra.Equals(";"))
                        return false;
                }
                if (pop.Equals("numero")) {
                    if (!t.tipo.Equals("TKN_DIG")|| !t.tipo.Equals("TKN_NUM"))
                        return false;
                }
                if (pop.Equals("identificador"))
                {
                    if (!t.tipo.Equals("TKN_IDE"))
                        return false;
                }
                if (pop.Equals("exp"))
                {
                    pila1.Pop();
                    pila1.Push("exp_simple");
                    pila1.Push("relacion");
                    pila1.Push("exp_simple");
                }
                if (pop.Equals("sent"))
                {

                }
            }
            return true;
        }

        private static void Arbol()
        {
            //ArbolGenerico arbol = new ArbolGenerico();
            //bool primera = true;
           // Nodo raiz = new Nodo();
            //Nodo P = new Nodo(); 
            //Nodo h = new Nodo();
           // Nodo m = new Nodo();
            int nivel = 0;
            String espacios="";
            foreach (tokens t in listaTokens)
             {
                espacios = "";
                String token= t.palabra;
                if (token.Contains("program") || 
                    token.Contains("{")|| 
                    token.Contains("int") ||
                    token.Contains("write") ||
                    token.Equals("do") ||
                    token.Equals("if") ||
                    token.Contains("(") ||
                    //token.Equals("until") ||
                   // token.Equals("then") ||
                    token.Equals("float") ||
                    //token.Equals("else")||
                    //token.Equals("while")||
                     token.Equals("=")
                    ) {
                    
                    for (int i=0; i<nivel;i++) {
                        espacios += "       ";
                    }
                    nivel++;
                    if (token.Equals("=")) { }
                    else
                    {

                        Console.WriteLine(espacios + token );
                    }
                    
                }
                else if (token.Equals("}") ||
                    token.Equals(";") ||
                    token.Equals(")") ||
                    token.Equals("fi") )
                     {
                    nivel--;
                    for (int i = 0; i < nivel; i++)
                    {
                        espacios += "       ";
                    }
                    if (token.Equals(";"))
                        continue;

                    Console.WriteLine(espacios+token);
                    
                }
                else {
                    if (token.Equals(","))
                        continue;
                    for (int i = 0; i < nivel; i++)
                    {
                        espacios += "       ";
                    }
                    Console.WriteLine(espacios+token);
                }
                
                 // que inicie con program 
                // if (t.palabra.Equals("program") && primera==true)
                // {

                     //raiz = arbol.Insertar(t.palabra, null);
                     //primera = false;
                    
                    
                // }
                 //else if(primera == true)
                 //{
                     //Console.WriteLine("Error en el codigo");
                     //return;
                 //}
                 //switch (t.tipo)
                //{
                    //case "TKN_RESERVA":
                        // if (t.palabra.Equals("int"))
                        //// {
                        //if (P.Dato.Equals(""))
                         //   P = arbol.Insertar(t.palabra, raiz);
                        //else
                          //  P = arbol.Insertar(t.palabra, P);
                        // }


                        //break;
                    //case "TKN_IDE": case "TKN_CAR":
                    //case "TKN_NUM":
                    //case "TKN_DIG":
                        //if (P.Dato.Equals(""))
                           // h = arbol.Insertar(t.palabra, raiz);
                       // else
                           // h = arbol.Insertar(t.palabra, P);
                       // break;
                    //case "TKN_PUN_COM":
                       
                       // break;
                }
            if (nivel == 1)
            {
                Console.WriteLine("el codigo esta bien");
            }
            else
            {
                Console.WriteLine("el codigo tiene error   ");
            }
            /*
            if (t.palabra.Equals("("))
            {
               pila1.Push("(");
            }
           if (t.palabra.Equals("("))
           {
               pila1.Push("(");
           }
           if (t.palabra.Equals("{"))
            {

            }
            if (t.tipo.Equals("TKN_RESERVA") && !t.palabra.Equals("program"))
            {
                P = arbol.Insertar(t.palabra, raiz);
            }
           if (t.tipo.Equals("TKN_IDE") || t.tipo.Equals("TKN_CAR"))
           {
               arbol.Insertar(t.palabra, P);
           }
            */

        }

             
            
            //TransversaPreO(raiz);
        }

    }
            
        
