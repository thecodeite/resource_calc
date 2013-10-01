using System.Text;

namespace ResourceCalculator
{
    internal class Tokenizer
    {
        private readonly char[] _buffer;
        private readonly int _end;
        private readonly StringBuilder _builder = new StringBuilder();

        private int _position;
        private Token _nextToken;

        public static Token NullToken = new Token("", Token.Category.Null);

        public Tokenizer(string line)
        {
            _buffer = line.ToCharArray();
            _end = _buffer.Length;
            _nextToken = ReadNextToken();
        }

        private Token ReadNextToken()
        {
            while (_position < _end && char.IsWhiteSpace(_buffer[_position]))
            {
                _position++;
            }

            if (_position >= _end)
            {
                return NullToken;
            }

            if (_buffer[_position] == '"')
            {
                _builder.Clear();
                _position++;
                while (_position < _end && _buffer[_position] != '"')
                {
                    _builder.Append(_buffer[_position]);
                    _position++;
                }
                _position++;

                return new Token(_builder.ToString(), Token.Category.String);
            }

            if (char.IsSymbol(_buffer[_position]))
            {
                _builder.Clear();
                while (_position < _end && (char.IsSymbol(_buffer[_position])))
                {
                    _builder.Append(_buffer[_position]);
                    _position++;
                }

                return new Token(_builder.ToString(), Token.Category.Symbol);
            }

            if (char.IsNumber(_buffer[_position]))
            {
                _builder.Clear();
                while (_position < _end && (char.IsNumber(_buffer[_position]) || _buffer[_position] == '.'))
                {
                    _builder.Append(_buffer[_position]);
                    _position++;
                }

                return new Token(_builder.ToString(), Token.Category.Number);
            }

            if (char.IsLetter(_buffer[_position]))
            {
                _builder.Clear();
                while (_position < _end && char.IsLetter(_buffer[_position]))
                {
                    _builder.Append(_buffer[_position]);
                    _position++;
                }

                return new Token(_builder.ToString(), Token.Category.Word);
            }

            throw new ParserException("Unknown character: " + _buffer[_position]);
        }

        public Token Peek()
        {
            return _nextToken;
        }

        public Token Read()
        {
            var tokenToRead = _nextToken;
            _nextToken = ReadNextToken();
            return tokenToRead;
        }

        public void Expect(string expected)
        {
            var token = Read();

            if (token.Value != expected)
            {
                throw new ParserException(string.Format("Expected {0} but got {1}", expected, token.Value));
            }
        }
    }

    public class Token
    {
        private readonly string _value;
        private readonly Category _category;

        public Token(string value, Category category)
        {
            _value = value;
            _category = category;
        }

        public enum Category
        {
            Word, Number, Symbol,
            Null,
            String
        }

        public string Value
        {
            get { return _value; }
        }

        public Category Cat
        {
            get { return _category; }
        }

        public override string ToString()
        {
            return Value + " (" + Cat + ")";
        }
    }
}