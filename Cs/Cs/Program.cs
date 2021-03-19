
using System;
using System.IO;



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


         
     //esta funcion lo que va ser es que elimine los comentarios y dejar todo en un string
     //falta que elimine los comentarios 
      static String makeString(StreamReader fp)
        {
            int desde = 0;
            int hasta = 0;
            int index = 0;
            String line=null;
            String recipiente = null;
            try
            {
                //Read the first line of text
                line = fp.ReadLine();
                //Continue to read until you reach end of file
                while (line != null)
                {
                    while (index < line.Length)
                    {
                        if (line[index].Equals('/'))
                        {
                            if (index+1<line.Length && line[index + 1].Equals('/'))
                            {
                                
                                line=line.Remove(index,line.Length-index);     
                            }
                        }

                        index++;
                    }
             
                    //junta el texto en un string 
                    recipiente += line+" ";
                    //Read the next line
                    line = fp.ReadLine();
                    index = 0;
                   
                }
                index = 0;
                
                while (index < recipiente.Length)
                {
                   if (recipiente[index].Equals('/') && recipiente[index+1].Equals('*'))
                    {
                        desde = index;
                        hasta = index;
                         while (hasta < recipiente.Length)
                            {
                                if (recipiente[hasta].Equals('*') && recipiente[hasta + 1].Equals('/'))
                                {
                                        recipiente = recipiente.Remove(desde, (hasta + 2)-desde);
                                Console.WriteLine("desde="+desde+" hasta="+hasta+" index="+index);
                                index = 0;
                                break;
                                /* // */
                                }
                                hasta++;
                            }   
                    }
                    index++;
                }   
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                
            }

            return recipiente;
        }


        //esta funcion checa las palabras que le son enviadas para identificar 
        //si estas son palabras reservadas 
        //aqui la meta es que cheque los identificadores , los numeros , digitos y caracteres 

        static void queTokenes(String palabra)
        {




            if (palabra.Equals("program") ||
            palabra.Equals("if") ||
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
                Console.WriteLine(palabra + " = TKN_RES");
                return;
            }



            int iii = 0;
            Double iiii = 0.0;
            bool resultParse;
            if (palabra.Length == 1 && palabra!="")
            {
                if (resultParse = int.TryParse(palabra, out iii))
                {
                    //Digito
                    Console.WriteLine(palabra + " = TKN_DIG");
                }
                else
                {
                    //Caracter
                    Console.WriteLine(palabra + " = TKN_CARC");
                }
            }
            if(palabra.Length >1)
            {
                if (resultParse = Double.TryParse(palabra, out iiii))
                {
                    //Número
                    Console.WriteLine(palabra + " = TKN_NUM");
                }
                else
                {
                    //Identificador
                    Console.WriteLine(palabra + " = TKN_IDE");
                }
            }
        }


        //esta funcion checa y va haciendo palabras asta que encuentre un digito especial 
        //en caso de que asi sea checa la palabra y la manda a la funcion de arriba 
        //y ademas que identifica los signos especiales 
        static void encontrarTokens(String linea,int largo) {
        token tok;
            
            tok.lexema = null;
            
            char caracter;
            int index = 0;
            while (index<largo )
            {
                caracter = linea[index];
                switch (caracter)
                {
                    case ' ':
                        queTokenes(tok.lexema);
                        tok.lexema = "";
                        index++;
                        break;
                    case '(':
                        
                        queTokenes(tok.lexema);
                        tok.lexema = "";
                        Console.WriteLine("(   = TKN_PAR_A");
                        index++;
                        break;
                    case ')':
                        queTokenes(tok.lexema);
                        tok.lexema = "";
                        Console.WriteLine(")   = TKN_PAR_B");
                        index++;
                        break;
                    case ';':
                        queTokenes(tok.lexema);
                        tok.lexema = "";
                        Console.WriteLine(";   = TKN_PUN_COM");
                        index++;
                        break;
                    case ',':
                        queTokenes(tok.lexema);
                        tok.lexema = "";
                        Console.WriteLine(",   = TKN_COM");
                        index++;
                        break;
                    case ':':
                        queTokenes(tok.lexema);
                        tok.lexema = "";
                        Console.WriteLine(":   = TKN_DOSCOM");
                        index++;
                        break;
                    case '+':
                        queTokenes(tok.lexema);
                        if(linea[index+1].Equals('=')) {
                            Console.WriteLine("+=   = TKN_SUM_IGUAL");
                            index++;
                        }
                        else
                        {
                            Console.WriteLine("+   = TKN_SUM");
                        }
                        tok.lexema = "";
                        index++;
                        break;
                    case '-':
                        queTokenes(tok.lexema);
                        tok.lexema = "";
                        if (linea[index + 1].Equals('='))
                        {
                            Console.WriteLine("-=   = TKN_RES_IGUAL");
                            index++;
                        }
                        else
                        {
                            Console.WriteLine("-   = TKN_RES");
                        }
                        index++;
                        break;
                    case '*':
                        queTokenes(tok.lexema);
                        tok.lexema = "";
                        Console.WriteLine("*   = TKN_MULT");
                        index++;
                        break;
                    case '/':
                        queTokenes(tok.lexema);
                        tok.lexema = "";
                        Console.WriteLine("/   = TKN_DIV");
                        index++;
                        break;
                    case '^':
                        queTokenes(tok.lexema);
                        tok.lexema = "";
                        Console.WriteLine("^   = TKN_POT");
                        index++;
                        break;
                    case '<':
                        queTokenes(tok.lexema);
                        tok.lexema = "";
                        if (linea[index + 1].Equals('='))
                        {
                            Console.WriteLine("<=   = TKN_MENOR_QUE");
                            index++;
                        }
                        else
                        {
                            Console.WriteLine("<   = TKN_MENOR");
                        }
                        index++;
                        break;
                    case '>':
                        queTokenes(tok.lexema);
                        tok.lexema = "";
                        if (linea[index + 1].Equals('='))
                        {
                           Console.WriteLine(">=   = TKN_MAYOR_QUE");
                            index++;
                        }
                        else
                        {
                            Console.WriteLine(">   = TKN_MAYOR");
                        }
                        index++;
                        break;
                    case '=':
                        queTokenes(tok.lexema);
                        tok.lexema = "";
                        if (linea[index + 1].Equals('='))
                        {
                           Console.WriteLine("==   = TKN_COPAR");
                            index++;
                        }
                        else
                        {
                            Console.WriteLine("=   = TKN_IGUALDAD");
                        }
                        index++;
                        break;
                    case '!':
                        queTokenes(tok.lexema);
                        tok.lexema = "";
                        if (linea[index + 1].Equals('='))
                        {
                            Console.WriteLine("!=   = TKN_DIF");
                            index++;
                        }
                        else
                        {
                            Console.WriteLine("!   = TKN_nose ");
                        }
                        index++;
                        break;
                    case '{':
                        queTokenes(tok.lexema);
                        tok.lexema = "";
                        Console.WriteLine("{    = TKN_CORCHETE_A");
                        index++;
                        break;
                    case '}':
                        queTokenes(tok.lexema);
                        tok.lexema = "";
                        Console.WriteLine("}    = TKN_CORCHETE_B");
                        index++;
                        break;
                    default:
                        tok.lexema  += caracter;
                        index++;
                        break;
                }
            }
        
        }
     
 
                  
                 
                     
         
         
        static void Main(string[] args)// aqui se reciven los atributos que seria solo el del archivo que va a buscar 
        {
            String linea;
          
            try
            {
                StreamReader fp = new StreamReader(args[0]);// aqui va el args[] y no se cual lleve el nombre del archivo 
                                                                                            // esta funcion se encarga de hacer todo el texto en
                                                                                            //un string para manipularlo mucho mas facil
                linea= makeString(fp);
                Console.WriteLine(linea); //esto es para ver como imprime el string

                //esto se encarga de encontrar los tokens en el string
                encontrarTokens(linea,linea.Length);
                       

                  
                //close the file
                fp.Close();
               
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                
            }
        }
    }
}
