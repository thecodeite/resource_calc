using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResourceCalculator
{
    class ResourceEngine
    {
        readonly Dictionary<string, ResourceExpression> _resourceTable = new Dictionary<string, ResourceExpression>();
        readonly Dictionary<string, string> _explinations = new Dictionary<string, string>();

        public void Parse(string script)
        {
            var parser = new Parser();

            var lines = script.Split(new char[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                var parsedLine = parser.ParseLine(line);

                var assignment = parsedLine as Assignment;
                var explination = parsedLine as Explination;

                if (assignment != null)
                {
                    if (assignment.Variable.Multiplyer != 1f)
                    {
                        foreach (var key in assignment.Expression.Recipie.Keys.ToList())
                        {
                            assignment.Expression.Recipie[key] /= assignment.Variable.Multiplyer;
                        }
                    }

                    _resourceTable[assignment.Variable.Name] = assignment.Expression;
                }

                if (explination != null)
                {
                    _explinations[explination.Variable.Name] = explination.FullName;
                }
            }
        }

        public Dictionary<string, ResourceExpression> GetResourceTable()
        {
            return _resourceTable;
        }
        
        public Dictionary<string, string> GetExplinationTable()
        {
            return _explinations;
        }

        public Recipie Evaluate(string resource, float multiplyer = 1f)
        {
            return Evaluate(new Variable(resource, multiplyer));
        }

        public Recipie Evaluate(Variable resource)
        {
            var recipie = new Recipie();

            var resourceName = resource.Name;
            var multiplyer = resource.Multiplyer;

            if (!_resourceTable.ContainsKey(resourceName))
            {
                recipie.Add(resourceName, 1);
                return recipie;
            }

            var exp = _resourceTable[resourceName];

            foreach (var kvp in exp.Recipie)
            {
                if (_resourceTable.ContainsKey(kvp.Key))
                {
                    foreach (var c in Evaluate(kvp.Key).Items)
                    {
                        recipie.Add(c.Key, c.Value * kvp.Value * multiplyer);
                    }
                }
                else
                {
                    recipie.Add(kvp.Key, kvp.Value * multiplyer);
                }
            }

            return recipie;
        }

        public string RecipieFor(string variableString)
        {
            var builder = new StringBuilder();

            var parser = new Parser();
            Variable variable = parser.ParseVariable(variableString);

            var cost = Evaluate(variable).Items;

            // Assert
            foreach (var f in cost)
            {
                builder.AppendFormat("{0,20} x {1}\r\n", Name(f.Key, _explinations), f.Value);
            }

            return builder.ToString();
        }

        private string Name(string key, Dictionary<string, string> explinations)
        {
            if (explinations.ContainsKey(key))
            {
                return explinations[key];
            }
            else
            {
                return key;
            }
        }
    }

    public class Recipie
    {
        public Dictionary<string, float> Items { get; private set; }

        public Recipie()
        {
            Items = new Dictionary<string, float>();
        }

        public void Add(string key, float value)
        {
            if (Items.ContainsKey(key))
            {
                Items[key] += value;
            }
            else
            {
                Items[key] = value;
            }
        }
    }
}
