using System;
using System.Collections.Generic;
using System.Linq;

namespace ResourceCalculator
{
    class Parser
    {
        public Line ParseLine(string line)
        {
            var tokenizer = new Tokenizer(line);

            var assignment = ReadLine(tokenizer);

            return assignment;
        }

        public Variable ParseVariable(string line)
        {
            var tokenizer = new Tokenizer(line);

            var variable = ReadVariable(tokenizer);

            return variable;
        }

        private Line ReadLine(Tokenizer tokenizer)
        {
            var variable = ReadVariable(tokenizer);

            var symbol = ReadSymbol(tokenizer);

            if (symbol == "=")
            {
                return ReadAssignment(tokenizer, variable);
            }

            if (symbol == "=>")
            {
                return ReadExplination(tokenizer, variable);
            }

            throw new ParserException("Don't know how to interpret symbol: "+symbol);
        }

        private Assignment ReadAssignment(Tokenizer tokenizer, Variable variable)
        {
            var assignment = new Assignment
            {
                Variable = variable,
                Expression = ReadExpression(tokenizer)
            };


            return assignment;
        }

        private Explination ReadExplination(Tokenizer tokenizer, Variable variable)
        {
            var name = tokenizer.Read();

            if (name.Cat != Token.Category.String)
            {
                throw new ParserException("Expecting a string but got "+name);
            }

            var explination = new Explination
            {
                Variable = variable,
                FullName = name.Value,
            };

            return explination;
        }



        private Variable ReadVariable(Tokenizer tokenizer)
        {
            var token = tokenizer.Read();

            float multiplyer = 1;

            if (token.Cat == Token.Category.Number)
            {
                multiplyer = float.Parse(token.Value);
                token = tokenizer.Read();
            }

            if (token.Cat != Token.Category.Word)
            {
                throw new ParserException(string.Format("Expected variable but got '{0}'", token));
            }

            return new Variable
            {
                Name = token.Value,
                Multiplyer = multiplyer,
            };
        }

        private string ReadSymbol(Tokenizer tokenizer, string expected = null)
        {
            var token = tokenizer.Read();
            if (token == null || token.Cat != Token.Category.Symbol)
            {
                throw new ParserException(string.Format("Expected symbol but got '{0}'", token ));
            }

            if (expected != null && expected != token.Value)
            {
                throw new ParserException(string.Format("Expected symbol '{0}' but got '{1}'", expected, token));
            }

            return token.Value;
        }

        private ResourceExpression ReadExpression(Tokenizer tokenizer)
        {
            var expression = new ResourceExpression();

            Token token;
            while ((token = tokenizer.Read()) != Tokenizer.NullToken)
            {
                var number = 1;
                if (token.Cat == Token.Category.Number)
                {
                    number = int.Parse(token.Value);
                    token = tokenizer.Read();
                }
                
                if (token.Cat == Token.Category.Word)
                {
                    expression.Recipie.Add(token.Value, number);
                }
                else
                {
                    throw new ParserException("EEk! What do I do with: " + token);
                }

                if (tokenizer.Peek() != Tokenizer.NullToken)
                {
                    ReadSymbol(tokenizer, "+");
                }
            }

            return expression;
        }
    }

    internal interface Line
    {
    }

    internal class ParserException : Exception
    {
        public ParserException(string message) : base(message)
        {
        }
    }

    class Explination : Line
    {
        public Variable Variable { get; set; }
        public string FullName { get; set; }
    }

    class Assignment : Line
    {
        public Variable Variable { get; set; }
        public ResourceExpression Expression { get; set; }
    }

    class Variable
    {
        public Variable(string name, float multiplyer)
        {
            Name = name;
            Multiplyer = multiplyer;
        }

        public Variable()
        {
        }

        public string Name { get; set; }
        public float Multiplyer { get; set; }
    }

    class ResourceExpression
    {
        private readonly Dictionary<string, float> _recipie;

        public ResourceExpression(Dictionary<string, float> dictionary = null)
        {
            _recipie = dictionary ?? new Dictionary<string, float>();
        }

        public Dictionary<string, float> Recipie
        {
            get { return _recipie; }
        }
    }
}
