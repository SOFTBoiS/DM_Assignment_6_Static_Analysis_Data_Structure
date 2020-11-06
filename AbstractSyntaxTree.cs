using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DM_Assignment_6a_Static_Analysis_Data_Structure
{
    public class AbstractSyntaxTree
    {
        public Node root;

        public AbstractSyntaxTree(List<List<string>> commands)
        {
            root = new Node("");
            root = MakeAST(commands);
        }

        public Node MakeAST(List<List<string>> commands)
        {
            var currNode = new Node("");
            for (int i = 0; i < commands.Count; i++)
            {
                var command = commands[i];
                switch (command[0])
                {
                    case "DEF":
                        DEFHandler(command, currNode);
                        break;

                    case "LET":
                        LETHandler(command, currNode);
                        break;

                    case "IF":
                        var block = new List<List<string>>();

                        //gets code block
                        for (int j = i+1; j < commands.Count; j++)
                        {
                            //updates i so it doesnt reiterate over the same lines
                            if(commands[j][0] == "}")
                            {
                                i = j;
                                break;
                            }
                            block.Add(commands[j]);
                        }

                        //get subAST
                        var subRoot = MakeAST(block);

                        //add subAST to root
                        var IF = new Node(command[0]);
                        var name = new Node(command[1][1..]);
                        var operatorType = new Node(command[2]);
                        var compareValue = new Node(command[3][0..^1]);

                        operatorType.children.Add(name);
                        operatorType.children.Add(compareValue);

                       
                        IF.children.Add(operatorType);
                        IF.children.Add(subRoot);
                        currNode.children.Add(IF);
                        
                        break;

                    default:
                        throw new ArgumentException("Bad initial word. Check VSSL syntax" + String.Join(" ", commands[i].ToArray()));
                }
            }

            return currNode;
            }


        private void LETHandler(List<string> command, Node currNode)
        { 
            if (command.Count == 4)
            {
                var LET = command[0];
                var name = command[1];
                var value = command[3];

                var node = new Node(LET);
                node.children.Add(new Node(name));
                node.children.Add(new Node(value));
                
                currNode.children.Add(node);
            }
            else
            {
                var LET = command[0];
                var name = command[1];
                var refVarName = command[3];
                var commandOperator = command[4];
                var valueToOperator = command[5];

                var comparatorNode = new Node(commandOperator);
                comparatorNode.children.Add(new Node(refVarName));
                comparatorNode.children.Add(new Node(valueToOperator));

                var node = new Node(LET);
                node.children.Add(new Node(name));
                node.children.Add(comparatorNode);

                currNode.children.Add(node);
            }
        }

        private void DEFHandler(List<string> command, Node currNode)
        {
            var DEF = command[0];
            var name = command[1][0..^1]; //Only takes out the variable name and ignores the ":" in the string
            var dataType = command[2];

            var node = new Node(DEF);
            node.children.Add(new Node(name));
            node.children.Add(new Node(dataType));
            currNode.children.Add(node);
        }

        public class Node
        {
            public string value;
            public List<Node> children;

            public Node(string value)
            {
                this.value = value;
                children = new List<Node>();
            }
        }
    }
}

/*
 * DEF X: INTEGER
 * 
 * if(something){
 * LET X = 1
 * LET X = 2
 * LET X = 3
 * }
 * 
 * 
 * 
 * 
 * 
 */
