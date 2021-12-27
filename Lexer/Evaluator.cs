using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexer
{
    internal class Evaluator
    {
        static private Token.Name[] subexpressionkinds =
        {
            Token.Name.OREXPRESSION, Token.Name.ANDEXPRESSION, Token.Name.EQUEXPRESSION, Token.Name.ATOMEXPRESSION
        };

        static public void Eval(Token token)
        {

            Token vardef = token.Children
                .Find(token => token.Type == Token.Name.VARDEFINITION);

            Token varlist = vardef.Children
                .Find(token => token.Type == Token.Name.VARLIST);

            Token assignmentlist = token.Children
                .Find(token => token.Type == Token.Name.CALCDEFINITION).Children
                .Find(token => token.Type == Token.Name.ASSIGNMENTLIST);

            try
            {
                Dictionary<string, bool> varTable = GenVarTable(varlist);
                EvalAssignments(assignmentlist, varTable);
            }
            catch (VarNotDefinedException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (VarAlreadyDefinedException e)
            {
                Console.WriteLine(vardef.Value + "\nПеременная " + e.Message + " объявлена более чем один раз");
            }
        }

        static private Dictionary<string, bool> GenVarTable(Token varlist)
        {
            Dictionary<string, bool> varTable = new();
            string name;

            while (varlist != null)
            {
                name = varlist.Children.Find(token => token.Type == Token.Name.IDENT).Value;

                if (varTable.ContainsKey(name))
                {
                    throw new VarAlreadyDefinedException(name);
                }
                else
                {
                    varTable[name] = false;
                    varlist = varlist.Children.Find(token => token.Type == Token.Name.VARLIST);
                }
            }

            return varTable;
        }

        static private void EvalAssignments(Token assignmentList, Dictionary<string, bool> vars)
        {
            while(assignmentList != null)
            {
                EvalAssignment(
                    assignmentList.Children.Find(token => token.Type == Token.Name.ASSIGNMENT), vars);
                assignmentList = assignmentList.Children.Find(token => token.Type == Token.Name.ASSIGNMENTLIST);
            }
        }

        static private void EvalAssignment(Token assignment, Dictionary<string, bool> vars)
        {
            try
            {
                if (assignment.Children.Any(token => token.Type == Token.Name.ASSIGNMENTCHAR))
                {
                    string name = assignment.Children.Find(token => token.Type == Token.Name.IDENT).Value;

                    if (!vars.ContainsKey(name))
                    {
                        throw new VarNotDefinedException(name);
                    }

                    Token expression = assignment.Children.Find(token => token.Type == Token.Name.EXPRESSION);
                    vars[name] = EvalExpression(expression, vars);
                }
                else if (assignment.Children.Any(token => token.Type == Token.Name.OPERATOR))
                {
                    Token oper = assignment.Children.Find(token => token.Type == Token.Name.OPERATOR);
                    EvalOperator(oper, vars);
                }
            }
            catch (VarNotDefinedException e)
            {
                throw new VarNotDefinedException(
                    assignment.Value + "\nПеременная " + e.Message + " не объявлена");
            }
        }

        static private void EvalOperator(Token oper, Dictionary<string, bool> vars)
        {
            if (oper.Children.Any(token => token.Type == Token.Name.READ))
            {
                Token read = oper.Children.Find(token => token.Type == Token.Name.READ);
                EvalRead(read, vars);
            }
            else if (oper.Children.Any(token => token.Type == Token.Name.WRITE))
            {
                Token write = oper.Children.Find(token => token.Type == Token.Name.WRITE);
                EvalWrite(write, vars);
            }
            else if (oper.Children.Any(token => token.Type == Token.Name.CONDITION))
            {
                Token cond = oper.Children.Find(token => token.Type == Token.Name.CONDITION);
                EvalCondition(cond, vars);
            }
        }

        static private void EvalRead(Token read, Dictionary<string, bool> vars)
        {
            Token varlist = read.Children.Find(token => token.Type == Token.Name.VARLIST);

            String name, value;

            while (varlist != null)
            {
                name = varlist.Children.Find(token => token.Type == Token.Name.IDENT).Value;

                if (!vars.ContainsKey(name))
                {
                    throw new VarNotDefinedException(name);
                }

                value = Console.ReadLine();

                if (value == "0" || value == "1")
                {
                    vars[name] = value == "1";
                }

                varlist = varlist.Children.Find(token => token.Type == Token.Name.VARLIST);
            }

            Console.WriteLine();
        }

        static private void EvalWrite(Token write, Dictionary<string, bool> vars)
        {
            Token varlist = write.Children.Find(token => token.Type == Token.Name.VARLIST);
            string name;

            while (varlist != null)
            {
                name = varlist.Children.Find(token => token.Type == Token.Name.IDENT).Value;

                if (!vars.ContainsKey(name))
                {
                    throw new VarNotDefinedException(name);
                }

                Console.Write(vars[name] ? "1 " : "0 ");
                varlist = varlist.Children.Find(token => token.Type == Token.Name.VARLIST);
            }

            Console.WriteLine();
        }

        static private void EvalCondition(Token cond, Dictionary<string, bool> vars)
        {
            List<Token> lists = cond.Children.Where(token => token.Type == Token.Name.ASSIGNMENTLIST).ToList();
            Token expr = cond.Children.Find(token => token.Type == Token.Name.EXPRESSION);
            EvalAssignments(lists[EvalExpression(expr, vars) ? 1 : 0], vars);
        }

        static private bool EvalExpression(Token expression, Dictionary<string, bool> vars)
        {
            Token subexpr = expression.Children
                    .Find(token => subexpressionkinds.Contains(token.Type));

            return EvalSubexpression(subexpr, vars);
        }

        static private bool EvalSubexpression(Token subexpression, Dictionary<string, bool> vars)
        {
            List<Token> subexprs = subexpression.Children
                    .Where(token => subexpressionkinds.Contains(token.Type)).ToList();

            if (subexpression.Children.Any(token => token.Type == Token.Name.BINARYOR))
            {
                return EvalSubexpression(subexprs[0], vars) || EvalSubexpression(subexprs[1], vars);
            }
            else if (subexpression.Children.Any(token => token.Type == Token.Name.BINARYAND))
            {
                return EvalSubexpression(subexprs[0], vars) && EvalSubexpression(subexprs[1], vars);
            }
            else if (subexpression.Children.Any(token => token.Type == Token.Name.BINARYEQU))
            {
                return EvalSubexpression(subexprs[0], vars) == EvalSubexpression(subexprs[1], vars);
            }
            else if (subexpression.Children.Any(token => token.Type == Token.Name.OPERAND))
            {
                Token operand = subexpression.Children.Find(token => token.Type == Token.Name.OPERAND);
                return EvalOperand(operand, vars);
            }
            else if (subexpression.Children.Any(token => token.Type == Token.Name.EXPRESSION))
            {
                Token expr = subexpression.Children.Find(token => token.Type == Token.Name.EXPRESSION);
                return EvalExpression(expr, vars);
            }

            return false;
        }

        static private bool EvalOperand(Token operand, Dictionary<string, bool> vars)
        {
            if (operand.Children.Any(token => token.Type == Token.Name.IDENT))
            {
                Token ident = operand.Children.Find(token => token.Type == Token.Name.IDENT);

                if (!vars.ContainsKey(ident.Value))
                {
                    throw new VarNotDefinedException(ident.Value);
                }

                return vars[ident.Value];
            }
            else if (operand.Children.Any(token => token.Type == Token.Name.CONSTANT))
            {
                Token ident = operand.Children.Find(token => token.Type == Token.Name.CONSTANT);
                return !(ident.Value == "0");
            }

            return false;
        }
    }
}
