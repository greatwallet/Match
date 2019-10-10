using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match
{
    // Calculate the result from string expression
    class Expr
    {
        const int N_OPTR = 5;
        // Priority Matrix for operators[Top, Current]
        readonly char[,] pri = new char[N_OPTR, N_OPTR] {
        /* Current Operator*/
        /*  +       -       *       \/      \0      */
/*  T    + */   { '>',    '>',    '<',    '<',    '>' },
/*  o    - */   { '>',    '>',    '<',    '<',    '>' },
/*  p    * */   { '>',    '>',    '>',    '>',    '>' },
/*      \/ */   { '>',    '>',    '>',    '>',    '>' },
/*     \0  */   { '<',    '<',    '<',    '<',    '=' }
            };

        // check if a char is digit
        private bool isdigit(char c)
        {
            return c - '0' >= 0 && c - '0' <= 9;
        }

        // get the index of operators in the "pri" Matrix
        private int IndexOfOprd(char op)
        {
            switch (op)
            {
                case '+':return 0;
                case '-':return 1;
                case '*': return 2;
                case '/':return 3;
                case '\0':return 4;
                default:throw new Exception("Error in IndexOfOprd");
            }
        }

        // Compare the priority of 2 operators
        private char orderBetween(char top,char cur)
        {
            return pri[IndexOfOprd(top), IndexOfOprd(cur)];
        }

        // read single or multiple digit - number from the expr, 
        // and increase the index to the next of the end of the  digits
        private double readNumber(string expr,ref int idx)
        {
            double num = 0; // Supposed to be an integer stored in double precision format
            for (; idx < expr.Length && isdigit(expr[idx]); idx++) 
            {
                num *= 10;
                num += Convert.ToDouble(expr[idx] - '0');
            }
            return num;
        }
        // Get the result of calculation of 2 numbers
        private double calcu(double pOpnd1, char op, double pOpnd2)
        {
            switch (op)
            {
                case '+':
                    return pOpnd1 + pOpnd2;
                case '-':
                    return pOpnd1 - pOpnd2;
                case '*':
                    return pOpnd1 * pOpnd2;
                case '/':
                    return pOpnd1 / pOpnd2;
                default:
                    throw new Exception("Error in calcu");
            }
        }
        // To get the result of an normal expr  
        public double evaluate(string expr)
        {
            expr = String.Concat(expr, '\0');
            // Stacks for operands and operators
            var opnd = new Stack<double>();
            var optr = new Stack<char>();
            // Put in ending mark
            optr.Push('\0');
            for (int idx = 0; optr.Count > 0;) 
            {
                if (isdigit(expr[idx]))
                {
                    double num = readNumber(expr, ref idx);
                    opnd.Push(num);
                }
                else
                {
                    switch (orderBetween(optr.Peek(), expr[idx]))
                    {
                        // top < current
                        case '<':
                            optr.Push(expr[idx]);idx++;
                            break;
                        // the expr ends
                        case '=':
                            optr.Pop();idx++;
                            break;
                        // top > current, time for calculating
                        case '>':
                            {
                                // get the operator
                                char op = optr.Pop();
                                // Get 2 operands
                                double pOpnd2 = opnd.Pop(), pOpnd1 = opnd.Pop();
                                opnd.Push(calcu(pOpnd1, op, pOpnd2));
                                break;
                            }
                        default:
                            throw new Exception("Error in evaluate");
                    }
                }
            }
            // opnd.Count Supposed to be 1;
            return opnd.Pop();
        }
    }
}
