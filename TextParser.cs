using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DM_Assignment_6a_Static_Analysis_Data_Structure
{
    public class TextParser
    {
        AbstractSyntaxTree AST;
        List<List<string>> commands;

        public List<List<string>> ParseString(string commandsString)
        {
            var commandsList = commandsString.Split("\n");
            List<List<string>> res = new List<List<string>>();
            foreach (var command in commandsList)
            {
                res.Add(new List<string>(command.Split(" ")));
            }

            return res;
        }

        //public AbstractSyntaxTree MakeAST(List<List<string>> commands)
        //{
        //    var AST = new AbstractSyntaxTree();
            
        //    return AST
        //}
    }
}
