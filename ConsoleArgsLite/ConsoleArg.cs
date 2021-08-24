using System;

namespace ConsoleArgsLite
{
    internal class ConsoleArg<T>
    {
        public ConsoleArg(string name, Func<string, T> parser, bool required, string consoleName = null)
        {
            Name = name;
            Parser = parser;
            Required = required;
            ConsoleName = consoleName;
        }

        public string Name { get; }
        public string ConsoleName { get; }
        public string ConsoleValue { get; }
        public T Value { get; private set; }
        public Func<string, T> Parser { get; }
        public bool Required { get; }

        public void Parse(string input) => Value = Parser(input);
    }
}